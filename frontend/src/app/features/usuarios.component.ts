import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UsuarioService } from '../services/usuario.service';
import { NotificationService } from '../services/notification.service';
import { Usuario } from '../models/usuario.model';
import { DeleteConfirmComponent } from '../shared/delete-confirm.component';
@Component({
  selector: 'app-usuarios',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, DeleteConfirmComponent],
  template: `
    <div class="page-head">
      <div>
        <h1>Usuarios</h1>
      </div>
      <button data-cy="new-user" (click)="newItem()">+ Nuevo</button>
    </div>
    @if (formVisible()) {
      <section class="panel">
        <h2>{{ editingId() ? 'Editar' : 'Nuevo' }} usuario</h2>
        <form [formGroup]="form" (ngSubmit)="save()">
          <div class="grid">
            <label>Nombre<input data-cy="user-name" formControlName="nombre" /></label
            ><label>Apellidos<input formControlName="apellidos" /></label
            ><label>Usuario<input data-cy="user-username" formControlName="usuario" /></label
            ><label>Clave<input type="password" formControlName="clave" /></label>
          </div>
          <div class="actions">
            <button type="button" class="secondary" (click)="closeForm()">Cancelar</button
            ><button data-cy="save-user" [disabled]="form.invalid">Guardar</button>
          </div>
        </form>
      </section>
    }
    <table>
      <thead>
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th>Usuario</th>
          <th>Acciones</th>
        </tr>
      </thead>
      <tbody>
        @for (x of service.usuarios(); track x.id) {
          <tr>
            <td>{{ x.id }}</td>
            <td>{{ x.nombre }} {{ x.apellidos }}</td>
            <td>{{ x.usuario }}</td>
            <td>
              <button class="link" (click)="detail.set(x)">Ver</button
              ><button class="link" (click)="edit(x)">Editar</button
              ><button class="link danger-text" (click)="askDelete(x)">Eliminar</button>
            </td>
          </tr>
        }
      </tbody>
    </table>
    @if (detail(); as x) {
      <section class="panel detail">
        <h2>Detalle usuario #{{ x.id }}</h2>
        <p><b>Nombre:</b> {{ x.nombre }} {{ x.apellidos }}</p>
        <p><b>Usuario:</b> {{ x.usuario }}</p>
        <button class="secondary" (click)="detail.set(null)">Cerrar</button>
      </section>
    }
    <app-delete-confirm
      [open]="!!pendingDelete()"
      [label]="pendingDelete()?.nombre || ''"
      (cancel)="pendingDelete.set(null)"
      (confirm)="confirmDelete()"
    />
  `,
})
export class UsuariosComponent {
  service = inject(UsuarioService);
  notify = inject(NotificationService);
  fb = inject(FormBuilder);
  formVisible = signal(false);
  editingId = signal<number | null>(null);
  detail = signal<Usuario | null>(null);
  pendingDelete = signal<Usuario | null>(null);
  form = this.fb.nonNullable.group({
    nombre: ['', Validators.required],
    apellidos: ['', Validators.required],
    usuario: ['', Validators.required],
    clave: ['', Validators.required],
  });
  newItem() {
    this.editingId.set(null);
    this.form.reset();
    this.formVisible.set(true);
  }
  edit(x: Usuario) {
    this.editingId.set(x.id);
    this.form.setValue({
      nombre: x.nombre,
      apellidos: x.apellidos,
      usuario: x.usuario,
      clave: x.clave,
    });
    this.formVisible.set(true);
  }
  closeForm() {
    this.formVisible.set(false);
  }
  save() {
    if (this.form.invalid) return;
    const v = this.form.getRawValue();
    if (this.service.usernameExists(v.usuario, this.editingId() ?? undefined)) {
      this.notify.error('El usuario ya existe');
      return;
    }
    const id = this.editingId();
    if (id) this.service.update(new Usuario(id, v.nombre, v.apellidos, v.usuario, v.clave));
    else this.service.create(v as any);
    this.notify.success('Usuario guardado correctamente');
    this.closeForm();
  }
  askDelete(x: Usuario) {
    this.pendingDelete.set(x);
  }
  confirmDelete() {
    const x = this.pendingDelete();
    if (x) {
      this.service.delete(x.id);
      this.notify.success('Usuario eliminado');
      this.pendingDelete.set(null);
    }
  }
}
