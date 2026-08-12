import { inject, Injectable } from '@angular/core';
import { Medico } from '../models/medico.model';
import { MedicoStore } from '../store/medico.store';
import { PacienteStore } from '../store/paciente.store';

@Injectable({
  providedIn: 'root',
})
export class MedicoService {
  private readonly store = inject(MedicoStore);
  private readonly pacienteStore = inject(PacienteStore);

  readonly medicos = this.store.items;
  readonly total = this.store.total;

  getById(id: number) {
    return this.store.getById(id);
  }

  create(data: Omit<Medico, 'id'>): Medico {
    const medico = new Medico(
      this.store.nextId(),
      data.nombre,
      data.apellidos,
      data.usuario,
      data.clave,
      data.numColegiado,
      data.pacienteIds ?? [],
    );

    this.store.create(medico);
    this.syncRelations(medico);

    return medico;
  }

  update(medico: Medico): void {
    this.store.update(medico);
    this.syncRelations(medico);
  }

  delete(id: number): void {
    this.store.delete(id);

    for (const paciente of this.pacienteStore.items()) {
      if (paciente.medicoIds.includes(id)) {
        this.pacienteStore.update({
          ...paciente,
          medicoIds: paciente.medicoIds.filter((medicoId) => medicoId !== id),
        });
      }
    }
  }

  private syncRelations(medico: Medico): void {
    for (const paciente of this.pacienteStore.items()) {
      const debeEstarRelacionado = medico.pacienteIds.includes(paciente.id);

      const yaEstaRelacionado = paciente.medicoIds.includes(medico.id);

      if (debeEstarRelacionado && !yaEstaRelacionado) {
        this.pacienteStore.update({
          ...paciente,
          medicoIds: [...paciente.medicoIds, medico.id],
        });
      }

      if (!debeEstarRelacionado && yaEstaRelacionado) {
        this.pacienteStore.update({
          ...paciente,
          medicoIds: paciente.medicoIds.filter((id) => id !== medico.id),
        });
      }
    }
  }
}
