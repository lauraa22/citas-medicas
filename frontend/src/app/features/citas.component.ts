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
  CitaService,
  CitaWrite,
} from '../services/cita.service';

import { PacienteService } from '../services/paciente.service';
import { MedicoService } from '../services/medico.service';
import { DiagnosticoService } from '../services/diagnostico.service';
import { NotificationService } from '../services/notification.service';

import { Cita } from '../models/cita.model';

import { DeleteConfirmComponent } from '../shared/delete-confirm.component';

@Component({
  selector: 'app-citas',

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
        <h1>Citas</h1>
      </div>

      <button
        data-cy="new-appointment"
        (click)="newItem()"
      >
        + Nueva
      </button>
    </div>

    <label class="search">
      Filtrar por médico

      <select
        data-cy="appointment-doctor-filter"
        [value]="doctorFilter() ?? ''"
        (change)="
          changeDoctorFilter(
            $any($event.target).value
          )
        "
      >
        <option value="">
          Todos los médicos
        </option>

        @for (
          medico of medicos.medicos();
          track medico.id
        ) {
          <option
            [value]="medico.id"
          >
            {{ medico.nombre }}
            {{ medico.apellidos }}
          </option>
        }
      </select>
    </label>

    @if (formVisible()) {
      <section class="panel">
        <h2>
          {{
            editingId()
              ? 'Editar'
              : 'Nueva'
          }}
          cita
        </h2>

        <form
          [formGroup]="form"
          (ngSubmit)="save()"
        >
          <div class="grid">
            <label>
              Fecha y hora

              <input
                data-cy="appointment-date"
                type="datetime-local"
                formControlName="fechaHora"
              />
            </label>

            <label>
              Motivo

              <input
                data-cy="appointment-reason"
                formControlName="motivoCita"
              />
            </label>

            <label>
              Paciente

              <select
                data-cy="appointment-patient"
                formControlName="pacienteId"
              >
                <option value="">
                  Selecciona paciente
                </option>

                @for (
                  paciente of pacientes.pacientes();
                  track paciente.id
                ) {
                  <option
                    [value]="paciente.id"
                  >
                    {{ paciente.nombre }}
                    {{ paciente.apellidos }}
                  </option>
                }
              </select>
            </label>

            <label>
              Médico

              <select
                data-cy="appointment-doctor"
                formControlName="medicoId"
              >
                <option value="">
                  Selecciona médico
                </option>

                @for (
                  medico of medicos.medicos();
                  track medico.id
                ) {
                  <option
                    [value]="medico.id"
                  >
                    {{ medico.nombre }}
                    {{ medico.apellidos }}
                  </option>
                }
              </select>
            </label>

            <label>
              Diagnóstico

              <select
                data-cy="appointment-diagnosis"
                formControlName="diagnosticoId"
              >
                <option value="">
                  Sin diagnóstico
                </option>

                @for (
                  diagnostico of diagnosticos.diagnosticos();
                  track diagnostico.id
                ) {
                  <option
                    [value]="diagnostico.id"
                  >
                    {{ diagnostico.enfermedad }}
                  </option>
                }
              </select>
            </label>
          </div>

          <div class="actions">
            <button
              type="button"
              class="secondary"
              (click)="closeForm()"
            >
              Cancelar
            </button>

            <button
              data-cy="save-appointment"
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
          <th>Fecha</th>
          <th>Paciente</th>
          <th>Médico</th>
          <th>Motivo</th>
          <th>Diagnóstico</th>
          <th>Acciones</th>
        </tr>
      </thead>

      <tbody>
        @for (
          cita of filtered();
          track cita.id
        ) {
          <tr
            data-cy="appointment-row"
          >
            <td>
              {{ cita.id }}
            </td>

            <td>
              {{
                cita.fechaHora
                  | date:
                    'dd/MM/yyyy HH:mm'
              }}
            </td>

            <td>
              {{
                patientName(
                  cita.pacienteId
                )
              }}
            </td>

            <td>
              {{
                doctorName(
                  cita.medicoId
                )
              }}
            </td>

            <td>
              {{ cita.motivoCita }}
            </td>

            <td>
              {{
                diagnosisName(
                  cita.diagnosticoId
                )
              }}
            </td>

            <td>
              <button
                class="link"
                (click)="detail.set(cita)"
              >
                Ver
              </button>

              <button
                class="link"
                (click)="edit(cita)"
              >
                Editar
              </button>

              <button
                class="link danger-text"
                (click)="
                  pendingDelete.set(cita)
                "
              >
                Eliminar
              </button>
            </td>
          </tr>
        }
      </tbody>
    </table>

    @if (detail(); as cita) {
      <section
        class="panel detail"
      >
        <h2>
          Detalle cita #{{ cita.id }}
        </h2>

        <cita-resumen
          [attr.fecha]="
            cita.fechaHora
          "
          [attr.texto]="
            cita.motivoCita
          "
        >
        </cita-resumen>

        <p>
          <b>Fecha:</b>

          {{
            cita.fechaHora
              | date:
                'dd/MM/yyyy HH:mm'
          }}
        </p>

        <p>
          <b>Paciente:</b>

          {{
            patientName(
              cita.pacienteId
            )
          }}
        </p>

        <p>
          <b>Médico:</b>

          {{
            doctorName(
              cita.medicoId
            )
          }}
        </p>

        <p>
          <b>Motivo:</b>

          {{ cita.motivoCita }}
        </p>

        <p>
          <b>Diagnóstico:</b>

          {{
            diagnosisName(
              cita.diagnosticoId
            )
          }}
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
      label="esta cita"
      (cancel)="
        pendingDelete.set(null)
      "
      (confirm)="confirmDelete()"
    />
  `,
})
export class CitasComponent
  implements OnInit
{
  service =
    inject(CitaService);

  pacientes =
    inject(PacienteService);

  medicos =
    inject(MedicoService);

  diagnosticos =
    inject(DiagnosticoService);

  notify =
    inject(NotificationService);

  fb =
    inject(FormBuilder);

  doctorFilter =
    signal<number | null>(null);

  filtered = computed(() =>
    this.service.byDoctor(
      this.doctorFilter(),
    ),
  );

  formVisible =
    signal(false);

  editingId =
    signal<number | null>(null);

  detail =
    signal<Cita | null>(null);

  pendingDelete =
    signal<Cita | null>(null);

  form =
    this.fb.nonNullable.group({
      fechaHora: [
        '',
        Validators.required,
      ],

      motivoCita: [
        '',
        Validators.required,
      ],

      pacienteId: [
        '',
        Validators.required,
      ],

      medicoId: [
        '',
        Validators.required,
      ],

      diagnosticoId: [''],
    });

  ngOnInit(): void {
    this.service.load();

    this.pacientes.load();

    this.medicos.load();

    this.diagnosticos.load();
  }

  newItem(): void {
    this.editingId.set(null);

    this.form.reset();

    this.formVisible.set(true);
  }

  edit(
    cita: Cita,
  ): void {
    this.editingId.set(
      cita.id,
    );

    this.form.setValue({
      fechaHora:
        this.toDateTimeInput(
          cita.fechaHora,
        ),

      motivoCita:
        cita.motivoCita,

      pacienteId:
        String(cita.pacienteId),

      medicoId:
        String(cita.medicoId),

      diagnosticoId:
        cita.diagnosticoId ===
        null
          ? ''
          : String(
              cita.diagnosticoId,
            ),
    });

    this.formVisible.set(true);
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    const values =
      this.form.getRawValue();

    const data: CitaWrite = {
      fechaHora:
        values.fechaHora,

      motivoCita:
        values.motivoCita,

      pacienteId:
        Number(
          values.pacienteId,
        ),

      medicoId:
        Number(
          values.medicoId,
        ),

      diagnosticoId:
        values.diagnosticoId
          ? Number(
              values.diagnosticoId,
            )
          : null,
    };

    const id =
      this.editingId();

    if (id !== null) {
      this.service
        .update(id, data)
        .subscribe({
          next: () => {
            this.notify.success(
              'Cita actualizada correctamente',
            );

            this.closeForm();
          },

          error: (error) => {
            console.error(
              'Error actualizando cita',
              error,
            );

            this.notify.error(
              'Error al actualizar la cita',
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
            'Cita creada correctamente',
          );

          this.closeForm();
        },

        error: (error) => {
          console.error(
            'Error creando cita',
            error,
          );

          this.notify.error(
            'Error al crear la cita',
          );
        },
      });
  }

  closeForm(): void {
    this.formVisible.set(false);

    this.editingId.set(null);
  }

  changeDoctorFilter(
    value: string,
  ): void {
    this.doctorFilter.set(
      value
        ? Number(value)
        : null,
    );
  }

  patientName(
    id: number,
  ): string {
    const paciente =
      this.pacientes.findById(id);

    return paciente
      ? `${paciente.nombre} ${paciente.apellidos}`
      : '-';
  }

  doctorName(
    id: number,
  ): string {
    const medico =
      this.medicos.findById(id);

    return medico
      ? `${medico.nombre} ${medico.apellidos}`
      : '-';
  }

  diagnosisName(
    id: number | null,
  ): string {
    if (id === null) {
      return 'Sin diagnóstico';
    }

    const diagnostico =
      this.diagnosticos.findById(
        id,
      );

    return diagnostico
      ? diagnostico.enfermedad
      : 'Sin diagnóstico';
  }

  confirmDelete(): void {
    const cita =
      this.pendingDelete();

    if (!cita) {
      return;
    }

    this.service
      .delete(cita.id)
      .subscribe({
        next: () => {
          this.notify.success(
            'Cita eliminada',
          );

          this.pendingDelete.set(
            null,
          );

          if (
            this.detail()?.id ===
            cita.id
          ) {
            this.detail.set(null);
          }
        },

        error: (error) => {
          console.error(
            'Error eliminando cita',
            error,
          );

          this.notify.error(
            'No se ha podido eliminar la cita',
          );
        },
      });
  }

  private toDateTimeInput(
    value: string | Date,
  ): string {
    const date =
      new Date(value);

    const year =
      date.getFullYear();

    const month =
      String(
        date.getMonth() + 1,
      ).padStart(2, '0');

    const day =
      String(
        date.getDate(),
      ).padStart(2, '0');

    const hours =
      String(
        date.getHours(),
      ).padStart(2, '0');

    const minutes =
      String(
        date.getMinutes(),
      ).padStart(2, '0');

    return (
      `${year}-${month}-${day}` +
      `T${hours}:${minutes}`
    );
  }
}