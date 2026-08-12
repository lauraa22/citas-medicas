import { TestBed } from '@angular/core/testing';

import {
  PacienteResumenElementComponent,
} from './paciente-resumen.element';

describe('PacienteResumenElementComponent', () => {
  it('crea el componente', async () => {
    await TestBed.configureTestingModule({
      imports: [
        PacienteResumenElementComponent,
      ],
    }).compileComponents();

    const fixture =
      TestBed.createComponent(
        PacienteResumenElementComponent,
      );

    expect(
      fixture.componentInstance,
    ).toBeTruthy();
  });

  it('muestra nombre y NSS', async () => {
    await TestBed.configureTestingModule({
      imports: [
        PacienteResumenElementComponent,
      ],
    }).compileComponents();

    const fixture =
      TestBed.createComponent(
        PacienteResumenElementComponent,
      );

    fixture.componentInstance.nombre =
      'Laura Pérez';

    fixture.componentInstance.nss =
      'NSS001';

    fixture.detectChanges();

    const text =
      fixture.nativeElement.textContent;

    expect(text).toContain(
      'Laura Pérez',
    );

    expect(text).toContain(
      'NSS001',
    );
  });
});