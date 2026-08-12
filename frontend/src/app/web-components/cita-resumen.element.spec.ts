import { TestBed } from '@angular/core/testing';

import {
  CitaResumenElementComponent,
} from './cita-resumen.element';

describe('CitaResumenElementComponent', () => {
  it('crea el componente', async () => {
    await TestBed.configureTestingModule({
      imports: [
        CitaResumenElementComponent,
      ],
    }).compileComponents();

    const fixture =
      TestBed.createComponent(
        CitaResumenElementComponent,
      );

    expect(
      fixture.componentInstance,
    ).toBeTruthy();
  });

  it('muestra fecha y texto de la cita', async () => {
    await TestBed.configureTestingModule({
      imports: [
        CitaResumenElementComponent,
      ],
    }).compileComponents();

    const fixture =
      TestBed.createComponent(
        CitaResumenElementComponent,
      );

    fixture.componentInstance.fecha =
      '13/08/2026 10:30';

    fixture.componentInstance.texto =
      'Consulta general';

    fixture.detectChanges();

    const text =
      fixture.nativeElement.textContent;

    expect(text).toContain(
      '13/08/2026 10:30',
    );

    expect(text).toContain(
      'Consulta general',
    );
  });

  it('muestra los valores por defecto', async () => {
    await TestBed.configureTestingModule({
      imports: [
        CitaResumenElementComponent,
      ],
    }).compileComponents();

    const fixture =
      TestBed.createComponent(
        CitaResumenElementComponent,
      );

    fixture.detectChanges();

    const text =
      fixture.nativeElement.textContent;

    expect(text).toContain(
      'Sin fecha',
    );

    expect(text).toContain(
      'Cita médica',
    );
  });
});