# Tests del backend - Citas Médicas

Este proyecto contiene pruebas unitarias con xUnit + Moq para los casos de uso del backend.

## Qué cubre

- Citas: GET por id, GET all, creación correcta, referencias inválidas, actualización, borrado.
- Diagnósticos: CRUD y recursos inexistentes.
- Usuarios: CRUD, conservación/cambio de contraseña y bloqueo de edición/borrado de Paciente/Medico desde Usuarios.
- Pacientes: consultas con médicos, creación/actualización transaccional, rollback, contraseña y borrado.
- Médicos: consultas con pacientes, creación/actualización transaccional, rollback, contraseña y borrado.

## Instalación

Copia la carpeta `CitasMedicas.Tests` dentro de `backend`, al mismo nivel que:
- CitasMedicas.Api
- CitasMedicas.Application
- CitasMedicas.Domain
- CitasMedicas.Infrastructure

Después, desde `backend`:

```powershell
dotnet sln add .\CitasMedicas.Tests\CitasMedicas.Tests.csproj
dotnet restore
dotnet test
```

Cobertura:

```powershell
dotnet test --collect:"XPlat Code Coverage"
```

## Pruebas manuales/integración que también debes hacer

Además de las unitarias, valida la API real en Swagger/Postman:

1. GET de las cinco entidades -> 200.
2. GET id inexistente -> 404.
3. POST válido -> 201.
4. PUT válido -> 204.
5. DELETE válido -> 204.
6. Cita con paciente/médico/diagnóstico inexistente -> 400 mediante GlobalExceptionHandler.
7. Paciente con médico inexistente -> 400 y no debe persistir cambios.
8. Médico con paciente inexistente -> 400 y no debe persistir cambios.
9. Editar paciente/médico con clave vacía -> conserva contraseña.
10. Intentar actualizar/eliminar un Paciente o Medico desde /api/Usuarios -> 400.
11. GET Usuario/Paciente/Medico -> no debe devolver `clave`.
12. Relaciones Paciente-Medico -> los ids relacionados deben devolverse correctamente.

Estas pruebas manuales comprueban Controller + AutoMapper API + manejador de excepciones + EF Core + SQL Server, cosas que las unitarias aisladas no cubren.
