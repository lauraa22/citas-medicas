import { Injectable } from '@angular/core';
import { BaseStore } from '../core/base/base.store';
import { Medico } from '../models/medico.model';
import { MEDICOS_MOCK } from '../mocks/data.mock';

@Injectable({ providedIn: 'root' })
export class MedicoStore extends BaseStore<Medico> {
  constructor() {
    super('citas-medicas-medicos', MEDICOS_MOCK);
  }
}
