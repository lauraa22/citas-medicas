import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';

import { DiagnosticoService } from './diagnostico.service';

describe('DiagnosticoService', () => {
  let service: DiagnosticoService;
  let httpMock: HttpTestingController;

  const apiUrl =
    'http://localhost:5134/api/diagnosticos';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    service =
      TestBed.inject(
        DiagnosticoService,
      );

    httpMock =
      TestBed.inject(
        HttpTestingController,
      );
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('carga diagnósticos', () => {
    service.load();

    const req =
      httpMock.expectOne(apiUrl);

    expect(req.request.method).toBe(
      'GET',
    );

    req.flush([
      {
        id: 1,
        enfermedad: 'Gripe',
        valoracionEspecialista:
          'Reposo',
      },
    ]);

    expect(
      service.diagnosticos().length,
    ).toBe(1);

    expect(
      service.diagnosticos()[0]
        .enfermedad,
    ).toBe('Gripe');
  });

  it('crea un diagnóstico', () => {
    const data = {
      enfermedad: 'Test',
      valoracionEspecialista:
        'Valor',
    };

    service
      .create(data)
      .subscribe((created) => {
        expect(created.id).toBe(2);

        expect(
          created.enfermedad,
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
      id: 2,
      ...data,
    });

    expect(
      service.findById(2)
        ?.enfermedad,
    ).toBe('Test');
  });

  it('actualiza un diagnóstico', () => {
    const data = {
      enfermedad: 'Cambio',
      valoracionEspecialista:
        'Nueva valoración',
    };

    service
      .update(1, data)
      .subscribe();

    const updateReq =
      httpMock.expectOne(
        `${apiUrl}/1`,
      );

    expect(
      updateReq.request.method,
    ).toBe('PUT');

    updateReq.flush(null);

    const loadReq =
      httpMock.expectOne(apiUrl);

    expect(
      loadReq.request.method,
    ).toBe('GET');

    loadReq.flush([
      {
        id: 1,
        ...data,
      },
    ]);

    expect(
      service.findById(1)
        ?.enfermedad,
    ).toBe('Cambio');
  });

  it('elimina un diagnóstico', () => {
    service.load();

    const loadReq =
      httpMock.expectOne(apiUrl);

    loadReq.flush([
      {
        id: 1,
        enfermedad: 'Test',
        valoracionEspecialista:
          'Valor',
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