import { inject, Injectable } from '@angular/core';
import { Diagnostico } from '../models/diagnostico.model';
import { DiagnosticoStore } from '../store/diagnostico.store';

@Injectable({
  providedIn: 'root',
})
export class DiagnosticoService {
  private readonly store = inject(DiagnosticoStore);

  readonly diagnosticos = this.store.items;
  readonly total = this.store.total;

  getById(id: number) {
    return this.store.getById(id);
  }

  create(data: Omit<Diagnostico, 'id'>): Diagnostico {
    const diagnostico = new Diagnostico(
      this.store.nextId(),
      data.valoracionEspecialista,
      data.enfermedad,
    );

    this.store.create(diagnostico);

    return diagnostico;
  }

  update(diagnostico: Diagnostico): void {
    this.store.update(diagnostico);
  }

  delete(id: number): void {
    this.store.delete(id);
  }
}
