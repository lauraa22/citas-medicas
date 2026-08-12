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

import { Medico } from '../models/medico.model';

export interface MedicoWrite {
  nombre: string;
  apellidos: string;
  usuario: string;
  clave: string;
  numColegiado: string;
  pacienteIds: number[];
}

@Injectable({
  providedIn: 'root',
})
export class MedicoService {
  private readonly http =
    inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5134/api/medicos';

  private readonly _medicos =
    signal<Medico[]>([]);

  readonly medicos =
    this._medicos.asReadonly();

  readonly total = computed(
    () => this._medicos().length,
  );

  load(): void {
    this.http
      .get<Medico[]>(
        this.apiUrl,
      )
      .pipe(
        map((medicos) =>
          medicos.map(
            (medico) =>
              this.normalize(medico),
          ),
        ),
      )
      .subscribe({
        next: (medicos) => {
          this._medicos.set(
            medicos,
          );
        },

        error: (error) => {
          console.error(
            'Error cargando médicos',
            error,
          );
        },
      });
  }

  getById(
    id: number,
  ): Observable<Medico> {
    return this.http
      .get<Medico>(
        `${this.apiUrl}/${id}`,
      )
      .pipe(
        map((medico) =>
          this.normalize(medico),
        ),
      );
  }

  findById(
    id: number,
  ): Medico | undefined {
    return this._medicos().find(
      (medico) =>
        medico.id === id,
    );
  }

  create(
    data: MedicoWrite,
  ): Observable<Medico> {
    return this.http
      .post<Medico>(
        this.apiUrl,
        data,
      )
      .pipe(
        map((medico) =>
          this.normalize(
            medico,
            data.clave,
          ),
        ),

        tap((created) => {
          this._medicos.update(
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
    data: MedicoWrite,
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
          this._medicos.update(
            (items) =>
              items.filter(
                (medico) =>
                  medico.id !== id,
              ),
          );
        }),
      );
  }

  private normalize(
    medico: Medico,
    clave = '',
  ): Medico {
    return new Medico(
      medico.id,
      medico.nombre,
      medico.apellidos,
      medico.usuario,
      medico.clave ?? clave,
      medico.numColegiado,
      medico.pacienteIds ?? [],
    );
  }
}