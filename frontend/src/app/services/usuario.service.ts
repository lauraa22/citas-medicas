import { inject, Injectable } from '@angular/core';
import { Usuario } from '../models/usuario.model';
import { UsuarioStore } from '../store/usuario.store';

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private readonly store = inject(UsuarioStore);

  readonly usuarios = this.store.items;
  readonly total = this.store.total;

  getById(id: number) {
    return this.store.getById(id);
  }

  create(data: Omit<Usuario, 'id'>): Usuario {
    const usuario = new Usuario(
      this.store.nextId(),
      data.nombre,
      data.apellidos,
      data.usuario,
      data.clave,
    );

    this.store.create(usuario);

    return usuario;
  }

  update(usuario: Usuario): void {
    this.store.update(usuario);
  }

  delete(id: number): void {
    this.store.delete(id);
  }

  usernameExists(username: string, ignoreId?: number): boolean {
    return this.usuarios().some(
      (usuario) =>
        usuario.usuario.toLowerCase() === username.toLowerCase() && usuario.id !== ignoreId,
    );
  }
}
