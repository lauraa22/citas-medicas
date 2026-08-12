import { inject, Injectable } from '@angular/core';
import { Cita } from '../models/cita.model';
import { CitaStore } from '../store/cita.store';

@Injectable({
  providedIn: 'root',
})
export class CitaService {
  private readonly store = inject(CitaStore);

  readonly citas = this.store.items;
  readonly total = this.store.total;

  getById(id: number) {
    return this.store.getById(id);
  }

  create(data: Omit<Cita, 'id'>): Cita {
    const cita = new Cita(
      this.store.nextId(),
      data.fechaHora,
      data.motivoCita,
      Number(data.pacienteId),
      Number(data.medicoId),
      data.diagnosticoId ? Number(data.diagnosticoId) : null,
    );

    this.store.create(cita);

    return cita;
  }

  update(cita: Cita): void {
    this.store.update({
      ...cita,
      pacienteId: Number(cita.pacienteId),
      medicoId: Number(cita.medicoId),
      diagnosticoId: cita.diagnosticoId ? Number(cita.diagnosticoId) : null,
    });
  }

  delete(id: number): void {
    this.store.delete(id);
  }

  byDoctor(medicoId: number | null): Cita[] {
    if (!medicoId) {
      return this.citas();
    }

    return this.citas().filter((cita) => cita.medicoId === medicoId);
  }
}
