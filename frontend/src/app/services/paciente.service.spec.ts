import { TestBed } from '@angular/core/testing';
import { PacienteService } from './paciente.service';
import { MedicoService } from './medico.service';
describe('PacienteService', () => {
  let p: PacienteService, m: MedicoService;
  beforeEach(() => {
    localStorage.clear();
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({});
    p = TestBed.inject(PacienteService);
    m = TestBed.inject(MedicoService);
  });
  it('busca por nombre o NSS', () => {
    expect(p.search('Ana').length).toBe(1);
    expect(p.search('NSS002')[0].nombre).toBe('Pedro');
  });
  it('sincroniza relación paciente-médico al crear', () => {
    const x = p.create({
      nombre: 'X',
      apellidos: 'Y',
      usuario: 'xy',
      clave: '1',
      nss: 'NSSX',
      numTarjeta: 'TX',
      telefono: '600999999',
      direccion: 'C',
      medicoIds: [1],
    } as any);
    expect(m.getById(1)?.pacienteIds).toContain(x.id);
  });
  it('elimina también la relación inversa', () => {
    p.delete(1);
    expect(m.getById(1)?.pacienteIds).not.toContain(1);
  });
});
