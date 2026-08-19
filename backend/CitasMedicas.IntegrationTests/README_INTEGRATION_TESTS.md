# Tests de integración - SQL Server

Este proyecto ejecuta tests de integración contra una base SQL Server Express
separada de la base normal de la aplicación.

Base usada por los tests:

```text
.\SQLEXPRESS
CitasMedicasTestDb
```

IMPORTANTE: los tests NO deben apuntar a `CitasMedicasDb`.

## 1. Cambio necesario en Program.cs

Al final de `CitasMedicas.Api/Program.cs`, después de:

```csharp
app.Run();
```

añade:

```csharp
public partial class Program { }
```

Quedará:

```csharp
app.MapControllers();

app.Run();

public partial class Program { }
```

## 2. Colocar el proyecto

Copia esta carpeta dentro de `backend` y deja esta estructura:

```text
backend/
├── CitasMedicas.Api/
├── CitasMedicas.Application/
├── CitasMedicas.Domain/
├── CitasMedicas.Infrastructure/
├── CitasMedicas.Tests/
└── CitasMedicas.IntegrationTests/
```

## 3. Añadir a la solución

Desde `backend`:

```powershell
dotnet sln add .\CitasMedicas.IntegrationTests\CitasMedicas.IntegrationTests.csproj
dotnet restore
```

## 4. Ejecutar solo integración

```powershell
dotnet test .\CitasMedicas.IntegrationTests\CitasMedicas.IntegrationTests.csproj
```

## 5. Ejecutar todos los tests

```powershell
dotnet test
```

## Qué comprueban

- GET pacientes -> 200.
- GET paciente inexistente -> 404.
- POST paciente -> 201.
- La clave no aparece en la respuesta.
- PUT paciente con clave nula conserva la contraseña anterior.
- DELETE paciente -> 204 y después 404.
- POST cita con paciente inexistente -> 400.
- GlobalExceptionHandler devuelve ProblemDetails.
- POST cita con paciente y médico válidos -> 201.
- No se permite eliminar un Paciente desde /api/Usuarios.
- La relación Paciente-Médico se persiste correctamente.

## Limpieza

Antes de cada test se ejecutan:

```csharp
EnsureDeletedAsync();
EnsureCreatedAsync();
```

sobre `CitasMedicasTestDb`.

Esto hace que cada test empiece con una base limpia.

NUNCA cambies la cadena de conexión del factory para usar `CitasMedicasDb`,
porque los tests eliminan y vuelven a crear la base.
