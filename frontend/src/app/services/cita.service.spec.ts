import { TestBed } from '@angular/core/testing';
import { CitaService } from './cita.service';
describe('CitaService', () => {
  let s: CitaService;
  beforeEach(() => {
    localStorage.clear();
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({});
    s = TestBed.inject(CitaService);
  });
  it('filtra citas por médico', () => {
    expect(s.byDoctor(1).every((x) => x.medicoId === 1)).toBe(true);
    expect(s.byDoctor(null).length).toBe(s.citas().length);
  });
  it('hace CRUD de una cita', () => {
    const x = s.create({
      fechaHora: '2026-08-20T10:00',
      motivoCita: 'Test',
      pacienteId: 1,
      medicoId: 2,
      diagnosticoId: null,
    } as any);
    expect(s.getById(x.id)).toBeTruthy();
    s.update({ ...x, motivoCita: 'Cambio' });
    expect(s.getById(x.id)?.motivoCita).toBe('Cambio');
    s.delete(x.id);
    expect(s.getById(x.id)).toBeUndefined();
  });
});
