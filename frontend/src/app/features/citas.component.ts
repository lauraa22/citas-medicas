import { CommonModule } from '@angular/common';

import {
  Component,
  CUSTOM_ELEMENTS_SCHEMA,
  computed,
  inject,
  signal
} from '@angular/core';

import {
  FormBuilder,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';

import { CitaService } from '../services/cita.service';
import { PacienteService } from '../services/paciente.service';
import { MedicoService } from '../services/medico.service';
import { DiagnosticoService } from '../services/diagnostico.service';
import { NotificationService } from '../services/notification.service';

import { Cita } from '../models/cita.model';

import { DeleteConfirmComponent } from '../shared/delete-confirm.component';

@Component({
  selector: 'app-citas',

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
        data-cy="appointment-filter"
        [value]="doctorFilter() ?? ''"
        (change)="setFilter($any($event.target).value)"
      >
        <option value="">
          Todos
        </option>

        @for (
          m of medicos.medicos();
          track m.id
        ) {
          <option [value]="m.id">
            {{ m.nombre }}
            {{ m.apellidos }}
          </option>
        }
      </select>
    </label>

    @if (formVisible()) {
      <section class="panel">
        <h2>
          {{ editingId() ? 'Editar' : 'Nueva' }}
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
              Paciente

              <select
                data-cy="appointment-patient"
                formControlName="pacienteId"
              >
                <option value="">
                  Selecciona
                </option>

                @for (
                  p of pacientes.pacientes();
                  track p.id
                ) {
                  <option [value]="p.id">
                    {{ p.nombre }}
                    {{ p.apellidos }}
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
                  Selecciona
                </option>

                @for (
                  m of medicos.medicos();
                  track m.id
                ) {
                  <option [value]="m.id">
                    {{ m.nombre }}
                    {{ m.apellidos }}
                  </option>
                }
              </select>
            </label>

            <label>
              Diagnóstico (opcional)

              <select
                formControlName="diagnosticoId"
              >
                <option value="">
                  Sin diagnóstico
                </option>

                @for (
                  d of diagnosticos.diagnosticos();
                  track d.id
                ) {
                  <option [value]="d.id">
                    {{ d.enfermedad }}
                  </option>
                }
              </select>
            </label>

            <label class="full">
              Motivo

              <textarea
                data-cy="appointment-reason"
                rows="2"
                formControlName="motivoCita"
              >
              </textarea>
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
          x of filtered();
          track x.id
        ) {
          <tr data-cy="appointment-row">

            <td>
              {{ x.id }}
            </td>

            <td>
              {{ x.fechaHora | date: 'dd/MM/yyyy HH:mm' }}
            </td>

            <td>
              {{ patientName(x.pacienteId) }}
            </td>

            <td>
              {{ doctorName(x.medicoId) }}
            </td>

            <td>
              {{ x.motivoCita }}
            </td>

            <td>
              {{ diagnosisName(x.diagnosticoId) }}
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
          Detalle cita #{{ x.id }}
        </h2>

        <cita-resumen
          [attr.fecha]="x.fechaHora"
          [attr.texto]="x.motivoCita"
        >
        </cita-resumen>

        <p>
          <b>Fecha:</b>
          {{ x.fechaHora | date: 'dd/MM/yyyy HH:mm' }}
        </p>

        <p>
          <b>Paciente:</b>
          {{ patientName(x.pacienteId) }}
        </p>

        <p>
          <b>Médico:</b>
          {{ doctorName(x.medicoId) }}
        </p>

        <p>
          <b>Motivo:</b>
          {{ x.motivoCita }}
        </p>

        <p>
          <b>Diagnóstico:</b>
          {{ diagnosisName(x.diagnosticoId) }}
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
      (cancel)="pendingDelete.set(null)"
      (confirm)="confirmDelete()"
    />
  `
})
export class CitasComponent {

  service = inject(CitaService);

  pacientes = inject(PacienteService);

  medicos = inject(MedicoService);

  diagnosticos = inject(DiagnosticoService);

  notify = inject(NotificationService);

  fb = inject(FormBuilder);

  doctorFilter =
    signal<number | null>(null);

  filtered = computed(() =>
    this.service.byDoctor(
      this.doctorFilter()
    )
  );

  formVisible =
    signal(false);

  editingId =
    signal<number | null>(null);

  detail =
    signal<Cita | null>(null);

  pendingDelete =
    signal<Cita | null>(null);

  form = this.fb.group({
    fechaHora: [
      '',
      Validators.required
    ],

    motivoCita: [
      '',
      Validators.required
    ],

    pacienteId: [
      '',
      Validators.required
    ],

    medicoId: [
      '',
      Validators.required
    ],

    diagnosticoId: ['']
  });

  setFilter(v: string) {
    this.doctorFilter.set(
      v ? Number(v) : null
    );
  }

  newItem() {
    this.editingId.set(null);

    this.form.reset({
      fechaHora: '',
      motivoCita: '',
      pacienteId: '',
      medicoId: '',
      diagnosticoId: ''
    });

    this.formVisible.set(true);
  }

  edit(x: Cita) {
    this.editingId.set(x.id);

    this.form.setValue({
      fechaHora: x.fechaHora,

      motivoCita: x.motivoCita,

      pacienteId:
        String(x.pacienteId),

      medicoId:
        String(x.medicoId),

      diagnosticoId:
        x.diagnosticoId
          ? String(x.diagnosticoId)
          : ''
    });

    this.formVisible.set(true);
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

    const cita =
      new Cita(
        id ?? 0,
        v.fechaHora!,
        v.motivoCita!,
        Number(v.pacienteId),
        Number(v.medicoId),
        v.diagnosticoId
          ? Number(v.diagnosticoId)
          : null
      );

    if (id) {
      this.service.update(cita);
    } else {
      this.service.create(
        cita as any
      );
    }

    this.notify.success(
      'Cita guardada correctamente'
    );

    this.closeForm();
  }

  patientName(id: number) {
    const paciente =
      this.pacientes.getById(id);

    return paciente
      ? `${paciente.nombre} ${paciente.apellidos}`
      : '-';
  }

  doctorName(id: number) {
    const medico =
      this.medicos.getById(id);

    return medico
      ? `${medico.nombre} ${medico.apellidos}`
      : '-';
  }

  diagnosisName(
    id: number | null
  ) {
    return id
      ? (
          this.diagnosticos
            .getById(id)
            ?.enfermedad ?? '-'
        )
      : 'Sin diagnóstico';
  }

  confirmDelete() {
    const cita =
      this.pendingDelete();

    if (cita) {
      this.service.delete(
        cita.id
      );

      this.notify.success(
        'Cita eliminada'
      );

      this.pendingDelete.set(null);
    }
  }
}