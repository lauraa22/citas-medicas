import { TestBed } from '@angular/core/testing';
import { DiagnosticoService } from './diagnostico.service';
describe('DiagnosticoService', () => {
  let s: DiagnosticoService;
  beforeEach(() => {
    localStorage.clear();
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({});
    s = TestBed.inject(DiagnosticoService);
  });
  it('hace CRUD', () => {
    const x = s.create({ enfermedad: 'Test', valoracionEspecialista: 'Valor' } as any);
    expect(s.getById(x.id)?.enfermedad).toBe('Test');
    s.update({ ...x, enfermedad: 'Cambio' });
    expect(s.getById(x.id)?.enfermedad).toBe('Cambio');
    s.delete(x.id);
    expect(s.getById(x.id)).toBeUndefined();
  });
});
