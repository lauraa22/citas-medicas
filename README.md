# Gestión de Citas Médicas

Aplicación **full stack** para la gestión de usuarios, pacientes, médicos, citas y diagnósticos.

El proyecto está compuesto por un frontend desarrollado con **Angular 22** y una API REST desarrollada con **.NET 10**, **Entity Framework Core** y **SQL Server Express**.

---

## Estructura del proyecto

```text
citas-medicas/
├── frontend/              # Aplicación Angular 22
├── backend/               # API REST .NET 10
├── postman/               # Colección de Postman para probar la API
├── .gitignore
└── README.md
```

---

## Tecnologías utilizadas

### Frontend

- Angular 22
- TypeScript
- Standalone Components
- Reactive Forms
- Angular Signals (`signal`, `computed`)
- Angular Elements / Web Components
- Angular Router
- HttpClient
- RxJS
- Vitest
- Cypress

### Backend

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server Express
- AutoMapper
- Repository Pattern
- Unit of Work
- Use Cases
- Inyección de dependencias
- Transacciones con Entity Framework Core
- Global Exception Handler
- OpenAPI
- Swagger UI

### Testing backend

- xUnit
- Moq
- Microsoft.AspNetCore.Mvc.Testing
- WebApplicationFactory
- Entity Framework Core
- SQL Server Express
- Coverlet
- ReportGenerator

---

## Arquitectura

El backend está organizado por capas:

```text
CitasMedicas.Api
        ↓
CitasMedicas.Application
        ↓
CitasMedicas.Domain


CitasMedicas.Infrastructure
        ↓
CitasMedicas.Domain
```

El flujo habitual de una operación es:

```text
Angular
   ↓
HttpClient
   ↓
Controller REST
   ↓
AutoMapper
   ↓
UseCase
   ↓
Unit of Work / Repository
   ↓
Entity Framework Core
   ↓
SQL Server
```

### `CitasMedicas.Domain`

Contiene las entidades de dominio:

- `Usuario`
- `Paciente`
- `Medico`
- `Cita`
- `Diagnostico`

y las interfaces de repositorio:

- `IGenericRepository`
- `IPacienteRepository`
- `IMedicoRepository`
- `IUnitOfWork`

El uso de interfaces permite desacoplar la lógica de aplicación de la implementación concreta de persistencia.


### `CitasMedicas.Application`

Contiene la lógica de aplicación.

Incluye:

- Modelos internos de aplicación.
- Casos de uso.
- Configuración de AutoMapper entre entidades y modelos.

Los casos de uso están separados por operación.

Cada UseCase tiene una responsabilidad concreta y expone un método principal:


### `CitasMedicas.Infrastructure`

Contiene la implementación del acceso a datos:

- `CitasMedicasDbContext`
- Repositorios SQL Server
- Implementación de `UnitOfWork`
- Configuración de Entity Framework Core
- Migraciones

### `CitasMedicas.Api`

Contiene:

- Controladores REST
- DTOs
- AutoMapper entre DTOs y modelos de Application
- Configuración de inyección de dependencias
- CORS
- Global Exception Handler
- OpenAPI
- Swagger UI

---

## Modelo de datos

### Herencia

`Paciente` y `Medico` heredan de `Usuario`.

`Usuario` es una clase base que también puede existir como entidad independiente.

Entity Framework Core utiliza una estrategia de herencia **TPH (Table Per Hierarchy)**. Los usuarios, pacientes y médicos se almacenan en la tabla `Usuarios` y se diferencian mediante un discriminador.

### Relaciones

- Paciente ↔ Médico: **N:M**
- Paciente → Cita: **1:N**
- Médico → Cita: **1:N**
- Cita → Diagnóstico: **0..1 : 1**

La relación N:M entre pacientes y médicos se representa mediante la tabla intermedia:

```text
PacienteMedico
```

El diagnóstico de una cita es opcional porque una cita puede existir antes de que el especialista emita un diagnóstico.

---

## Funcionalidades

La aplicación permite realizar operaciones CRUD sobre:

- Usuarios
- Pacientes
- Médicos
- Citas
- Diagnósticos

También incluye:

- Dashboard con contadores reactivos
- Buscador de pacientes
- Filtro de citas por médico
- Gestión de la relación N:M entre pacientes y médicos
- Confirmación antes de eliminar registros
- Sistema de notificaciones
- Formularios reactivos con validaciones
- Consumo de la API REST mediante `HttpClient`
- Estado reactivo mediante Angular Signals
- Web Components creados con Angular Elements

### Web Components

Se han creado e integrado tres Web Components:

```html
<paciente-resumen></paciente-resumen>
<medico-resumen></medico-resumen>
<cita-resumen></cita-resumen>
```

---

## AutoMapper

AutoMapper se utiliza en dos puntos diferentes.

- API

Transformación entre: DTO ↔ Application Model mediante: ApiMappingProfile

- Application

Transformación entre: Application Model ↔ Domain Entity mediante: MappingProfile

Esto permite mantener separadas las representaciones utilizadas por cada capa.

