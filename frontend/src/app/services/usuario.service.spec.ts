import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';

import {
  HttpTestingController,
  provideHttpClientTesting,
} from '@angular/common/http/testing';

import { UsuarioService } from './usuario.service';

describe('UsuarioService', () => {
  let service: UsuarioService;
  let httpMock: HttpTestingController;

  const apiUrl =
    'http://localhost:5134/api/usuarios';

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
      ],
    });

    service =
      TestBed.inject(UsuarioService);

    httpMock =
      TestBed.inject(
        HttpTestingController,
      );
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('carga usuarios desde la API', () => {
    service.load();

    const request =
      httpMock.expectOne(apiUrl);

    expect(
      request.request.method,
    ).toBe('GET');

    request.flush([
      {
        id: 1,
        nombre: 'Laura',
        apellidos: 'Test',
        usuario: 'laura',
      },
    ]);

    expect(
      service.usuarios().length,
    ).toBe(1);

    expect(
      service.usuarios()[0].usuario,
    ).toBe('laura');
  });

  it('crea un usuario', () => {
    const data = {
      nombre: 'Eva',
      apellidos: 'Test',
      usuario: 'eva.test',
      clave: '1234',
    };

    service
      .create(data)
      .subscribe((created) => {
        expect(created.id).toBe(2);

        expect(
          created.usuario,
        ).toBe('eva.test');
      });

    const request =
      httpMock.expectOne(apiUrl);

    expect(
      request.request.method,
    ).toBe('POST');

    expect(
      request.request.body,
    ).toEqual(data);

    request.flush({
      id: 2,
      nombre: 'Eva',
      apellidos: 'Test',
      usuario: 'eva.test',
    });

    expect(
      service.findById(2),
    ).toBeTruthy();
  });

  it('actualiza un usuario', () => {
    const data = {
      nombre: 'Evelyn',
      apellidos: 'Test',
      usuario: 'eva.test',
      clave: '1234',
    };

    service
      .update(2, data)
      .subscribe();

    const updateRequest =
      httpMock.expectOne(
        `${apiUrl}/2`,
      );

    expect(
      updateRequest.request.method,
    ).toBe('PUT');

    updateRequest.flush(null);

    const loadRequest =
      httpMock.expectOne(apiUrl);

    expect(
      loadRequest.request.method,
    ).toBe('GET');

    loadRequest.flush([
      {
        id: 2,
        nombre: 'Evelyn',
        apellidos: 'Test',
        usuario: 'eva.test',
      },
    ]);

    expect(
      service.findById(2)?.nombre,
    ).toBe('Evelyn');
  });

  it('elimina un usuario', () => {
    service.load();

    httpMock
      .expectOne(apiUrl)
      .flush([
        {
          id: 2,
          nombre: 'Eva',
          apellidos: 'Test',
          usuario: 'eva.test',
        },
      ]);

    service
      .delete(2)
      .subscribe();

    const request =
      httpMock.expectOne(
        `${apiUrl}/2`,
      );

    expect(
      request.request.method,
    ).toBe('DELETE');

    request.flush(null);

    expect(
      service.findById(2),
    ).toBeUndefined();
  });

  it('detecta nombres de usuario duplicados', () => {
    service.load();

    httpMock
      .expectOne(apiUrl)
      .flush([
        {
          id: 1,
          nombre: 'Laura',
          apellidos: 'Test',
          usuario: 'laura',
        },
      ]);

    expect(
      service.usernameExists(
        'LAURA',
      ),
    ).toBe(true);

    expect(
      service.usernameExists(
        'laura',
        1,
      ),
    ).toBe(false);
  });
});