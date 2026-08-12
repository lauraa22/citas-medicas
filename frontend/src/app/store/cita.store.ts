import { Injectable } from '@angular/core';
import { BaseStore } from '../core/base/base.store';
import { Cita } from '../models/cita.model';
import { CITAS_MOCK } from '../mocks/data.mock';

@Injectable({ providedIn: 'root' })
export class CitaStore extends BaseStore<Cita> {
  constructor() {
    super('citas-medicas-citas', CITAS_MOCK);
  }
}
