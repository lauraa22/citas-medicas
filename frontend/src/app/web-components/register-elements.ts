import { Injector } from '@angular/core';
import { createCustomElement } from '@angular/elements';
import { PacienteResumenElementComponent } from './paciente-resumen.element';
import { MedicoResumenElementComponent } from './medico-resumen.element';
import { CitaResumenElementComponent } from './cita-resumen.element';

export function registerMedicalElements(injector: Injector): void {
  const defs: Array<[string, any]> = [
    ['paciente-resumen', PacienteResumenElementComponent],
    ['medico-resumen', MedicoResumenElementComponent],
    ['cita-resumen', CitaResumenElementComponent],
  ];
  for (const [name, component] of defs) {
    if (!customElements.get(name))
      customElements.define(name, createCustomElement(component, { injector }));
  }
}
