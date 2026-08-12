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
  UsuarioService,
  UsuarioWrite,
} from '../services/usuario.service';

import { NotificationService } from '../services/notification.service';
import { Usuario } from '../models/usuario.model';
import { DeleteConfirmComponent } from '../shared/delete-confirm.component';

@Component({
  selector: 'app-usuarios',

  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule,
    DeleteConfirmComponent,
  ],

  template: `
    <div class="page-head">
      <div>
        <h1>Usuarios</h1>
      </div>

      <button
        data-cy="new-user"
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
          usuario
        </h2>

        <form
          [formGroup]="form"
          (ngSubmit)="save()"
        >
          <div class="grid">
            <label>
              Nombre

              <input
                data-cy="user-name"
                formControlName="nombre"
              />
            </label>

            <label>
              Apellidos

              <input
                data-cy="user-lastname"
                formControlName="apellidos"
              />
            </label>

            <label>
              Usuario

              <input
                data-cy="user-username"
                formControlName="usuario"
              />
            </label>

            <label>
              Clave

              <input
                data-cy="user-password"
                type="password"
                formControlName="clave"
              />
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
              data-cy="save-user"
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
          <th>Nombre</th>
          <th>Usuario</th>
          <th>Acciones</th>
        </tr>
      </thead>

      <tbody>
        @for (
          usuario of service.usuarios();
          track usuario.id
        ) {
          <tr data-cy="user-row">
            <td>
              {{ usuario.id }}
            </td>

            <td>
              {{ usuario.nombre }}
              {{ usuario.apellidos }}
            </td>

            <td>
              {{ usuario.usuario }}
            </td>

            <td>
              <button
                class="link"
                (click)="detail.set(usuario)"
              >
                Ver
              </button>

              <button
                class="link"
                (click)="edit(usuario)"
              >
                Editar
              </button>

              <button
                class="link danger-text"
                (click)="askDelete(usuario)"
              >
                Eliminar
              </button>
            </td>
          </tr>
        }
      </tbody>
    </table>

    @if (detail(); as usuario) {
      <section class="panel detail">
        <h2>
          Detalle usuario
          #{{ usuario.id }}
        </h2>

        <p>
          <b>Nombre:</b>
          {{ usuario.nombre }}
          {{ usuario.apellidos }}
        </p>

        <p>
          <b>Usuario:</b>
          {{ usuario.usuario }}
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
export class UsuariosComponent
  implements OnInit
{
  service =
    inject(UsuarioService);

  notify =
    inject(NotificationService);

  fb =
    inject(FormBuilder);

  formVisible =
    signal(false);

  editingId =
    signal<number | null>(null);

  detail =
    signal<Usuario | null>(null);

  pendingDelete =
    signal<Usuario | null>(null);

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
    usuario: Usuario,
  ): void {
    this.editingId.set(
      usuario.id,
    );

    this.form.setValue({
      nombre:
        usuario.nombre,

      apellidos:
        usuario.apellidos,

      usuario:
        usuario.usuario,

      // La API no devuelve la clave.
      clave: '',
    });

    this.formVisible.set(true);
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

    if (
      this.service.usernameExists(
        values.usuario,
        this.editingId() ??
          undefined,
      )
    ) {
      this.notify.error(
        'El usuario ya existe',
      );

      return;
    }

    const data: UsuarioWrite = {
      nombre:
        values.nombre,

      apellidos:
        values.apellidos,

      usuario:
        values.usuario,

      clave:
        values.clave,
    };

    const id =
      this.editingId();

    if (id !== null) {
      this.service
        .update(id, data)
        .subscribe({
          next: () => {
            this.notify.success(
              'Usuario actualizado correctamente',
            );

            this.closeForm();
          },

          error: (error) => {
            console.error(error);

            this.notify.error(
              error.error?.message ??
                'Error al actualizar el usuario',
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
            'Usuario creado correctamente',
          );

          this.closeForm();
        },

        error: (error) => {
          console.error(error);

          this.notify.error(
            error.error?.message ??
              'Error al crear el usuario',
          );
        },
      });
  }

  askDelete(
    usuario: Usuario,
  ): void {
    this.pendingDelete.set(
      usuario,
    );
  }

  confirmDelete(): void {
    const usuario =
      this.pendingDelete();

    if (!usuario) {
      return;
    }

    this.service
      .delete(usuario.id)
      .subscribe({
        next: () => {
          this.notify.success(
            'Usuario eliminado',
          );

          this.pendingDelete.set(
            null,
          );

          if (
            this.detail()?.id ===
            usuario.id
          ) {
            this.detail.set(null);
          }
        },

        error: (error) => {
          console.error(error);

          this.notify.error(
            error.error?.message ??
              'No se ha podido eliminar el usuario',
          );
        },
      });
  }
}