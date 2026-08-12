import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';

import { CitaService } from './cita.service';

describe('CitaService', () => {
  let service: CitaService;
  let httpMock: HttpTestingController;

  const apiUrl =
    'http://localhost:5134/api/citas';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    service = TestBed.inject(CitaService);
    httpMock =
      TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('carga las citas', () => {
    service.load();

    const req =
      httpMock.expectOne(apiUrl);

    expect(req.request.method).toBe('GET');

    req.flush([
      {
        id: 1,
        fechaHora:
          '2026-08-20T10:00:00',
        motivoCita: 'Consulta',
        pacienteId: 1,
        medicoId: 2,
        diagnosticoId: null,
      },
    ]);

    expect(service.citas().length).toBe(1);

    expect(
      service.citas()[0].motivoCita,
    ).toBe('Consulta');
  });

  it('filtra citas por médico', () => {
    service.load();

    const req =
      httpMock.expectOne(apiUrl);

    req.flush([
      {
        id: 1,
        fechaHora:
          '2026-08-20T10:00:00',
        motivoCita: 'Primera',
        pacienteId: 1,
        medicoId: 2,
        diagnosticoId: null,
      },
      {
        id: 2,
        fechaHora:
          '2026-08-21T11:00:00',
        motivoCita: 'Segunda',
        pacienteId: 2,
        medicoId: 3,
        diagnosticoId: null,
      },
    ]);

    expect(
      service.byDoctor(2).length,
    ).toBe(1);

    expect(
      service.byDoctor(2)[0]
        .medicoId,
    ).toBe(2);

    expect(
      service.byDoctor(null).length,
    ).toBe(2);
  });

  it('crea una cita', () => {
    const data = {
      fechaHora:
        '2026-08-20T10:00',
      motivoCita: 'Test',
      pacienteId: 1,
      medicoId: 2,
      diagnosticoId: null,
    };

    service
      .create(data)
      .subscribe((created) => {
        expect(created.id).toBe(10);
        expect(
          created.motivoCita,
        ).toBe('Test');
      });

    const req =
      httpMock.expectOne(apiUrl);

    expect(req.request.method).toBe(
      'POST',
    );

    expect(req.request.body).toEqual(
      data,
    );

    req.flush({
      id: 10,
      ...data,
    });

    expect(
      service.findById(10),
    ).toBeTruthy();
  });

  it('actualiza una cita', () => {
    const data = {
      fechaHora:
        '2026-08-20T10:00',
      motivoCita: 'Cambio',
      pacienteId: 1,
      medicoId: 2,
      diagnosticoId: null,
    };

    service
      .update(10, data)
      .subscribe();

    const updateReq =
      httpMock.expectOne(
        `${apiUrl}/10`,
      );

    expect(
      updateReq.request.method,
    ).toBe('PUT');

    updateReq.flush(null);

    // update() llama a load()
    const loadReq =
      httpMock.expectOne(apiUrl);

    expect(
      loadReq.request.method,
    ).toBe('GET');

    loadReq.flush([
      {
        id: 10,
        ...data,
      },
    ]);

    expect(
      service.findById(10)
        ?.motivoCita,
    ).toBe('Cambio');
  });

  it('elimina una cita', () => {
    service.load();

    const loadReq =
      httpMock.expectOne(apiUrl);

    loadReq.flush([
      {
        id: 1,
        fechaHora:
          '2026-08-20T10:00:00',
        motivoCita: 'Test',
        pacienteId: 1,
        medicoId: 2,
        diagnosticoId: null,
      },
    ]);

    service.delete(1).subscribe();

    const deleteReq =
      httpMock.expectOne(
        `${apiUrl}/1`,
      );

    expect(
      deleteReq.request.method,
    ).toBe('DELETE');

    deleteReq.flush(null);

    expect(
      service.findById(1),
    ).toBeUndefined();
  });
});