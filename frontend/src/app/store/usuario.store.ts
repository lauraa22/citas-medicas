import { Injectable } from '@angular/core';
import { BaseStore } from '../core/base/base.store';
import { Usuario } from '../models/usuario.model';
import { USUARIOS_MOCK } from '../mocks/data.mock';

@Injectable({ providedIn: 'root' })
export class UsuarioStore extends BaseStore<Usuario> {
  constructor() {
    super('citas-medicas-usuarios', USUARIOS_MOCK);
  }
}
