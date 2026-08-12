import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';

import { PacientesComponent } from './pacientes.component';
import { PacienteService } from '../services/paciente.service';
import { MedicoService } from '../services/medico.service';

describe('PacientesComponent', () => {
  let httpMock: HttpTestingController;

  const pacientesUrl =
    'http://localhost:5134/api/pacientes';

  const medicosUrl =
    'http://localhost:5134/api/medicos';

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        PacientesComponent,
      ],

      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    }).compileComponents();

    httpMock =
      TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('filtra con el buscador signal', () => {
    const fixture =
      TestBed.createComponent(
        PacientesComponent,
      );

    const component =
      fixture.componentInstance;

    /*
     * El primer detectChanges ejecuta ngOnInit(),
     * que llama a PacienteService.load()
     * y MedicoService.load().
     */
    fixture.detectChanges();

    const pacientesRequest =
      httpMock.expectOne(
        pacientesUrl,
      );

    expect(
      pacientesRequest.request.method,
    ).toBe('GET');

    pacientesRequest.flush([
      {
        id: 1,
        nombre: 'Ana',
        apellidos: 'Pérez',
        usuario: 'ana',
        nss: 'NSS001',
        numTarjeta: 'T001',
        telefono: '612345678',
        direccion: 'Granada',
        medicoIds: [],
      },
      {
        id: 2,
        nombre: 'Pedro',
        apellidos: 'López',
        usuario: 'pedro',
        nss: 'NSS002',
        numTarjeta: 'T002',
        telefono: '623456789',
        direccion: 'Madrid',
        medicoIds: [],
      },
    ]);

    const medicosRequest =
      httpMock.expectOne(
        medicosUrl,
      );

    expect(
      medicosRequest.request.method,
    ).toBe('GET');

    medicosRequest.flush([]);

    /*
     * Después de recibir los datos
     * buscamos por "Ana".
     */
    component.search.set('Ana');

    fixture.detectChanges();

    expect(
      component.filtered().length,
    ).toBe(1);

    expect(
      component.filtered()[0].nombre,
    ).toBe('Ana');

    const rows =
      fixture.nativeElement.querySelectorAll(
        '[data-cy="patient-row"]',
      );

    expect(rows.length).toBe(1);
  });

  it('integra paciente-resumen en el detalle', () => {
    const fixture =
      TestBed.createComponent(
        PacientesComponent,
      );

    const component =
      fixture.componentInstance;

    fixture.detectChanges();

    const pacientesRequest =
      httpMock.expectOne(
        pacientesUrl,
      );

    pacientesRequest.flush([
      {
        id: 1,
        nombre: 'Laura',
        apellidos: 'Sánchez',
        usuario: 'laura',
        nss: 'NSS001',
        numTarjeta: 'T001',
        telefono: '612345678',
        direccion: 'Granada',
        medicoIds: [],
      },
    ]);

    const medicosRequest =
      httpMock.expectOne(
        medicosUrl,
      );

    medicosRequest.flush([]);

    /*
     * Cogemos el paciente que ya está
     * cargado en el signal del servicio.
     */
    const paciente =
      component.service
        .pacientes()[0];

    expect(paciente).toBeTruthy();

    /*
     * Abrimos manualmente el detalle.
     */
    component.detail.set(
      paciente,
    );

    fixture.detectChanges();

    const webComponent =
      fixture.nativeElement.querySelector(
        'paciente-resumen',
      );

    expect(
      webComponent,
    ).toBeTruthy();

    expect(
      webComponent.getAttribute(
        'nombre',
      ),
    ).toContain('Laura');

    expect(
      webComponent.getAttribute(
        'nss',
      ),
    ).toBe('NSS001');
  });

  it('abre el formulario para crear un paciente', () => {
    const fixture =
        TestBed.createComponent(
        PacientesComponent,
        );

    const component =
        fixture.componentInstance;

    fixture.detectChanges();

    httpMock
        .expectOne(
        'http://localhost:5134/api/pacientes',
        )
        .flush([]);

    httpMock
        .expectOne(
        'http://localhost:5134/api/medicos',
        )
        .flush([]);

    component.newItem();

    fixture.detectChanges();

    expect(
        component.formVisible(),
    ).toBe(true);

    expect(
        component.editingId(),
    ).toBeNull();
    });

    it('selecciona y deselecciona médicos relacionados', () => {
    const fixture =
        TestBed.createComponent(
        PacientesComponent,
        );

    const component =
        fixture.componentInstance;

    fixture.detectChanges();

    httpMock
        .expectOne(
        'http://localhost:5134/api/pacientes',
        )
        .flush([]);

    httpMock
        .expectOne(
        'http://localhost:5134/api/medicos',
        )
        .flush([]);

    component.toggleDoctor(1, true);

    expect(
        component.selectedDoctors(),
    ).toContain(1);

    component.toggleDoctor(1, false);

    expect(
        component.selectedDoctors(),
    ).not.toContain(1);
  });

  it('muestra los nombres de los médicos relacionados', () => {
    const fixture =
        TestBed.createComponent(
        PacientesComponent,
        );

    const component =
        fixture.componentInstance;

    fixture.detectChanges();

    httpMock
        .expectOne(
        'http://localhost:5134/api/pacientes',
        )
        .flush([]);

    httpMock
        .expectOne(
        'http://localhost:5134/api/medicos',
        )
        .flush([
        {
            id: 1,
            nombre: 'Ana',
            apellidos: 'Martínez',
            usuario: 'ana',
            numColegiado: 'MED001',
            pacienteIds: [],
        },
        ]);

    const paciente = {
        id: 1,
        nombre: 'Laura',
        apellidos: 'Pérez',
        usuario: 'laura',
        clave: '',
        nss: 'NSS001',
        numTarjeta: 'T001',
        telefono: '612345678',
        direccion: 'Granada',
        medicoIds: [1],
    } as any;

    expect(
        component.doctorNames(paciente),
    ).toContain('Ana Martínez');
  });

  it('cierra el formulario y limpia el id de edición', () => {
    const fixture =
        TestBed.createComponent(
        PacientesComponent,
        );

    const component =
        fixture.componentInstance;

    fixture.detectChanges();

    httpMock
        .expectOne(
        'http://localhost:5134/api/pacientes',
        )
        .flush([]);

    httpMock
        .expectOne(
        'http://localhost:5134/api/medicos',
        )
        .flush([]);

    component.editingId.set(1);
    component.formVisible.set(true);

    component.closeForm();

    expect(
        component.formVisible(),
    ).toBe(false);

    expect(
        component.editingId(),
    ).toBeNull();
    });
});