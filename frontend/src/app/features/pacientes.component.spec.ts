import { TestBed } from '@angular/core/testing';
import { PacientesComponent } from './pacientes.component';
describe('PacientesComponent', () => {
  beforeEach(() => localStorage.clear());
  it('filtra con el buscador signal', async () => {
    await TestBed.configureTestingModule({ imports: [PacientesComponent] }).compileComponents();
    const f = TestBed.createComponent(PacientesComponent);
    f.detectChanges();
    f.componentInstance.search.set('Ana');
    f.detectChanges();
    expect(f.componentInstance.filtered().length).toBe(1);
    expect(f.nativeElement.textContent).toContain('Ana Martínez');
  });

  it('integra paciente-resumen en el detalle', async () => {

    await TestBed.configureTestingModule({
        imports: [PacientesComponent]
    }).compileComponents();

    const fixture =
        TestBed.createComponent(PacientesComponent);

    fixture.detectChanges();

    const paciente =
        fixture.componentInstance
        .service.pacientes()[0];

    fixture.componentInstance
        .detail.set(paciente);

    fixture.detectChanges();

    const webComponent =
        fixture.nativeElement.querySelector(
        'paciente-resumen'
        );

    expect(webComponent)
        .toBeTruthy();
    });
});
