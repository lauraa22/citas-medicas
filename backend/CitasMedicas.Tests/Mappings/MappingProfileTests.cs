using AutoMapper;
using CitasMedicas.Application.Mappings;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Entities;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace CitasMedicas.Tests.Mappings;

public class MappingProfileTests
{
    private readonly IMapper _mapper;
    private readonly MapperConfiguration _configuration;

    public MappingProfileTests()
    {
        _configuration = new MapperConfiguration(
            cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            },
            NullLoggerFactory.Instance);

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void MappingProfile_ConfigurationIsValid()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Fact]
    public void Usuario_To_UsuarioModel_MapsCorrectly()
    {
        var usuario = new Usuario
        {
            Id = 1,
            Nombre = "Laura",
            Apellidos = "Guirao",
            NombreUsuario = "laura22",
            Clave = "123456"
        };

        var result =
            _mapper.Map<UsuarioModel>(usuario);

        Assert.Equal(usuario.Id, result.Id);
        Assert.Equal(usuario.Nombre, result.Nombre);
        Assert.Equal(usuario.Apellidos, result.Apellidos);
        Assert.Equal(usuario.NombreUsuario, result.Usuario);

        Assert.Null(result.Clave);
    }

    [Fact]
    public void UsuarioModel_To_Usuario_MapsCorrectly()
    {
        var model = new UsuarioModel
        {
            Id = 99,
            Nombre = "Laura",
            Apellidos = "Guirao",
            Usuario = "laura22",
            Clave = "123456"
        };

        var result =
            _mapper.Map<Usuario>(model);

        Assert.Equal(0, result.Id);
        Assert.Equal(model.Nombre, result.Nombre);
        Assert.Equal(model.Apellidos, result.Apellidos);
        Assert.Equal(model.Usuario, result.NombreUsuario);
        Assert.Equal(model.Clave, result.Clave);
    }

    [Fact]
    public void Paciente_To_PacienteModel_MapsCorrectly()
    {
        var paciente = new Paciente
        {
            Id = 1,
            Nombre = "Laura",
            Apellidos = "Guirao",
            NombreUsuario = "laura22",
            Clave = "123456",
            NSS = "123456789",
            NumTarjeta = "TARJETA1",
            Telefono = "600000000",
            Direccion = "Granada"
        };

        paciente.Medicos.Add(
            new Medico
            {
                Id = 10,
                Nombre = "Doctor 1"
            });

        paciente.Medicos.Add(
            new Medico
            {
                Id = 20,
                Nombre = "Doctor 2"
            });

        var result =
            _mapper.Map<PacienteModel>(paciente);

        Assert.Equal(paciente.Id, result.Id);
        Assert.Equal(paciente.Nombre, result.Nombre);
        Assert.Equal(paciente.Apellidos, result.Apellidos);
        Assert.Equal(paciente.NombreUsuario, result.Usuario);

        Assert.Equal(paciente.NSS, result.NSS);
        Assert.Equal(paciente.NumTarjeta, result.NumTarjeta);
        Assert.Equal(paciente.Telefono, result.Telefono);
        Assert.Equal(paciente.Direccion, result.Direccion);

        Assert.Null(result.Clave);

        Assert.Equal(
            new List<int> { 10, 20 },
            result.MedicoIds);
    }

    [Fact]
    public void PacienteModel_To_Paciente_MapsCorrectly()
    {
        var model = new PacienteModel
        {
            Id = 99,
            Nombre = "Laura",
            Apellidos = "Guirao",
            Usuario = "laura22",
            Clave = "123456",
            NSS = "123456789",
            NumTarjeta = "TARJETA1",
            Telefono = "600000000",
            Direccion = "Granada",
            MedicoIds = new List<int> { 10, 20 }
        };

        var result =
            _mapper.Map<Paciente>(model);

        Assert.Equal(0, result.Id);

        Assert.Equal(model.Nombre, result.Nombre);
        Assert.Equal(model.Apellidos, result.Apellidos);
        Assert.Equal(model.Usuario, result.NombreUsuario);
        Assert.Equal(model.Clave, result.Clave);

        Assert.Equal(model.NSS, result.NSS);
        Assert.Equal(model.NumTarjeta, result.NumTarjeta);
        Assert.Equal(model.Telefono, result.Telefono);
        Assert.Equal(model.Direccion, result.Direccion);

        Assert.Empty(result.Medicos);
        Assert.Empty(result.Citas);
    }

    [Fact]
    public void Medico_To_MedicoModel_MapsCorrectly()
    {
        var medico = new Medico
        {
            Id = 1,
            Nombre = "Ana",
            Apellidos = "Lopez",
            NombreUsuario = "ana.medico",
            Clave = "123456",
            NumColegiado = "COL001"
        };

        medico.Pacientes.Add(
            new Paciente
            {
                Id = 10,
                Nombre = "Paciente 1"
            });

        medico.Pacientes.Add(
            new Paciente
            {
                Id = 20,
                Nombre = "Paciente 2"
            });

        var result =
            _mapper.Map<MedicoModel>(medico);

        Assert.Equal(medico.Id, result.Id);
        Assert.Equal(medico.Nombre, result.Nombre);
        Assert.Equal(medico.Apellidos, result.Apellidos);
        Assert.Equal(medico.NombreUsuario, result.Usuario);
        Assert.Equal(medico.NumColegiado, result.NumColegiado);

        Assert.Null(result.Clave);

        Assert.Equal(
            new List<int> { 10, 20 },
            result.PacienteIds);
    }

    [Fact]
    public void MedicoModel_To_Medico_MapsCorrectly()
    {
        var model = new MedicoModel
        {
            Id = 99,
            Nombre = "Ana",
            Apellidos = "Lopez",
            Usuario = "ana.medico",
            Clave = "123456",
            NumColegiado = "COL001",
            PacienteIds = new List<int> { 10, 20 }
        };

        var result =
            _mapper.Map<Medico>(model);

        Assert.Equal(0, result.Id);
        Assert.Equal(model.Nombre, result.Nombre);
        Assert.Equal(model.Apellidos, result.Apellidos);
        Assert.Equal(model.Usuario, result.NombreUsuario);
        Assert.Equal(model.Clave, result.Clave);
        Assert.Equal(model.NumColegiado, result.NumColegiado);

        Assert.Empty(result.Pacientes);
        Assert.Empty(result.Citas);
    }

    [Fact]
    public void Cita_To_CitaModel_MapsCorrectly()
    {
        var cita = new Cita
        {
            Id = 1,
            FechaHora = new DateTime(
                2026,
                8,
                19,
                12,
                30,
                0),
            MotivoCita = "Revisión",
            PacienteId = 2,
            MedicoId = 3,
            DiagnosticoId = 4
        };

        var result =
            _mapper.Map<CitaModel>(cita);

        Assert.Equal(cita.Id, result.Id);
        Assert.Equal(cita.FechaHora, result.FechaHora);
        Assert.Equal(cita.MotivoCita, result.MotivoCita);
        Assert.Equal(cita.PacienteId, result.PacienteId);
        Assert.Equal(cita.MedicoId, result.MedicoId);
        Assert.Equal(cita.DiagnosticoId, result.DiagnosticoId);
    }

    [Fact]
    public void CitaModel_To_Cita_MapsCorrectly()
    {
        var model = new CitaModel
        {
            Id = 99,
            FechaHora = new DateTime(
                2026,
                8,
                19,
                12,
                30,
                0),
            MotivoCita = "Revisión",
            PacienteId = 2,
            MedicoId = 3,
            DiagnosticoId = 4
        };

        var result =
            _mapper.Map<Cita>(model);

        Assert.Equal(0, result.Id);

        Assert.Equal(model.FechaHora, result.FechaHora);
        Assert.Equal(model.MotivoCita, result.MotivoCita);
        Assert.Equal(model.PacienteId, result.PacienteId);
        Assert.Equal(model.MedicoId, result.MedicoId);
        Assert.Equal(model.DiagnosticoId, result.DiagnosticoId);

        Assert.Null(result.Paciente);
        Assert.Null(result.Medico);
        Assert.Null(result.Diagnostico);
    }

    [Fact]
    public void Diagnostico_To_DiagnosticoModel_MapsCorrectly()
    {
        var diagnostico = new Diagnostico
        {
            Id = 1,
            ValoracionEspecialista = "Paciente estable",
            Enfermedad = "Gripe"
        };

        var result =
            _mapper.Map<DiagnosticoModel>(diagnostico);

        Assert.Equal(diagnostico.Id, result.Id);

        Assert.Equal(
            diagnostico.ValoracionEspecialista,
            result.ValoracionEspecialista);

        Assert.Equal(
            diagnostico.Enfermedad,
            result.Enfermedad);
    }

    [Fact]
    public void DiagnosticoModel_To_Diagnostico_MapsCorrectly()
    {
        var model = new DiagnosticoModel
        {
            Id = 99,
            ValoracionEspecialista = "Paciente estable",
            Enfermedad = "Gripe"
        };

        var result =
            _mapper.Map<Diagnostico>(model);

        Assert.Equal(0, result.Id);

        Assert.Equal(
            model.ValoracionEspecialista,
            result.ValoracionEspecialista);

        Assert.Equal(
            model.Enfermedad,
            result.Enfermedad);

        Assert.Null(result.Cita);
    }
}