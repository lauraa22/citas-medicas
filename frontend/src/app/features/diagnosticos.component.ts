import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { DiagnosticoService } from '../services/diagnostico.service';
import { NotificationService } from '../services/notification.service';
import { Diagnostico } from '../models/diagnostico.model';
import { DeleteConfirmComponent } from '../shared/delete-confirm.component';
@Component({
  selector: 'app-diagnosticos',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DeleteConfirmComponent],
  template: `
    <div class="page-head">
      <div>
        <h1>Diagnósticos</h1>
      </div>
      <button data-cy="new-diagnosis" (click)="newItem()">+ Nuevo</button>
    </div>
    @if (formVisible()) {
      <section class="panel">
        <h2>{{ editingId() ? 'Editar' : 'Nuevo' }} diagnóstico</h2>
        <form [formGroup]="form" (ngSubmit)="save()">
          <div class="grid">
            <label>Enfermedad<input data-cy="diagnosis-name" formControlName="enfermedad" /></label
            ><label class="full"
              >Valoración<textarea
                data-cy="diagnosis-value"
                rows="3"
                formControlName="valoracionEspecialista"
              ></textarea>
            </label>
          </div>
          <div class="actions">
            <button type="button" class="secondary" (click)="closeForm()">Cancelar</button
            ><button data-cy="save-diagnosis" [disabled]="form.invalid">Guardar</button>
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
        @for (x of service.diagnosticos(); track x.id) {
          <tr>
            <td>{{ x.id }}</td>
            <td>{{ x.enfermedad }}</td>
            <td>{{ x.valoracionEspecialista }}</td>
            <td>
              <button class="link" (click)="detail.set(x)">Ver</button
              ><button class="link" (click)="edit(x)">Editar</button
              ><button class="link danger-text" (click)="pendingDelete.set(x)">Eliminar</button>
            </td>
          </tr>
        }
      </tbody>
    </table>
    @if (detail(); as x) {
      <section class="panel detail">
        <h2>Detalle diagnóstico #{{ x.id }}</h2>
        <p><b>Enfermedad:</b> {{ x.enfermedad }}</p>
        <p><b>Valoración:</b> {{ x.valoracionEspecialista }}</p>
        <button class="secondary" (click)="detail.set(null)">Cerrar</button>
      </section>
    }
    <app-delete-confirm
      [open]="!!pendingDelete()"
      [label]="pendingDelete()?.enfermedad || ''"
      (cancel)="pendingDelete.set(null)"
      (confirm)="confirmDelete()"
    />
  `,
})
export class DiagnosticosComponent {
  service = inject(DiagnosticoService);
  notify = inject(NotificationService);
  fb = inject(FormBuilder);
  formVisible = signal(false);
  editingId = signal<number | null>(null);
  detail = signal<Diagnostico | null>(null);
  pendingDelete = signal<Diagnostico | null>(null);
  form = this.fb.nonNullable.group({
    enfermedad: ['', Validators.required],
    valoracionEspecialista: ['', Validators.required],
  });
  newItem() {
    this.editingId.set(null);
    this.form.reset();
    this.formVisible.set(true);
  }
  edit(x: Diagnostico) {
    this.editingId.set(x.id);
    this.form.setValue({
      enfermedad: x.enfermedad,
      valoracionEspecialista: x.valoracionEspecialista,
    });
    this.formVisible.set(true);
  }
  closeForm() {
    this.formVisible.set(false);
  }
  save() {
    if (this.form.invalid) return;
    const v = this.form.getRawValue();
    const id = this.editingId();
    if (id) this.service.update(new Diagnostico(id, v.valoracionEspecialista, v.enfermedad));
    else this.service.create(v as any);
    this.notify.success('Diagnóstico guardado correctamente');
    this.closeForm();
  }
  confirmDelete() {
    const x = this.pendingDelete();
    if (x) {
      this.service.delete(x.id);
      this.notify.success('Diagnóstico eliminado');
      this.pendingDelete.set(null);
    }
  }
}
