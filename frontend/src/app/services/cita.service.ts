import { HttpClient } from '@angular/common/http';
import {
  computed,
  inject,
  Injectable,
  signal,
} from '@angular/core';
import { Observable, tap } from 'rxjs';

import { Cita } from '../models/cita.model';

export interface CitaWrite {
  fechaHora: string;
  motivoCita: string;
  pacienteId: number;
  medicoId: number;
  diagnosticoId: number | null;
}

@Injectable({
  providedIn: 'root',
})
export class CitaService {
  private readonly http = inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5134/api/citas';

  private readonly _citas =
    signal<Cita[]>([]);

  readonly citas =
    this._citas.asReadonly();

  readonly total = computed(
    () => this._citas().length,
  );

  load(): void {
    this.http
      .get<Cita[]>(this.apiUrl)
      .subscribe({
        next: (citas) => {
          this._citas.set(citas);
        },

        error: (error) => {
          console.error(
            'Error cargando citas',
            error,
          );
        },
      });
  }

  getById(
    id: number,
  ): Observable<Cita> {
    return this.http.get<Cita>(
      `${this.apiUrl}/${id}`,
    );
  }

  findById(
    id: number,
  ): Cita | undefined {
    return this._citas().find(
      (cita) => cita.id === id,
    );
  }

  create(
    data: CitaWrite,
  ): Observable<Cita> {
    return this.http
      .post<Cita>(
        this.apiUrl,
        data,
      )
      .pipe(
        tap((created) => {
          this._citas.update(
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
    data: CitaWrite,
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
          this._citas.update(
            (items) =>
              items.filter(
                (cita) =>
                  cita.id !== id,
              ),
          );
        }),
      );
  }

  /**
   * Único filtro de citas de la aplicación.
   * Devuelve todas las citas o únicamente
   * las correspondientes al médico indicado.
   */
  byDoctor(
    medicoId: number | null,
  ): Cita[] {
    if (
      medicoId === null ||
      medicoId === 0
    ) {
      return this._citas();
    }

    return this._citas().filter(
      (cita) =>
        cita.medicoId === medicoId,
    );
  }
}