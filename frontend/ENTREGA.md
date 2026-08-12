# Guía rápida de la entrega

## Qué se demuestra

* CRUD completo de Usuarios, Pacientes, Médicos, Citas y Diagnósticos.
* IDs numéricos en todas las entidades.
* Herencia entre Usuario, Paciente y Médico.
* Relación N:M Paciente-Médico sincronizada mediante IDs.
* Citas relacionadas con Paciente, Médico y Diagnóstico.
* Angular Signals mediante `signal`, `computed` y `effect`.
* Dashboard con contadores reactivos.
* Buscador de pacientes.
* Filtro de citas por médico.
* Modal de confirmación para eliminar.
* Sistema de notificaciones.
* 3 Web Components mediante Angular Elements.
* Tests unitarios con Vitest.
* Tests E2E con Cypress.

## Simplificaciones

Para mantener el ejercicio sencillo se ha decidido:

* No añadir estados a las citas.
* No implementar ordenación.
* Utilizar un único buscador, en Pacientes.
* Utilizar un único filtro, en Citas por Médico.

## Ejecución

Instalar dependencias:

```bash
npm install
```

Iniciar la aplicación:

```bash
npm start
```

## Vitest

Ejecutar tests unitarios:

```bash
npm test
```

Ejecutar cobertura:

```bash
npm run test:coverage
```

## Cypress

Con la aplicación arrancada en otra terminal:

```bash
npm run cypress:open
```

Para ejecutar todos los tests E2E directamente:

```bash
npm run cypress:run
```

## Comprobación antes de entregar

Ejecutar:

```bash
npm run build
npm test
npm run cypress:run
```

La entrega debería realizarse únicamente cuando el build y todas las pruebas finalicen correctamente.
