using System.Net;
using System.Net.Http.Json;
using CitasMedicas.Api.DTOs;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Infrastructure.Persistence;
using CitasMedicas.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CitasMedicas.IntegrationTests.Api;

/// <summary>
/// Tests de integración de los principales flujos de la API.
/// Prueban HTTP + Controllers + AutoMapper + UseCases + Repositories
/// + EF Core + SQL Server.
/// </summary>
public class ApiIntegrationTests
    : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public ApiIntegrationTests(
        CustomWebApplicationFactory factory)
    {
        _factory = factory;

        _client = factory.CreateClient(
            new Microsoft.AspNetCore.Mvc.Testing
                .WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
    }

    [Fact]
    public async Task GetPacientes_EmptyDatabase_ReturnsOk()
    {
        await _factory.ResetDatabaseAsync();

        var response =
            await _client.GetAsync("/api/Pacientes");

        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);

        var pacientes =
            await response.Content
                .ReadFromJsonAsync<List<PacienteDto>>();

        Assert.NotNull(pacientes);
        Assert.Empty(pacientes);
    }

    [Fact]
    public async Task GetPaciente_UnknownId_ReturnsNotFound()
    {
        await _factory.ResetDatabaseAsync();

        var response =
            await _client.GetAsync(
                "/api/Pacientes/99999");

        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }

    [Fact]
    public async Task CreatePaciente_ValidData_ReturnsCreatedAndDoesNotExposePassword()
    {
        await _factory.ResetDatabaseAsync();

        var request =
            NewPaciente(
                "paciente.integracion",
                "NSS-INT-001");

        var response =
            await _client.PostAsJsonAsync(
                "/api/Pacientes",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var json =
            await response.Content
                .ReadAsStringAsync();

        Assert.DoesNotContain(
            "\"clave\"",
            json,
            StringComparison.OrdinalIgnoreCase);

        var created =
            await response.Content
                .ReadFromJsonAsync<PacienteDto>();

        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal(
            request.Nombre,
            created.Nombre);
        Assert.Equal(
            request.Usuario,
            created.Usuario);

        var getResponse =
            await _client.GetAsync(
                $"/api/Pacientes/{created.Id}");

        Assert.Equal(
            HttpStatusCode.OK,
            getResponse.StatusCode);
    }

    [Fact]
    public async Task UpdatePaciente_BlankPassword_ReturnsNoContentAndPreservesStoredPassword()
    {
        await _factory.ResetDatabaseAsync();

        var created =
            await CreatePacienteAsync(
                "paciente.update",
                "NSS-INT-002");

        var update = new PacienteDto
        {
            Id = created.Id,
            Nombre = "Nombre actualizado",
            Apellidos = created.Apellidos,
            Usuario = created.Usuario,
            Clave = null,
            NSS = created.NSS,
            NumTarjeta = created.NumTarjeta,
            Telefono = created.Telefono,
            Direccion = created.Direccion,
            MedicoIds = []
        };

        var response =
            await _client.PutAsJsonAsync(
                $"/api/Pacientes/{created.Id}",
                update);

        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);

        using var scope =
            _factory.Services.CreateScope();

        var context =
            scope.ServiceProvider
                .GetRequiredService<CitasMedicasDbContext>();

        var stored =
            await context.Set<Paciente>()
                .FindAsync(created.Id);

        Assert.NotNull(stored);
        Assert.Equal(
            "clave-secreta",
            stored.Clave);
        Assert.Equal(
            "Nombre actualizado",
            stored.Nombre);
    }

    [Fact]
    public async Task DeletePaciente_ExistingPatient_ReturnsNoContentAndThenNotFound()
    {
        await _factory.ResetDatabaseAsync();

        var created =
            await CreatePacienteAsync(
                "paciente.delete",
                "NSS-INT-003");

        var deleteResponse =
            await _client.DeleteAsync(
                $"/api/Pacientes/{created.Id}");

        Assert.Equal(
            HttpStatusCode.NoContent,
            deleteResponse.StatusCode);

        var getResponse =
            await _client.GetAsync(
                $"/api/Pacientes/{created.Id}");

        Assert.Equal(
            HttpStatusCode.NotFound,
            getResponse.StatusCode);
    }

    [Fact]
    public async Task CreateCita_UnknownPatient_ReturnsBadRequestFromGlobalExceptionHandler()
    {
        await _factory.ResetDatabaseAsync();

        var request = new CitaDto
        {
            FechaHora =
                DateTime.Now.AddDays(1),
            MotivoCita =
                "Prueba integración",
            PacienteId = 99999,
            MedicoId = 1,
            DiagnosticoId = null
        };

        var response =
            await _client.PostAsJsonAsync(
                "/api/Citas",
                request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var problem =
            await response.Content
                .ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(
            400,
            problem.Status);
        Assert.Equal(
            "Operación no válida",
            problem.Title);
        Assert.Contains(
            "paciente",
            problem.Detail ?? string.Empty,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CreateCita_ValidPatientAndDoctor_ReturnsCreated()
    {
        await _factory.ResetDatabaseAsync();

        var paciente =
            await CreatePacienteAsync(
                "paciente.cita",
                "NSS-INT-004");

        var medico =
            await CreateMedicoAsync(
                "medico.cita",
                "COL-INT-001");

        var request = new CitaDto
        {
            FechaHora =
                DateTime.Now.AddDays(2),
            MotivoCita =
                "Consulta integración",
            PacienteId =
                paciente.Id,
            MedicoId =
                medico.Id,
            DiagnosticoId = null
        };

        var response =
            await _client.PostAsJsonAsync(
                "/api/Citas",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var created =
            await response.Content
                .ReadFromJsonAsync<CitaDto>();

        Assert.NotNull(created);
        Assert.True(created.Id > 0);
        Assert.Equal(
            paciente.Id,
            created.PacienteId);
        Assert.Equal(
            medico.Id,
            created.MedicoId);

        var getResponse =
            await _client.GetAsync(
                $"/api/Citas/{created.Id}");

        Assert.Equal(
            HttpStatusCode.OK,
            getResponse.StatusCode);
    }

    [Fact]
    public async Task DeletePacienteThroughUsuarios_ReturnsBadRequest()
    {
        await _factory.ResetDatabaseAsync();

        var paciente =
            await CreatePacienteAsync(
                "paciente.usuario",
                "NSS-INT-005");

        var response =
            await _client.DeleteAsync(
                $"/api/Usuarios/{paciente.Id}");

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);

        var problem =
            await response.Content
                .ReadFromJsonAsync<ProblemDetails>();

        Assert.NotNull(problem);
        Assert.Equal(
            400,
            problem.Status);
        Assert.Contains(
            "pacientes",
            problem.Detail ?? string.Empty,
            StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task CreatePaciente_WithExistingDoctor_PersistsRelationship()
    {
        await _factory.ResetDatabaseAsync();

        var medico =
            await CreateMedicoAsync(
                "medico.relacion",
                "COL-INT-002");

        var request =
            NewPaciente(
                "paciente.relacion",
                "NSS-INT-006");

        request.MedicoIds = [medico.Id];

        var response =
            await _client.PostAsJsonAsync(
                "/api/Pacientes",
                request);

        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);

        var created =
            await response.Content
                .ReadFromJsonAsync<PacienteDto>();

        Assert.NotNull(created);
        Assert.Contains(
            medico.Id,
            created.MedicoIds);

        var stored =
            await _client.GetFromJsonAsync<PacienteDto>(
                $"/api/Pacientes/{created.Id}");

        Assert.NotNull(stored);
        Assert.Contains(
            medico.Id,
            stored.MedicoIds);
    }

    private async Task<PacienteDto>
        CreatePacienteAsync(
            string usuario,
            string nss)
    {
        var response =
            await _client.PostAsJsonAsync(
                "/api/Pacientes",
                NewPaciente(
                    usuario,
                    nss));

        response.EnsureSuccessStatusCode();

        return
            await response.Content
                .ReadFromJsonAsync<PacienteDto>()
            ?? throw new InvalidOperationException(
                "No se pudo leer el paciente creado.");
    }

    private async Task<MedicoDto>
        CreateMedicoAsync(
            string usuario,
            string numColegiado)
    {
        var request = new MedicoDto
        {
            Nombre = "Médico",
            Apellidos = "Integración",
            Usuario = usuario,
            Clave = "clave-medico",
            NumColegiado = numColegiado,
            PacienteIds = []
        };

        var response =
            await _client.PostAsJsonAsync(
                "/api/Medicos",
                request);

        response.EnsureSuccessStatusCode();

        return
            await response.Content
                .ReadFromJsonAsync<MedicoDto>()
            ?? throw new InvalidOperationException(
                "No se pudo leer el médico creado.");
    }

    private static PacienteDto
        NewPaciente(
            string usuario,
            string nss)
    {
        return new PacienteDto
        {
            Nombre = "Paciente",
            Apellidos = "Integración",
            Usuario = usuario,
            Clave = "clave-secreta",
            NSS = nss,
            NumTarjeta =
                $"TAR-{nss}",
            Telefono =
                "600000000",
            Direccion =
                "Granada",
            MedicoIds = []
        };
    }
}
