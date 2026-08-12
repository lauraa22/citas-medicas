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

import { Paciente } from '../models/paciente.model';

export interface PacienteWrite {
  nombre: string;
  apellidos: string;
  usuario: string;
  clave: string;
  nss: string;
  numTarjeta: string;
  telefono: string;
  direccion: string;
  medicoIds: number[];
}

@Injectable({
  providedIn: 'root',
})
export class PacienteService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5134/api/pacientes';

  private readonly _pacientes =
    signal<Paciente[]>([]);

  readonly pacientes =
    this._pacientes.asReadonly();

  readonly total = computed(
    () => this._pacientes().length,
  );

  load(): void {
    this.http
      .get<Paciente[]>(this.apiUrl)
      .pipe(
        map((pacientes) =>
          pacientes.map((paciente) =>
            this.normalize(paciente),
          ),
        ),
      )
      .subscribe({
        next: (pacientes) => {
          this._pacientes.set(pacientes);
        },
        error: (error) => {
          console.error(
            'Error cargando pacientes',
            error,
          );
        },
      });
  }

  getById(
    id: number,
  ): Observable<Paciente> {
    return this.http
      .get<Paciente>(
        `${this.apiUrl}/${id}`,
      )
      .pipe(
        map((paciente) =>
          this.normalize(paciente),
        ),
      );
  }

  findById(
    id: number,
  ): Paciente | undefined {
    return this._pacientes().find(
      (paciente) =>
        paciente.id === id,
    );
  }

  create(
    data: PacienteWrite,
  ): Observable<Paciente> {
    return this.http
      .post<Paciente>(
        this.apiUrl,
        data,
      )
      .pipe(
        map((paciente) =>
          this.normalize(
            paciente,
            data.clave,
          ),
        ),
        tap((created) => {
          this._pacientes.update(
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
    data: PacienteWrite,
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
          this._pacientes.update(
            (items) =>
              items.filter(
                (paciente) =>
                  paciente.id !== id,
              ),
          );
        }),
      );
  }

  search(
    text: string,
  ): Paciente[] {
    const value =
      text.trim().toLowerCase();

    if (!value) {
      return this._pacientes();
    }

    return this._pacientes().filter(
      (paciente) =>
        paciente.nombre
          .toLowerCase()
          .includes(value) ||
        paciente.apellidos
          .toLowerCase()
          .includes(value) ||
        paciente.nss
          .toLowerCase()
          .includes(value),
    );
  }

  private normalize(
    paciente: Paciente,
    clave = '',
  ): Paciente {
    return new Paciente(
      paciente.id,
      paciente.nombre,
      paciente.apellidos,
      paciente.usuario,
      paciente.clave ?? clave,
      paciente.nss,
      paciente.numTarjeta,
      paciente.telefono,
      paciente.direccion,
      paciente.medicoIds ?? [],
    );
  }
}