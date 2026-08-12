import { inject, Injectable } from '@angular/core';
import { Paciente } from '../models/paciente.model';
import { PacienteStore } from '../store/paciente.store';
import { MedicoStore } from '../store/medico.store';

@Injectable({
  providedIn: 'root',
})
export class PacienteService {
  private readonly store = inject(PacienteStore);
  private readonly medicoStore = inject(MedicoStore);

  readonly pacientes = this.store.items;
  readonly total = this.store.total;

  getById(id: number) {
    return this.store.getById(id);
  }

  create(data: Omit<Paciente, 'id'>): Paciente {
    const paciente = new Paciente(
      this.store.nextId(),
      data.nombre,
      data.apellidos,
      data.usuario,
      data.clave,
      data.nss,
      data.numTarjeta,
      data.telefono,
      data.direccion,
      data.medicoIds ?? [],
    );

    this.store.create(paciente);
    this.syncRelations(paciente);

    return paciente;
  }

  update(paciente: Paciente): void {
    this.store.update(paciente);
    this.syncRelations(paciente);
  }

  delete(id: number): void {
    this.store.delete(id);

    for (const medico of this.medicoStore.items()) {
      if (medico.pacienteIds.includes(id)) {
        this.medicoStore.update({
          ...medico,
          pacienteIds: medico.pacienteIds.filter((pacienteId) => pacienteId !== id),
        });
      }
    }
  }

  search(text: string): Paciente[] {
    const termino = text.trim().toLowerCase();

    if (!termino) {
      return this.pacientes();
    }

    return this.pacientes().filter((paciente) =>
      `${paciente.nombre} ${paciente.apellidos} ${paciente.nss}`.toLowerCase().includes(termino),
    );
  }

  private syncRelations(paciente: Paciente): void {
    for (const medico of this.medicoStore.items()) {
      const debeEstarRelacionado = paciente.medicoIds.includes(medico.id);

      const yaEstaRelacionado = medico.pacienteIds.includes(paciente.id);

      if (debeEstarRelacionado && !yaEstaRelacionado) {
        this.medicoStore.update({
          ...medico,
          pacienteIds: [...medico.pacienteIds, paciente.id],
        });
      }

      if (!debeEstarRelacionado && yaEstaRelacionado) {
        this.medicoStore.update({
          ...medico,
          pacienteIds: medico.pacienteIds.filter((id) => id !== paciente.id),
        });
      }
    }
  }
}
