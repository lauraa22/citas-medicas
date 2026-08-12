# Gestión de Citas Médicas — Angular 22

Aplicación frontend desarrollada con Angular 22 para la gestión de citas médicas.

El objetivo del ejercicio es demostrar el uso de Angular moderno, Signals, Web Components, testing unitario y pruebas E2E mediante una aplicación sencilla pero completa.

## Tecnologías utilizadas

* Angular 22.
* TypeScript.
* Standalone Components.
* Reactive Forms.
* Angular Signals.
* Angular Elements.
* Web Components.
* Angular Router.
* Builder moderno de Angular (`@angular/build:application`) con servidor de desarrollo basado en Vite.
* Vitest.
* Cypress.
* LocalStorage para persistencia frontend.

## Funcionalidades

La aplicación incluye CRUD completo de:

* Usuarios.
* Pacientes.
* Médicos.
* Citas.
* Diagnósticos.

También incluye:

* Dashboard con contadores reactivos.
* Buscador de pacientes.
* Filtro de citas por médico.
* Modal de confirmación antes de eliminar registros.
* Sistema de notificaciones de éxito y error.
* Persistencia de datos mediante LocalStorage.

## Angular Signals

Se utilizan Signals para gestionar el estado reactivo de la aplicación:

* `signal()` para almacenar estado.
* `computed()` para valores derivados.
* `effect()` para efectos secundarios, como la persistencia automática en LocalStorage.

Ejemplos de uso:

* Stores reactivos.
* Contadores del Dashboard.
* Buscador de pacientes.
* Filtro de citas por médico.

## Modelo de datos y relaciones

Todas las entidades utilizan IDs numéricos.

### Herencia

* `Paciente` hereda de `Usuario`.
* `Medico` hereda de `Usuario`.

### Relaciones

* Paciente y Médico mantienen una relación N:M mediante IDs.
* `Paciente` almacena `medicoIds`.
* `Medico` almacena `pacienteIds`.
* `PacienteService` y `MedicoService` sincronizan ambos lados de la relación.
* `Cita` referencia a Paciente mediante `pacienteId`.
* `Cita` referencia a Médico mediante `medicoId`.
* `Cita` referencia a Diagnóstico mediante `diagnosticoId`.
* `Diagnostico` dispone de su propio ID.

Las relaciones mediante IDs permiten evitar la duplicación de objetos completos dentro de las citas.

## Web Components

Se han creado tres Web Components mediante Angular Elements:

* `<paciente-resumen>`
* `<medico-resumen>`
* `<cita-resumen>`

Estos Web Components están integrados en las vistas reales de detalle de Pacientes, Médicos y Citas.

## Testing

### Vitest

Se utilizan pruebas unitarias para comprobar:

* Servicios.
* Operaciones CRUD.
* Relaciones entre Pacientes y Médicos.
* Buscador de pacientes.
* Dashboard.
* Modal de confirmación.
* Sistema de notificaciones.
* Angular Elements.

Para ejecutar los tests:

```bash
npm test
```

Para ejecutar cobertura:

```bash
npm run test:coverage
```

### Cypress

Se utilizan pruebas E2E para comprobar los principales flujos de usuario:

* Dashboard.
* Usuarios.
* Pacientes.
* Médicos.
* Citas.
* Diagnósticos.
* Buscador de pacientes.
* Filtro de citas.
* Web Components integrados.

Con la aplicación ejecutándose en otra terminal:

```bash
npm run cypress:open
```

También se pueden ejecutar directamente todos los tests:

```bash
npm run cypress:run
```

## Instalación y ejecución

Instalar las dependencias:

```bash
npm install
```

Iniciar la aplicación:

```bash
npm start
```

La aplicación estará disponible normalmente en:

```text
http://localhost:4200
```

## Simplificaciones realizadas

Se ha mantenido el ejercicio intencionadamente sencillo para centrarlo en las tecnologías solicitadas.

No se han implementado:

* Estados de las citas.
* Ordenación de tablas.
* Múltiples buscadores.
* Múltiples filtros.

Se ha implementado únicamente:

* Un buscador en Pacientes.
* Un filtro de Citas por Médico.

## Mejoras futuras

Como posibles mejoras se plantean:

* Crear un componente reutilizable para los botones de la aplicación.
* Añadir validaciones adicionales en formularios.
* Mejorar la integridad referencial al eliminar entidades relacionadas.
* Ampliar los tests unitarios de los componentes.
* Integrar un backend mediante .NET y Entity Framework Core.
* Sustituir LocalStorage por persistencia real en base de datos.
* Incorporar autenticación y gestión segura de usuarios.

En esta primera versión se ha priorizado demostrar correctamente Angular, Signals, Web Components, Vitest y Cypress antes de añadir funcionalidades adicionales.
