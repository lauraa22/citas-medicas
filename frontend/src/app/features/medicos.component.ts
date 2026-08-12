import { CommonModule } from '@angular/common';

import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  inject,
  signal
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { MedicoService } from '../services/medico.service';
import { PacienteService } from '../services/paciente.service';
import { NotificationService } from '../services/notification.service';

import { Medico } from '../models/medico.model';

import { DeleteConfirmComponent } from '../shared/delete-confirm.component';

@Component({
  selector: 'app-medicos',

  standalone: true,

  schemas: [CUSTOM_ELEMENTS_SCHEMA],

  imports: [
    CommonModule,
    ReactiveFormsModule,
    DeleteConfirmComponent
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
          {{ editingId() ? 'Editar' : 'Nuevo' }} médico
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
                  [checked]="selectedPatients().includes(p.id)"
                  (change)="
                    togglePatient(
                      p.id,
                      $any($event.target).checked
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
                (click)="pendingDelete.set(x)"
              >
                Eliminar
              </button>
            </td>
          </tr>
        }
      </tbody>
    </table>

    @if (detail(); as x) {
      <section class="panel detail">
        <h2>
          Detalle médico #{{ x.id }}
        </h2>

        <medico-resumen
          [attr.nombre]="
            x.nombre + ' ' + x.apellidos
          "
          [attr.colegiado]="x.numColegiado"
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
        pendingDelete()?.nombre || ''
      "
      (cancel)="pendingDelete.set(null)"
      (confirm)="confirmDelete()"
    />
  `
})
export class MedicosComponent {

  service = inject(MedicoService);

  pacientes = inject(PacienteService);

  notify = inject(NotificationService);

  fb = inject(FormBuilder);

  formVisible = signal(false);

  editingId = signal<number | null>(null);

  detail = signal<Medico | null>(null);

  pendingDelete =
    signal<Medico | null>(null);

  selectedPatients =
    signal<number[]>([]);

  form = this.fb.nonNullable.group({
    nombre: [
      '',
      Validators.required
    ],

    apellidos: [
      '',
      Validators.required
    ],

    usuario: [
      '',
      Validators.required
    ],

    clave: [
      '',
      Validators.required
    ],

    numColegiado: [
      '',
      Validators.required
    ]
  });

  newItem() {
    this.editingId.set(null);

    this.selectedPatients.set([]);

    this.form.reset();

    this.formVisible.set(true);
  }

  edit(x: Medico) {
    this.editingId.set(x.id);

    this.selectedPatients.set([
      ...x.pacienteIds
    ]);

    this.form.setValue({
      nombre: x.nombre,
      apellidos: x.apellidos,
      usuario: x.usuario,
      clave: x.clave,
      numColegiado: x.numColegiado
    });

    this.formVisible.set(true);
  }

  togglePatient(
    id: number,
    on: boolean
  ) {
    this.selectedPatients.update(
      pacientes =>
        on
          ? [...pacientes, id]
          : pacientes.filter(
              pacienteId =>
                pacienteId !== id
            )
    );
  }

  closeForm() {
    this.formVisible.set(false);
  }

  save() {
    if (this.form.invalid) {
      return;
    }

    const v =
      this.form.getRawValue();

    const id =
      this.editingId();

    const medico =
      new Medico(
        id ?? 0,
        v.nombre,
        v.apellidos,
        v.usuario,
        v.clave,
        v.numColegiado,
        this.selectedPatients()
      );

    if (id) {
      this.service.update(medico);
    } else {
      this.service.create(
        medico as any
      );
    }

    this.notify.success(
      'Médico guardado correctamente'
    );

    this.closeForm();
  }

  patientNames(
    medico: Medico
  ) {
    return (
      medico.pacienteIds
        .map(
          id =>
            this.pacientes.getById(id)
        )
        .filter(Boolean)
        .map(
          paciente =>
            `${paciente!.nombre} ${paciente!.apellidos}`
        )
        .join(', ') ||
      'Sin asignar'
    );
  }

  confirmDelete() {
    const medico =
      this.pendingDelete();

    if (medico) {
      this.service.delete(
        medico.id
      );

      this.notify.success(
        'Médico eliminado'
      );

      this.pendingDelete.set(null);
    }
  }
}