---

## Manejo global de errores

La API utiliza un GlobalExceptionHandler basado en IExceptionHandler.

Permite transformar excepciones de aplicación en respuestas HTTP mediante ProblemDetails.

Ejemplos:

InvalidOperationException → 400 Bad Request
KeyNotFoundException      → 404 Not Found
Otros errores             → 500 Internal Server Error

Esto evita repetir bloques try/catch en cada Controller y centraliza el tratamiento de errores.


---

## Persistencia

Los datos de la aplicación se almacenan en **SQL Server Express**.

La base de datos utilizada es:

```text
CitasMedicasDb
```

La cadena de conexión se encuentra en:

```text
backend/CitasMedicas.Api/appsettings.json
```

Configuración utilizada en desarrollo:

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=CitasMedicasDb;Trusted_Connection=True;TrustServerCertificate=True;"
```

> La configuración anterior está pensada para un entorno local con una instancia `SQLEXPRESS`. Puede ser necesario modificarla si se utiliza otra instancia de SQL Server.

---

## Crear o actualizar la base de datos

Desde la carpeta `backend`:

```powershell
dotnet ef database update `
  --project CitasMedicas.Infrastructure `
  --startup-project CitasMedicas.Api
```

Para comprobar si existen cambios pendientes en el modelo:

```powershell
dotnet ef migrations has-pending-model-changes `
  --project CitasMedicas.Infrastructure `
  --startup-project CitasMedicas.Api
```

Para crear una nueva migración:

```powershell
dotnet ef migrations add NombreMigracion `
  --project CitasMedicas.Infrastructure `
  --startup-project CitasMedicas.Api `
  --output-dir Persistence/Migrations
```

---

## Ejecutar el backend

Desde la carpeta `backend`:

```powershell
dotnet restore
dotnet build
dotnet run --project CitasMedicas.Api
```

Durante el desarrollo, la API está configurada para trabajar con el frontend ejecutado en:

```text
http://localhost:4200
```

La API se ejecuta normalmente en:

```text
http://localhost:5134
```

### Swagger

Con el backend ejecutándose en modo Development:

```text
http://localhost:5134/swagger
```

### OpenAPI

```text
http://localhost:5134/openapi/v1.json
```

---

## Ejecutar el frontend

Desde la carpeta `frontend`:

```powershell
npm install
npm start
```

La aplicación se ejecuta normalmente en:

```text
http://localhost:4200
```

El backend debe estar en ejecución para que Usuarios, Pacientes, Médicos, Citas y Diagnósticos puedan consultar y persistir datos.

---

## Endpoints principales

### Usuarios

```text
GET    /api/usuarios
GET    /api/usuarios/{id}
POST   /api/usuarios
PUT    /api/usuarios/{id}
DELETE /api/usuarios/{id}
```

### Pacientes

```text
GET    /api/pacientes
GET    /api/pacientes/{id}
POST   /api/pacientes
PUT    /api/pacientes/{id}
DELETE /api/pacientes/{id}
```

### Médicos

```text
GET    /api/medicos
GET    /api/medicos/{id}
POST   /api/medicos
PUT    /api/medicos/{id}
DELETE /api/medicos/{id}
```

### Citas

```text
GET    /api/citas
GET    /api/citas/{id}
POST   /api/citas
PUT    /api/citas/{id}
DELETE /api/citas/{id}
```

### Diagnósticos

```text
GET    /api/diagnosticos
GET    /api/diagnosticos/{id}
POST   /api/diagnosticos
PUT    /api/diagnosticos/{id}
DELETE /api/diagnosticos/{id}
```

---

## Testing

El proyecto utiliza diferentes niveles de pruebas.

### Tests unitarios del backend

Ubicación: backend/CitasMedicas.Tests

Tecnologías:

- xUnit
- Moq
- AutoMapper
- Coverlet

Las pruebas unitarias validan los casos de uso de forma aislada.

Se utilizan mocks para sustituir:

- Repositorios.
- Unit of Work.
- Dependencias externas.

Se comprueban, entre otros:

- CRUD de Usuarios.
- CRUD de Pacientes.
- CRUD de Médicos.
- CRUD de Citas.
- CRUD de Diagnósticos.
- Recursos inexistentes.
- Validación de relaciones.
- Transacciones.
- Rollback.
- Conservación de contraseñas.
- Restricciones entre Usuario, Paciente y Médico.
- Mapeos de AutoMapper.

Ejecutar:

```powershell
cd backend
dotnet test .\CitasMedicas.Tests\CitasMedicas.Tests.csproj
```

Cobertura:
```powershell
dotnet test .\CitasMedicas.Tests\CitasMedicas.Tests.csproj --collect:"XPlat Code Coverage"
```

### Tests de integración del backend

Ubicación: backend/CitasMedicas.IntegrationTests

Tecnologías:

- xUnit
- Microsoft.AspNetCore.Mvc.Testing
- WebApplicationFactory
- HttpClient
- Entity Framework Core
- SQL Server Express

Los tests de integración comprueban el funcionamiento conjunto de:

