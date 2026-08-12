import { TestBed } from '@angular/core/testing';
import { Injector } from '@angular/core';
import { registerMedicalElements } from './register-elements';
describe('Angular Elements', () => {
  it('registra los tres custom elements', () => {
    registerMedicalElements(TestBed.inject(Injector));
    expect(customElements.get('paciente-resumen')).toBeTruthy();
    expect(customElements.get('medico-resumen')).toBeTruthy();
    expect(customElements.get('cita-resumen')).toBeTruthy();
  });
});
