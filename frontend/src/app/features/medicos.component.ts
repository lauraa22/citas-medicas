import { CommonModule } from '@angular/common';

import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  OnInit,
  inject,
  signal,
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import {
  MedicoService,
  MedicoWrite,
} from '../services/medico.service';

import { PacienteService } from '../services/paciente.service';

import { NotificationService } from '../services/notification.service';

import { Medico } from '../models/medico.model';

import { DeleteConfirmComponent } from '../shared/delete-confirm.component';

@Component({
  selector: 'app-medicos',

  standalone: true,

  schemas: [
    CUSTOM_ELEMENTS_SCHEMA,
  ],

  imports: [
    CommonModule,
    ReactiveFormsModule,
    DeleteConfirmComponent,
  ],

  template: `
    <div class="page-head">
      <div>
        <h1>Médicos</h1>
      </div>

      <button
        data-cy="new-doctor"
        (click)="newItem()"
      >
        + Nuevo
      </button>
    </div>

    @if (formVisible()) {
      <section class="panel">
        <h2>
          {{
            editingId()
              ? 'Editar'
              : 'Nuevo'
          }}
          médico
        </h2>

        <form
          [formGroup]="form"
          (ngSubmit)="save()"
        >
          <div class="grid">
            <label>
              Nombre

              <input
                data-cy="doctor-name"
                formControlName="nombre"
              />
            </label>

            <label>
              Apellidos

              <input
                formControlName="apellidos"
              />
            </label>

            <label>
              Usuario

              <input
                formControlName="usuario"
              />
            </label>

            <label>
              Clave

              <input
                type="password"
                formControlName="clave"
              />
            </label>

            <label>
              Nº colegiado

              <input
                data-cy="doctor-license"
                formControlName="numColegiado"
              />
            </label>
          </div>

          <fieldset>
            <legend>
              Pacientes relacionados
            </legend>

            @for (
              p of pacientes.pacientes();
              track p.id
            ) {
              <label class="check">
                <input
                  type="checkbox"
                  [checked]="
                    selectedPatients()
                      .includes(p.id)
                  "
                  (change)="
                    togglePatient(
                      p.id,
                      $any(
                        $event.target
                      ).checked
                    )
                  "
                />

                {{ p.nombre }}
                {{ p.apellidos }}
              </label>
            }
          </fieldset>

          <div class="actions">
            <button
              type="button"
              class="secondary"
              (click)="closeForm()"
            >
              Cancelar
            </button>

            <button
              data-cy="save-doctor"
              [disabled]="form.invalid"
            >
              Guardar
            </button>
          </div>
        </form>
      </section>
    }

    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Médico</th>
          <th>Colegiado</th>
          <th>Pacientes</th>
          <th>Acciones</th>
        </tr>
      </thead>

      <tbody>
        @for (
          x of service.medicos();
          track x.id
        ) {
          <tr>
            <td>
              {{ x.id }}
            </td>

            <td>
              {{ x.nombre }}
              {{ x.apellidos }}
            </td>

            <td>
              {{ x.numColegiado }}
            </td>

            <td>
              {{ patientNames(x) }}
            </td>

            <td>
              <button
                class="link"
                (click)="detail.set(x)"
              >
                Ver
              </button>

              <button
                class="link"
                (click)="edit(x)"
              >
                Editar
              </button>

              <button
                class="link danger-text"
                (click)="
                  pendingDelete.set(x)
                "
              >
                Eliminar
              </button>
            </td>
          </tr>
        }
      </tbody>
    </table>

    @if (detail(); as x) {
      <section
        class="panel detail"
      >
        <h2>
          Detalle médico #{{ x.id }}
        </h2>

        <medico-resumen
          [attr.nombre]="
            x.nombre +
            ' ' +
            x.apellidos
          "
          [attr.colegiado]="
            x.numColegiado
          "
        >
        </medico-resumen>

        <p>
          <b>Nombre:</b>
          {{ x.nombre }}
          {{ x.apellidos }}
        </p>

        <p>
          <b>Colegiado:</b>
          {{ x.numColegiado }}
        </p>

        <p>
          <b>Pacientes:</b>
          {{ patientNames(x) }}
        </p>

        <button
          class="secondary"
          (click)="detail.set(null)"
        >
          Cerrar
        </button>
      </section>
    }

    <app-delete-confirm
      [open]="!!pendingDelete()"
      [label]="
        pendingDelete()?.nombre ||
        ''
      "
      (cancel)="
        pendingDelete.set(null)
      "
      (confirm)="confirmDelete()"
    />
  `,
})
export class MedicosComponent
  implements OnInit
{
  service =
    inject(MedicoService);

  pacientes =
    inject(PacienteService);

  notify =
    inject(NotificationService);

  fb =
    inject(FormBuilder);

  formVisible =
    signal(false);

  editingId =
    signal<number | null>(null);

  detail =
    signal<Medico | null>(null);

  pendingDelete =
    signal<Medico | null>(null);

  selectedPatients =
    signal<number[]>([]);

  form =
    this.fb.nonNullable.group({
      nombre: [
        '',
        Validators.required,
      ],

      apellidos: [
        '',
        Validators.required,
      ],

      usuario: [
        '',
        Validators.required,
      ],

      clave: [
        ''
      ],

      numColegiado: [
        '',
        Validators.required,
      ],
    });

  ngOnInit(): void {
    this.service.load();
    this.pacientes.load();
  }

  newItem(): void {
    this.editingId.set(null);

    this.selectedPatients.set([]);

    this.form.reset();

    this.formVisible.set(true);
  }

  edit(
    medico: Medico,
  ): void {
    this.editingId.set(
      medico.id,
    );

    this.selectedPatients.set([
      ...medico.pacienteIds,
    ]);

    this.form.setValue({
      nombre:
        medico.nombre,

      apellidos:
        medico.apellidos,

      usuario:
        medico.usuario,

      /*
       * El backend no devuelve
       * la clave del médico.
       */
      clave: '',

      numColegiado:
        medico.numColegiado,
    });

    this.formVisible.set(true);
  }

  togglePatient(
    id: number,
    selected: boolean,
  ): void {
    this.selectedPatients.update(
      (patients) =>
        selected
          ? patients.includes(id)
            ? patients
            : [...patients, id]
          : patients.filter(
              (patientId) =>
                patientId !== id,
            ),
    );
  }

  closeForm(): void {
    this.formVisible.set(false);
    this.editingId.set(null);
  }

  save(): void {
  if (this.form.invalid) {
    return;
  }

  const values =
    this.form.getRawValue();

  // Al crear, la clave es obligatoria.
  // Al editar, puede quedar vacía para conservar la actual.
  if (
    this.editingId() === null &&
    !values.clave.trim()
  ) {
    this.notify.error(
      'La clave es obligatoria al crear un médico',
    );

    return;
  }

  const data: MedicoWrite = {
    nombre:
      values.nombre,

    apellidos:
      values.apellidos,

    usuario:
      values.usuario,

    clave:
      values.clave,

    numColegiado:
      values.numColegiado,

    pacienteIds:
      this.selectedPatients(),
  };

  const id =
    this.editingId();

  if (id !== null) {
    this.service
      .update(id, data)
      .subscribe({
        next: () => {
          this.notify.success(
            'Médico actualizado correctamente',
          );

          this.closeForm();
        },

        error: (error) => {
          console.error(
            'Error actualizando médico',
            error,
          );

          this.notify.error(
            'Error al actualizar el médico',
          );
        },
      });

    return;
  }

  this.service
    .create(data)
    .subscribe({
      next: () => {
        this.notify.success(
          'Médico creado correctamente',
        );

        this.closeForm();
      },

      error: (error) => {
        console.error(
          'Error creando médico',
          error,
        );

        this.notify.error(
          'Error al crear el médico',
        );
      },
    });

  }

  patientNames(
    medico: Medico,
  ): string {
    return (
      medico.pacienteIds
        .map((id) =>
          this.pacientes.findById(id),
        )
        .filter(
          (paciente) =>
            paciente !== undefined,
        )
        .map(
          (paciente) =>
            `${paciente!.nombre} ${paciente!.apellidos}`,
        )
        .join(', ') ||
      'Sin asignar'
    );
  }

  confirmDelete(): void {
    const medico =
      this.pendingDelete();

    if (!medico) {
      return;
    }

    this.service
      .delete(medico.id)
      .subscribe({
        next: () => {
          this.notify.success(
            'Médico eliminado',
          );

          this.pendingDelete.set(
            null,
          );

          if (
            this.detail()?.id ===
            medico.id
          ) {
            this.detail.set(null);
          }
        },

        error: (error) => {
          console.error(
            'Error eliminando médico',
            error,
          );

          this.notify.error(
            'No se ha podido eliminar el médico',
          );
        },
      });
  }
}