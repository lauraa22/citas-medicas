import { Injectable } from '@angular/core';
import { BaseStore } from '../core/base/base.store';
import { Diagnostico } from '../models/diagnostico.model';
import { DIAGNOSTICOS_MOCK } from '../mocks/data.mock';

@Injectable({ providedIn: 'root' })
export class DiagnosticoStore extends BaseStore<Diagnostico> {
  constructor() {
    super('citas-medicas-diagnosticos', DIAGNOSTICOS_MOCK);
  }
}
