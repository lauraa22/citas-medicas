import { Injectable } from '@angular/core';
import { BaseStore } from '../core/base/base.store';
import { Paciente } from '../models/paciente.model';
import { PACIENTES_MOCK } from '../mocks/data.mock';

@Injectable({ providedIn: 'root' })
export class PacienteStore extends BaseStore<Paciente> {
  constructor() {
    super('citas-medicas-pacientes', PACIENTES_MOCK);
  }
}
