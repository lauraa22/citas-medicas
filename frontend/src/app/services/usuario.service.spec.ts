import { TestBed } from '@angular/core/testing';
import { UsuarioService } from './usuario.service';
describe('UsuarioService', () => {
  let s: UsuarioService;
  beforeEach(() => {
    localStorage.clear();
    TestBed.resetTestingModule();
    TestBed.configureTestingModule({});
    s = TestBed.inject(UsuarioService);
  });
  it('crea, actualiza y elimina usuarios', () => {
    const n = s.create({ nombre: 'Test', apellidos: 'Uno', usuario: 'test', clave: 'x' } as any);
    expect(n.id).toBeGreaterThan(0);
    expect(s.getById(n.id)?.usuario).toBe('test');
    s.update({ ...n, nombre: 'Cambio' });
    expect(s.getById(n.id)?.nombre).toBe('Cambio');
    s.delete(n.id);
    expect(s.getById(n.id)).toBeUndefined();
  });
  it('detecta usuario duplicado', () => expect(s.usernameExists('laura')).toBe(true));
});
