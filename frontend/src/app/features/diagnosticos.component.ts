import { CommonModule } from '@angular/common';

import {
  Component,
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
  DiagnosticoService,
  DiagnosticoWrite,
} from '../services/diagnostico.service';

import { NotificationService } from '../services/notification.service';

import { Diagnostico } from '../models/diagnostico.model';

import { DeleteConfirmComponent } from '../shared/delete-confirm.component';

@Component({
  selector: 'app-diagnosticos',

  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule,
    DeleteConfirmComponent,
  ],

  template: `
    <div class="page-head">
      <div>
        <h1>Diagnósticos</h1>
      </div>

      <button
        data-cy="new-diagnosis"
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
          diagnóstico
        </h2>

        <form
          [formGroup]="form"
          (ngSubmit)="save()"
        >
          <div class="grid">
            <label>
              Enfermedad

              <input
                data-cy="diagnosis-disease"
                formControlName="enfermedad"
              />
            </label>

            <label>
              Valoración especialista

              <textarea
                data-cy="diagnosis-assessment"
                formControlName="valoracionEspecialista"
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
              data-cy="save-diagnosis"
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
          <th>Enfermedad</th>
          <th>Valoración</th>
          <th>Acciones</th>
        </tr>
      </thead>

      <tbody>
        @for (
          diagnostico of service.diagnosticos();
          track diagnostico.id
        ) {
          <tr
            data-cy="diagnosis-row"
          >
            <td>
              {{ diagnostico.id }}
            </td>

            <td>
              {{
                diagnostico.enfermedad
              }}
            </td>

            <td>
              {{
                diagnostico.valoracionEspecialista
              }}
            </td>

            <td>
              <button
                class="link"
                (click)="
                  detail.set(
                    diagnostico
                  )
                "
              >
                Ver
              </button>

              <button
                class="link"
                (click)="
                  edit(diagnostico)
                "
              >
                Editar
              </button>

              <button
                class="link danger-text"
                (click)="
                  pendingDelete.set(
                    diagnostico
                  )
                "
              >
                Eliminar
              </button>
            </td>
          </tr>
        }
      </tbody>
    </table>

    @if (detail(); as diagnostico) {
      <section
        class="panel detail"
      >
        <h2>
          Detalle diagnóstico
          #{{ diagnostico.id }}
        </h2>

        <p>
          <b>Enfermedad:</b>

          {{
            diagnostico.enfermedad
          }}
        </p>

        <p>
          <b>
            Valoración especialista:
          </b>

          {{
            diagnostico.valoracionEspecialista
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
      [label]="
        pendingDelete()
          ?.enfermedad || ''
      "
      (cancel)="
        pendingDelete.set(null)
      "
      (confirm)="confirmDelete()"
    />
  `,
})
export class DiagnosticosComponent
  implements OnInit
{
  service =
    inject(DiagnosticoService);

  notify =
    inject(NotificationService);

  fb =
    inject(FormBuilder);

  formVisible =
    signal(false);

  editingId =
    signal<number | null>(null);

  detail =
    signal<Diagnostico | null>(
      null,
    );

  pendingDelete =
    signal<Diagnostico | null>(
      null,
    );

  form =
    this.fb.nonNullable.group({
      enfermedad: [
        '',
        Validators.required,
      ],

      valoracionEspecialista: [
        '',
        Validators.required,
      ],
    });

  ngOnInit(): void {
    this.service.load();
  }

  newItem(): void {
    this.editingId.set(null);

    this.form.reset();

    this.formVisible.set(true);
  }

  edit(
    diagnostico: Diagnostico,
  ): void {
    this.editingId.set(
      diagnostico.id,
    );

    this.form.setValue({
      enfermedad:
        diagnostico.enfermedad,

      valoracionEspecialista:
        diagnostico.valoracionEspecialista,
    });

    this.formVisible.set(true);
  }

  save(): void {
    if (this.form.invalid) {
      return;
    }

    const values =
      this.form.getRawValue();

    const data: DiagnosticoWrite = {
      enfermedad:
        values.enfermedad,

      valoracionEspecialista:
        values.valoracionEspecialista,
    };

    const id =
      this.editingId();

    if (id !== null) {
      this.service
        .update(id, data)
        .subscribe({
          next: () => {
            this.notify.success(
              'Diagnóstico actualizado correctamente',
            );

            this.closeForm();
          },

          error: (error) => {
            console.error(
              'Error actualizando diagnóstico',
              error,
            );

            this.notify.error(
              'Error al actualizar el diagnóstico',
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
            'Diagnóstico creado correctamente',
          );

          this.closeForm();
        },

        error: (error) => {
          console.error(
            'Error creando diagnóstico',
            error,
          );

          this.notify.error(
            'Error al crear el diagnóstico',
          );
        },
      });
  }

  closeForm(): void {
    this.formVisible.set(false);

    this.editingId.set(null);
  }

  confirmDelete(): void {
    const diagnostico =
      this.pendingDelete();

    if (!diagnostico) {
      return;
    }

    this.service
      .delete(diagnostico.id)
      .subscribe({
        next: () => {
          this.notify.success(
            'Diagnóstico eliminado',
          );

          this.pendingDelete.set(
            null,
          );

          if (
            this.detail()?.id ===
            diagnostico.id
          ) {
            this.detail.set(null);
          }
        },

        error: (error) => {
          console.error(
            'Error eliminando diagnóstico',
            error,
          );

          this.notify.error(
            'No se ha podido eliminar el diagnóstico',
          );
        },
      });
  }
}