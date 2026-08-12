import { CommonModule } from '@angular/common';

import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  OnInit,
  computed,
  inject,
  signal,
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';

import {
  PacienteService,
  PacienteWrite,
} from '../services/paciente.service';

import { MedicoService } from '../services/medico.service';

import { NotificationService } from '../services/notification.service';

import { Paciente } from '../models/paciente.model';

import { DeleteConfirmComponent } from '../shared/delete-confirm.component';

@Component({
  selector: 'app-pacientes',

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
        <h1>Pacientes</h1>
      </div>

      <button
        data-cy="new-patient"
        (click)="newItem()"
      >
        + Nuevo
      </button>
    </div>

    <label class="search">
      Buscar paciente

      <input
        data-cy="patient-search"
        [value]="search()"
        (input)="
          search.set(
            $any($event.target).value
          )
        "
        placeholder="Nombre, apellidos o NSS"
      />
    </label>

    @if (formVisible()) {
      <section class="panel">
        <h2>
          {{
            editingId()
              ? 'Editar'
              : 'Nuevo'
          }}
          paciente
        </h2>

        <form
          [formGroup]="form"
          (ngSubmit)="save()"
        >
          <div class="grid">
            <label>
              Nombre
              <input
                data-cy="patient-name"
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
              NSS
              <input
                data-cy="patient-nss"
                formControlName="nss"
              />
            </label>

            <label>
              Tarjeta
              <input
                formControlName="numTarjeta"
              />
            </label>

            <label>
              Teléfono
              <input
                formControlName="telefono"
              />
            </label>

            <label>
              Dirección
              <input
                formControlName="direccion"
              />
            </label>
          </div>

          <fieldset>
            <legend>
              Médicos relacionados
            </legend>

            @for (
              m of medicos.medicos();
              track m.id
            ) {
              <label class="check">
                <input
                  type="checkbox"
                  [checked]="
                    selectedDoctors()
                      .includes(m.id)
                  "
                  (change)="
                    toggleDoctor(
                      m.id,
                      $any(
                        $event.target
                      ).checked
                    )
                  "
                />

                {{ m.nombre }}
                {{ m.apellidos }}
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
              data-cy="save-patient"
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
          <th>Paciente</th>
          <th>NSS</th>
          <th>Médicos</th>
          <th>Acciones</th>
        </tr>
      </thead>

      <tbody>
        @for (
          x of filtered();
          track x.id
        ) {
          <tr
            data-cy="patient-row"
          >
            <td>
              {{ x.id }}
            </td>

            <td>
              {{ x.nombre }}
              {{ x.apellidos }}
            </td>

            <td>
              {{ x.nss }}
            </td>

            <td>
              {{ doctorNames(x) }}
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
          Detalle paciente #{{ x.id }}
        </h2>

        <paciente-resumen
          [attr.nombre]="
            x.nombre +
            ' ' +
            x.apellidos
          "
          [attr.nss]="x.nss"
        >
        </paciente-resumen>

        <p>
          <b>Nombre:</b>
          {{ x.nombre }}
          {{ x.apellidos }}
        </p>

        <p>
          <b>NSS:</b>
          {{ x.nss }}
        </p>

        <p>
          <b>Tarjeta:</b>
          {{ x.numTarjeta }}
        </p>

        <p>
          <b>Teléfono:</b>
          {{ x.telefono }}
        </p>

        <p>
          <b>Dirección:</b>
          {{ x.direccion }}
        </p>

        <p>
          <b>Médicos:</b>
          {{ doctorNames(x) }}
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
export class PacientesComponent
  implements OnInit
{
  service =
    inject(PacienteService);

  medicos =
    inject(MedicoService);

  notify =
    inject(NotificationService);

  fb =
    inject(FormBuilder);

  search = signal('');

  filtered = computed(() =>
    this.service.search(
      this.search(),
    ),
  );

  formVisible =
    signal(false);

  editingId =
    signal<number | null>(null);

  detail =
    signal<Paciente | null>(null);

  pendingDelete =
    signal<Paciente | null>(null);

  selectedDoctors =
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

      nss: [
        '',
        Validators.required,
      ],

      numTarjeta: [
        '',
        Validators.required,
      ],

      telefono: [
        '',
        [
          Validators.required,
          Validators.pattern(
            /^[679]\d{8}$/,
          ),
        ],
      ],

      direccion: [
        '',
        Validators.required,
      ],
    });

  ngOnInit(): void {
    this.service.load();
    this.medicos.load();
  }

  newItem(): void {
    this.editingId.set(null);

    this.selectedDoctors.set([]);

    this.form.reset();

    this.formVisible.set(true);
  }

  edit(
    paciente: Paciente,
  ): void {
    this.editingId.set(
      paciente.id,
    );

    this.selectedDoctors.set([
      ...paciente.medicoIds,
    ]);

    this.form.setValue({
      nombre:
        paciente.nombre,

      apellidos:
        paciente.apellidos,

      usuario:
        paciente.usuario,

      /*
        * La API no devuelve la clave.
        * Si se deja vacía al editar,
        * el backend conserva la clave actual.
       */
      clave: '',

      nss:
        paciente.nss,

      numTarjeta:
        paciente.numTarjeta,

      telefono:
        paciente.telefono,

      direccion:
        paciente.direccion,
    });

    this.formVisible.set(true);
  }

  toggleDoctor(
    id: number,
    selected: boolean,
  ): void {
    this.selectedDoctors.update(
      (doctors) =>
        selected
          ? doctors.includes(id)
            ? doctors
            : [...doctors, id]
          : doctors.filter(
              (doctorId) =>
                doctorId !== id,
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
      'La clave es obligatoria al crear un paciente',
    );

    return;
  }

  const data: PacienteWrite = {
    nombre:
      values.nombre,

    apellidos:
      values.apellidos,

    usuario:
      values.usuario,

    clave:
      values.clave,

    nss:
      values.nss,

    numTarjeta:
      values.numTarjeta,

    telefono:
      values.telefono,

    direccion:
      values.direccion,

    medicoIds:
      this.selectedDoctors(),
  };

  const id =
    this.editingId();

  if (id !== null) {
    this.service
      .update(id, data)
      .subscribe({
        next: () => {
          this.notify.success(
            'Paciente actualizado correctamente',
          );

          this.closeForm();
        },

        error: (error) => {
          console.error(
            'Error actualizando paciente',
            error,
          );

          this.notify.error(
            'Error al actualizar el paciente',
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
          'Paciente creado correctamente',
        );

        this.closeForm();
      },

      error: (error) => {
        console.error(
          'Error creando paciente',
          error,
        );

        this.notify.error(
          'Error al crear el paciente',
        );
      },
    });

  }

  doctorNames(
    paciente: Paciente,
  ): string {
    return (
      paciente.medicoIds
        .map((id) =>
          this.medicos.findById(id),
        )
        .filter(
          (medico) =>
            medico !== undefined,
        )
        .map(
          (medico) =>
            `${medico!.nombre} ${medico!.apellidos}`,
        )
        .join(', ') ||
      'Sin asignar'
    );
  }

  confirmDelete(): void {
    const paciente =
      this.pendingDelete();

    if (!paciente) {
      return;
    }

    this.service
      .delete(paciente.id)
      .subscribe({
        next: () => {
          this.notify.success(
            'Paciente eliminado',
          );

          this.pendingDelete.set(
            null,
          );

          if (
            this.detail()?.id ===
            paciente.id
          ) {
            this.detail.set(null);
          }
        },

        error: (error) => {
          console.error(
            'Error eliminando paciente',
            error,
          );

          this.notify.error(
            'No se ha podido eliminar el paciente',
          );
        },
      });
  }
}