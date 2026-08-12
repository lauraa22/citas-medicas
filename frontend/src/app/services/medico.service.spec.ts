import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';

import { MedicoService } from './medico.service';

describe('MedicoService', () => {
  let service: MedicoService;
  let httpMock: HttpTestingController;

  const apiUrl =
    'http://localhost:5134/api/medicos';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    service =
      TestBed.inject(MedicoService);

    httpMock =
      TestBed.inject(
        HttpTestingController,
      );
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('carga los médicos', () => {
    service.load();

    const req =
      httpMock.expectOne(apiUrl);

    expect(req.request.method).toBe(
      'GET',
    );

    req.flush([
      {
        id: 1,
        nombre: 'Ana',
        apellidos:
          'Martínez López',
        usuario: 'ana.medico',
        numColegiado: 'MED001',
        pacienteIds: [1],
      },
    ]);

    expect(
      service.medicos().length,
    ).toBe(1);

    expect(
      service.medicos()[0]
        .numColegiado,
    ).toBe('MED001');
  });

  it('crea un médico con pacientes asociados', () => {
    const data = {
      nombre: 'Doc',
      apellidos: 'Test',
      usuario: 'doc',
      clave: '1234',
      numColegiado: 'COLX',
      pacienteIds: [1],
    };

    service
      .create(data)
      .subscribe((created) => {
        expect(created.id).toBe(10);

        expect(
          created.pacienteIds,
        ).toContain(1);
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
      nombre: data.nombre,
      apellidos: data.apellidos,
      usuario: data.usuario,
      numColegiado:
        data.numColegiado,
      pacienteIds:
        data.pacienteIds,
    });

    expect(
      service.findById(10)
        ?.pacienteIds,
    ).toContain(1);
  });

  it('actualiza un médico', () => {
    const data = {
      nombre: 'Ana',
      apellidos:
        'Martínez López',
      usuario: 'ana.medico',
      clave: '1234',
      numColegiado: 'NUEVO',
      pacienteIds: [1],
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

    expect(
      updateReq.request.body,
    ).toEqual(data);

    updateReq.flush(null);

    // update() recarga los médicos
    const loadReq =
      httpMock.expectOne(apiUrl);

    expect(
      loadReq.request.method,
    ).toBe('GET');

    loadReq.flush([
      {
        id: 1,
        nombre: data.nombre,
        apellidos:
          data.apellidos,
        usuario:
          data.usuario,
        numColegiado:
          data.numColegiado,
        pacienteIds:
          data.pacienteIds,
      },
    ]);

    expect(
      service.findById(1)
        ?.numColegiado,
    ).toBe('NUEVO');
  });

  it('elimina un médico', () => {
    service.load();

    const loadReq =
      httpMock.expectOne(apiUrl);

    loadReq.flush([
      {
        id: 1,
        nombre: 'Ana',
        apellidos:
          'Martínez López',
        usuario: 'ana.medico',
        numColegiado: 'MED001',
        pacienteIds: [],
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