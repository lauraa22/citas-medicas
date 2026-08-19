using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces.Repositories;
using Moq;

namespace CitasMedicas.Tests;

internal static class TestMocks
{
    public static Mock<IUnitOfWork> CreateUnitOfWork(
        out Mock<IGenericRepository<Usuario>> usuarios,
        out Mock<IPacienteRepository> pacientes,
        out Mock<IMedicoRepository> medicos,
        out Mock<IGenericRepository<Cita>> citas,
        out Mock<IGenericRepository<Diagnostico>> diagnosticos)
    {
        usuarios = new Mock<IGenericRepository<Usuario>>();
        pacientes = new Mock<IPacienteRepository>();
        medicos = new Mock<IMedicoRepository>();
        citas = new Mock<IGenericRepository<Cita>>();
        diagnosticos = new Mock<IGenericRepository<Diagnostico>>();

        var uow = new Mock<IUnitOfWork>();
        uow.SetupGet(x => x.Usuarios).Returns(usuarios.Object);
        uow.SetupGet(x => x.Pacientes).Returns(pacientes.Object);
        uow.SetupGet(x => x.Medicos).Returns(medicos.Object);
        uow.SetupGet(x => x.Citas).Returns(citas.Object);
        uow.SetupGet(x => x.Diagnosticos).Returns(diagnosticos.Object);

        return uow;
    }

    public static Mock<IMapper> CreateMapper()
    {
        var mapper = new Mock<IMapper>();

        mapper.Setup(x => x.Map<Cita>(It.IsAny<CitaModel>()))
            .Returns((CitaModel m) => new Cita
            {
                Id = m.Id,
                FechaHora = m.FechaHora,
                MotivoCita = m.MotivoCita,
                PacienteId = m.PacienteId,
                MedicoId = m.MedicoId,
                DiagnosticoId = m.DiagnosticoId
            });

        mapper.Setup(x => x.Map<CitaModel>(It.IsAny<Cita>()))
            .Returns((Cita e) => new CitaModel
            {
                Id = e.Id,
                FechaHora = e.FechaHora,
                MotivoCita = e.MotivoCita,
                PacienteId = e.PacienteId,
                MedicoId = e.MedicoId,
                DiagnosticoId = e.DiagnosticoId
            });

        mapper.Setup(x => x.Map<Diagnostico>(It.IsAny<DiagnosticoModel>()))
            .Returns((DiagnosticoModel m) => new Diagnostico
            {
                Id = m.Id,
                ValoracionEspecialista = m.ValoracionEspecialista,
                Enfermedad = m.Enfermedad
            });

        mapper.Setup(x => x.Map<DiagnosticoModel>(It.IsAny<Diagnostico>()))
            .Returns((Diagnostico e) => new DiagnosticoModel
            {
                Id = e.Id,
                ValoracionEspecialista = e.ValoracionEspecialista,
                Enfermedad = e.Enfermedad
            });

        mapper.Setup(x => x.Map<Usuario>(It.IsAny<UsuarioModel>()))
            .Returns((UsuarioModel m) => new Usuario
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Apellidos = m.Apellidos,
                NombreUsuario = m.Usuario,
                Clave = m.Clave ?? string.Empty
            });

        mapper.Setup(x => x.Map<UsuarioModel>(It.IsAny<Usuario>()))
            .Returns((Usuario e) => new UsuarioModel
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Apellidos = e.Apellidos,
                Usuario = e.NombreUsuario,
                Clave = null
            });

        mapper.Setup(x => x.Map<Paciente>(It.IsAny<PacienteModel>()))
            .Returns((PacienteModel m) => new Paciente
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Apellidos = m.Apellidos,
                NombreUsuario = m.Usuario,
                Clave = m.Clave ?? string.Empty,
                NSS = m.NSS,
                NumTarjeta = m.NumTarjeta,
                Telefono = m.Telefono,
                Direccion = m.Direccion
            });

        mapper.Setup(x => x.Map<PacienteModel>(It.IsAny<Paciente>()))
            .Returns((Paciente e) => new PacienteModel
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Apellidos = e.Apellidos,
                Usuario = e.NombreUsuario,
                Clave = null,
                NSS = e.NSS,
                NumTarjeta = e.NumTarjeta,
                Telefono = e.Telefono,
                Direccion = e.Direccion,
                MedicoIds = e.Medicos.Select(x => x.Id).ToList()
            });

        mapper.Setup(x => x.Map<Medico>(It.IsAny<MedicoModel>()))
            .Returns((MedicoModel m) => new Medico
            {
                Id = m.Id,
                Nombre = m.Nombre,
                Apellidos = m.Apellidos,
                NombreUsuario = m.Usuario,
                Clave = m.Clave ?? string.Empty,
                NumColegiado = m.NumColegiado
            });

        mapper.Setup(x => x.Map<MedicoModel>(It.IsAny<Medico>()))
            .Returns((Medico e) => new MedicoModel
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Apellidos = e.Apellidos,
                Usuario = e.NombreUsuario,
                Clave = null,
                NumColegiado = e.NumColegiado,
                PacienteIds = e.Pacientes.Select(x => x.Id).ToList()
            });

        mapper.Setup(x => x.Map(It.IsAny<CitaModel>(), It.IsAny<Cita>()))
            .Callback((object source, object destination) =>
            {
                var m = (CitaModel)source;
                var e = (Cita)destination;
                e.FechaHora = m.FechaHora;
                e.MotivoCita = m.MotivoCita;
                e.PacienteId = m.PacienteId;
                e.MedicoId = m.MedicoId;
                e.DiagnosticoId = m.DiagnosticoId;
            });

        mapper.Setup(x => x.Map(It.IsAny<DiagnosticoModel>(), It.IsAny<Diagnostico>()))
            .Callback((object source, object destination) =>
            {
                var m = (DiagnosticoModel)source;
                var e = (Diagnostico)destination;
                e.ValoracionEspecialista = m.ValoracionEspecialista;
                e.Enfermedad = m.Enfermedad;
            });

        mapper.Setup(x => x.Map(It.IsAny<PacienteModel>(), It.IsAny<Paciente>()))
            .Callback((object source, object destination) =>
            {
                var m = (PacienteModel)source;
                var e = (Paciente)destination;
                e.Nombre = m.Nombre;
                e.Apellidos = m.Apellidos;
                e.NombreUsuario = m.Usuario;
                e.Clave = m.Clave ?? string.Empty;
                e.NSS = m.NSS;
                e.NumTarjeta = m.NumTarjeta;
                e.Telefono = m.Telefono;
                e.Direccion = m.Direccion;
            });

        mapper.Setup(x => x.Map(It.IsAny<MedicoModel>(), It.IsAny<Medico>()))
            .Callback((object source, object destination) =>
            {
                var m = (MedicoModel)source;
                var e = (Medico)destination;
                e.Nombre = m.Nombre;
                e.Apellidos = m.Apellidos;
                e.NombreUsuario = m.Usuario;
                e.Clave = m.Clave ?? string.Empty;
                e.NumColegiado = m.NumColegiado;
            });

        return mapper;
    }
}
