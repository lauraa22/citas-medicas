import { HttpClient } from '@angular/common/http';

import {
  computed,
  inject,
  Injectable,
  signal,
} from '@angular/core';

import {
  map,
  Observable,
  tap,
} from 'rxjs';

import { Usuario } from '../models/usuario.model';

export interface UsuarioWrite {
  nombre: string;
  apellidos: string;
  usuario: string;
  clave: string;
}

@Injectable({
  providedIn: 'root',
})
export class UsuarioService {
  private readonly http =
    inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5134/api/usuarios';

  private readonly _usuarios =
    signal<Usuario[]>([]);

  readonly usuarios =
    this._usuarios.asReadonly();

  readonly total = computed(
    () => this._usuarios().length,
  );

  load(): void {
    this.http
      .get<Usuario[]>(this.apiUrl)
      .pipe(
        map((usuarios) =>
          usuarios.map(
            (usuario) =>
              this.normalize(usuario),
          ),
        ),
      )
      .subscribe({
        next: (usuarios) => {
          this._usuarios.set(usuarios);
        },

        error: (error) => {
          console.error(
            'Error cargando usuarios',
            error,
          );
        },
      });
  }

  getById(
    id: number,
  ): Observable<Usuario> {
    return this.http
      .get<Usuario>(
        `${this.apiUrl}/${id}`,
      )
      .pipe(
        map((usuario) =>
          this.normalize(usuario),
        ),
      );
  }

  findById(
    id: number,
  ): Usuario | undefined {
    return this._usuarios().find(
      (usuario) =>
        usuario.id === id,
    );
  }

  create(
    data: UsuarioWrite,
  ): Observable<Usuario> {
    return this.http
      .post<Usuario>(
        this.apiUrl,
        data,
      )
      .pipe(
        map((usuario) =>
          this.normalize(
            usuario,
            data.clave,
          ),
        ),

        tap((created) => {
          this._usuarios.update(
            (items) => [
              ...items,
              created,
            ],
          );
        }),
      );
  }

  update(
    id: number,
    data: UsuarioWrite,
  ): Observable<void> {
    return this.http
      .put<void>(
        `${this.apiUrl}/${id}`,
        data,
      )
      .pipe(
        tap(() => {
          this.load();
        }),
      );
  }

  delete(
    id: number,
  ): Observable<void> {
    return this.http
      .delete<void>(
        `${this.apiUrl}/${id}`,
      )
      .pipe(
        tap(() => {
          this._usuarios.update(
            (items) =>
              items.filter(
                (usuario) =>
                  usuario.id !== id,
              ),
          );
        }),
      );
  }

  usernameExists(
    username: string,
    ignoreId?: number,
  ): boolean {
    const normalized =
      username.trim().toLowerCase();

    return this._usuarios().some(
      (usuario) =>
        usuario.usuario
          .toLowerCase() ===
          normalized &&
        usuario.id !== ignoreId,
    );
  }

  private normalize(
    usuario: Usuario,
    clave = '',
  ): Usuario {
    return new Usuario(
      usuario.id,
      usuario.nombre,
      usuario.apellidos,
      usuario.usuario,
      usuario.clave ?? clave,
    );
  }
}