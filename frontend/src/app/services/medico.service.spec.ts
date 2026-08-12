import { TestBed } from '@angular/core/testing';
import { MedicoService } from './medico.service';
import { PacienteService } from './paciente.service';
describe('MedicoService', () => {
  let m: MedicoService, p: PacienteService;
  beforeEach(() => {
    localStorage.clear();
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({});
    m = TestBed.inject(MedicoService);
    p = TestBed.inject(PacienteService);
  });
  it('crea y relaciona un médico con pacientes', () => {
    const x = m.create({
      nombre: 'Doc',
      apellidos: 'Test',
      usuario: 'doc',
      clave: '1',
      numColegiado: 'COLX',
      pacienteIds: [1],
    } as any);
    expect(p.getById(1)?.medicoIds).toContain(x.id);
  });
  it('actualiza un médico', () => {
    const x = m.getById(1)!;
    m.update({ ...x, numColegiado: 'NUEVO' });
    expect(m.getById(1)?.numColegiado).toBe('NUEVO');
  });
});