HTTP
↓
Controller
↓
AutoMapper
↓
UseCase
↓
Repository / UnitOfWork
↓
Entity Framework Core
↓
SQL Server

Para evitar modificar la base de datos de desarrollo, utilizan una base independiente: CitasMedicasTestDb

Antes de cada test se reinicia la base mediante:

EnsureDeletedAsync();
EnsureCreatedAsync();

Actualmente se comprueban escenarios como:

- GET de pacientes.
- Recurso inexistente → 404.
- Creación de paciente → 201.
- La contraseña no se devuelve en la respuesta.
- Actualización conservando la contraseña anterior.
- Eliminación de paciente.
- Creación de cita válida.
- Cita con paciente inexistente → 400.
- Funcionamiento del GlobalExceptionHandler.
- Restricción de eliminar un paciente desde Usuarios.
- Persistencia de la relación Paciente-Médico.

Ejecutar únicamente integración:

```powershell
cd backend
dotnet test .\CitasMedicas.IntegrationTests\CitasMedicas.IntegrationTests.csproj
```


Ejecutar todos los tests del backend:

```powershell
cd backend
dotnet test
```


Cobertura de integración:
```powershell
dotnet test .\CitasMedicas.IntegrationTests\CitasMedicas.IntegrationTests.csproj --collect:"XPlat Code Coverage"
Generar informe HTML de cobertura
```


Ejemplo para integración:
```powershell
reportgenerator `
-reports:"CitasMedicas.IntegrationTests\TestResults\**\coverage.cobertura.xml" `
-targetdir:"CitasMedicas.IntegrationTests\CoverageReport" `
-reporttypes:Html
```


Abrir: backend/CitasMedicas.IntegrationTests/CoverageReport/index.html

Las carpetas de resultados y cobertura no se versionan en Git.



### Pruebas unitarias con Vitest

Desde `frontend`:

```powershell
npm test
```

Para ejecutar las pruebas con cobertura:

```powershell
npm run test:coverage
```

El informe de cobertura se genera en:

```text
frontend/coverage/
```

### Pruebas E2E con Cypress

Para ejecutar Cypress deben estar levantados tanto el backend como el frontend.

**Terminal 1 — Backend**

```powershell
cd backend
dotnet run --project CitasMedicas.Api
```

**Terminal 2 — Frontend**

```powershell
cd frontend
npm start
```

**Terminal 3 — Cypress**

```powershell
cd frontend
npm run cypress:run
```

También puede abrirse Cypress en modo interactivo:

```powershell
npm run cypress:open
```

---

## Pruebas de la API con Postman

La colección de Postman se encuentra en:

```text
postman/CitasMedicas.postman_collection.json
```

Permite comprobar el funcionamiento de los principales endpoints de la API y sus relaciones.

También se pueden probar los endpoints directamente desde Swagger.

---

## Comprobación antes de ejecutar o entregar

### Backend

```powershell
cd backend
dotnet clean
dotnet restore
dotnet build
dotnet test
```

### Frontend

```powershell
cd frontend
npm install
npm run build
npm test
npm run test:coverage
```

### E2E

Con frontend y backend ejecutándose:

```powershell
cd frontend
npm run cypress:run
```

---

## Simplificaciones del ejercicio

Para mantener el proyecto centrado en los requisitos principales:

- No se implementan estados de las citas.
- No se implementa ordenación de tablas.
- Se utiliza un buscador en Pacientes.
- Se utiliza un filtro de Citas por Médico.
- No se implementa autenticación JWT.
- Se utiliza Angular Signals para la gestión de estado reactivo sin introducir una librería adicional como NgRx.
- Las URLs de la API están configuradas para el entorno local de desarrollo.

La propiedad `Clave` forma parte del modelo del ejercicio. Este proyecto no debe considerarse un sistema de autenticación real. En una aplicación de producción las contraseñas deberían almacenarse mediante un mecanismo de hashing seguro y nunca como texto plano.

---

## Consideraciones de diseño

- Se utilizan DTOs para separar las entidades de dominio de los datos expuestos por la API.
- La propiedad `Clave` no se devuelve en los DTOs de lectura.
- AutoMapper realiza las conversiones entre DTOs, modelos de aplicación y entidades.
- La lógica de negocio se organiza mediante UseCases.
- Cada UseCase tiene una responsabilidad concreta.
- Repository y Unit of Work encapsulan el acceso a datos.
- Las operaciones que modifican entidades y relaciones utilizan transacciones cuando es necesario.
- `Paciente` y `Medico` disponen de sus propias operaciones, aunque ambos heredan de `Usuario`.
- El diagnóstico se mantiene opcional en una cita para permitir crear la cita antes de que exista una valoración médica.
- Los errores HTTP se gestionan de forma centralizada mediante GlobalExceptionHandler.
- Los tests de integración utilizan una base SQL Server exclusiva para evitar modificar los datos de desarrollo.
---

## Autor

Proyecto desarrollado como ejercicio práctico de formación.

Laura Guirao Torrente


