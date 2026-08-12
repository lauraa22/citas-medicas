import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';

import { PacienteService } from './paciente.service';

describe('PacienteService', () => {
  let service: PacienteService;
  let httpMock: HttpTestingController;

  const apiUrl =
    'http://localhost:5134/api/pacientes';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    service =
      TestBed.inject(
        PacienteService,
      );

    httpMock =
      TestBed.inject(
        HttpTestingController,
      );
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('carga pacientes desde la API', () => {
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
        apellidos: 'Pérez',
        usuario: 'ana',
        nss: 'NSS001',
        numTarjeta: 'T001',
        telefono: '612345678',
        direccion: 'Granada',
        medicoIds: [1],
      },
      {
        id: 2,
        nombre: 'Pedro',
        apellidos: 'López',
        usuario: 'pedro',
        nss: 'NSS002',
        numTarjeta: 'T002',
        telefono: '623456789',
        direccion: 'Granada',
        medicoIds: [],
      },
    ]);

    expect(
      service.pacientes().length,
    ).toBe(2);
  });

  it('busca por nombre o NSS', () => {
    service.load();

    const req =
      httpMock.expectOne(apiUrl);

    req.flush([
      {
        id: 1,
        nombre: 'Ana',
        apellidos: 'Pérez',
        usuario: 'ana',
        nss: 'NSS001',
        numTarjeta: 'T001',
        telefono: '612345678',
        direccion: 'Granada',
        medicoIds: [1],
      },
      {
        id: 2,
        nombre: 'Pedro',
        apellidos: 'López',
        usuario: 'pedro',
        nss: 'NSS002',
        numTarjeta: 'T002',
        telefono: '623456789',
        direccion: 'Granada',
        medicoIds: [],
      },
    ]);

    expect(
      service.search('Ana').length,
    ).toBe(1);

    expect(
      service.search('NSS002')[0]
        .nombre,
    ).toBe('Pedro');
  });

  it('crea un paciente con médicos relacionados', () => {
    const data = {
      nombre: 'X',
      apellidos: 'Y',
      usuario: 'xy',
      clave: '1234',
      nss: 'NSSX',
      numTarjeta: 'TX',
      telefono: '600999999',
      direccion: 'C',
      medicoIds: [1],
    };

    service
      .create(data)
      .subscribe((created) => {
        expect(created.id).toBe(10);

        expect(
          created.medicoIds,
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
      apellidos:
        data.apellidos,
      usuario: data.usuario,
      nss: data.nss,
      numTarjeta:
        data.numTarjeta,
      telefono:
        data.telefono,
      direccion:
        data.direccion,
      medicoIds:
        data.medicoIds,
    });

    expect(
      service.findById(10)
        ?.medicoIds,
    ).toContain(1);
  });

  it('actualiza un paciente', () => {
    const data = {
      nombre: 'Ana',
      apellidos: 'Pérez',
      usuario: 'ana',
      clave: '1234',
      nss: 'NSS001',
      numTarjeta: 'T001',
      telefono: '612345678',
      direccion: 'Granada',
      medicoIds: [1],
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
        usuario: data.usuario,
        nss: data.nss,
        numTarjeta:
          data.numTarjeta,
        telefono:
          data.telefono,
        direccion:
          data.direccion,
        medicoIds:
          data.medicoIds,
      },
    ]);

    expect(
      service.findById(1)
        ?.telefono,
    ).toBe('612345678');
  });

  it('elimina un paciente', () => {
    service.load();

    const loadReq =
      httpMock.expectOne(apiUrl);

    loadReq.flush([
      {
        id: 1,
        nombre: 'Ana',
        apellidos: 'Pérez',
        usuario: 'ana',
        nss: 'NSS001',
        numTarjeta: 'T001',
        telefono: '612345678',
        direccion: 'Granada',
        medicoIds: [1],
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