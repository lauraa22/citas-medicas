import { HttpClient } from '@angular/common/http';

import {
  computed,
  inject,
  Injectable,
  signal,
} from '@angular/core';

import {
  Observable,
  tap,
} from 'rxjs';

import { Diagnostico } from '../models/diagnostico.model';

export interface DiagnosticoWrite {
  valoracionEspecialista: string;
  enfermedad: string;
}

@Injectable({
  providedIn: 'root',
})
export class DiagnosticoService {
  private readonly http =
    inject(HttpClient);

  private readonly apiUrl =
    'http://localhost:5134/api/diagnosticos';

  private readonly _diagnosticos =
    signal<Diagnostico[]>([]);

  readonly diagnosticos =
    this._diagnosticos.asReadonly();

  readonly total = computed(
    () =>
      this._diagnosticos().length,
  );

  load(): void {
    this.http
      .get<Diagnostico[]>(
        this.apiUrl,
      )
      .subscribe({
        next: (diagnosticos) => {
          this._diagnosticos.set(
            diagnosticos,
          );
        },

        error: (error) => {
          console.error(
            'Error cargando diagnósticos',
            error,
          );
        },
      });
  }

  getById(
    id: number,
  ): Observable<Diagnostico> {
    return this.http.get<Diagnostico>(
      `${this.apiUrl}/${id}`,
    );
  }

  findById(
    id: number,
  ): Diagnostico | undefined {
    return this._diagnosticos().find(
      (diagnostico) =>
        diagnostico.id === id,
    );
  }

  create(
    data: DiagnosticoWrite,
  ): Observable<Diagnostico> {
    return this.http
      .post<Diagnostico>(
        this.apiUrl,
        data,
      )
      .pipe(
        tap((created) => {
          this._diagnosticos.update(
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
    data: DiagnosticoWrite,
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
          this._diagnosticos.update(
            (items) =>
              items.filter(
                (diagnostico) =>
                  diagnostico.id !==
                  id,
              ),
          );
        }),
      );
  }
}