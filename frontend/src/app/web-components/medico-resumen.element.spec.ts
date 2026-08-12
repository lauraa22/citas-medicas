import { TestBed } from '@angular/core/testing';

import {
  MedicoResumenElementComponent,
} from './medico-resumen.element';

describe('MedicoResumenElementComponent', () => {
  it('crea el componente', async () => {
    await TestBed.configureTestingModule({
      imports: [
        MedicoResumenElementComponent,
      ],
    }).compileComponents();

    const fixture =
      TestBed.createComponent(
        MedicoResumenElementComponent,
      );

    expect(
      fixture.componentInstance,
    ).toBeTruthy();
  });

  it('muestra nombre y número de colegiado', async () => {
    await TestBed.configureTestingModule({
      imports: [
        MedicoResumenElementComponent,
      ],
    }).compileComponents();

    const fixture =
      TestBed.createComponent(
        MedicoResumenElementComponent,
      );

    fixture.componentInstance.nombre =
      'Ana Martínez';

    fixture.componentInstance.colegiado =
      'MED001';

    fixture.detectChanges();

    const text =
      fixture.nativeElement.textContent;

    expect(text).toContain(
      'Ana Martínez',
    );

    expect(text).toContain(
      'MED001',
    );
  });
});