using CitasMedicas.Api.DTOs;
using CitasMedicas.Application.Models;

namespace CitasMedicas.Api.Mappings;

/// <summary>
/// Proporciona métodos de extensión para convertir
/// entre los DTOs de la API y los modelos internos de Application.
/// </summary>
public static class DtoMappingExtensions
{
    /// <summary>
    /// Convierte un <see cref="UsuarioDto"/> en un
    /// <see cref="UsuarioModel"/>.
    /// </summary>
    /// <param name="dto">DTO del usuario.</param>
    /// <returns>Modelo interno del usuario.</returns>
    public static UsuarioModel ToModel(
        this UsuarioDto dto)
    {
        return new UsuarioModel
        {
            Id = dto.Id,
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Usuario = dto.Usuario,
            Clave = dto.Clave
        };
    }

    /// <summary>
    /// Convierte un <see cref="UsuarioModel"/> en un
    /// <see cref="UsuarioDto"/>.
    /// </summary>
    /// <param name="model">Modelo interno del usuario.</param>
    /// <returns>DTO del usuario.</returns>
    public static UsuarioDto ToDto(
        this UsuarioModel model)
    {
        return new UsuarioDto
        {
            Id = model.Id,
            Nombre = model.Nombre,
            Apellidos = model.Apellidos,
            Usuario = model.Usuario,
            Clave = null
        };
    }

    /// <summary>
    /// Convierte un <see cref="PacienteDto"/> en un
    /// <see cref="PacienteModel"/>.
    /// </summary>
    /// <param name="dto">DTO del paciente.</param>
    /// <returns>Modelo interno del paciente.</returns>
    public static PacienteModel ToModel(
        this PacienteDto dto)
    {
        return new PacienteModel
        {
            Id = dto.Id,
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Usuario = dto.Usuario,
            Clave = dto.Clave,
            NSS = dto.NSS,
            NumTarjeta = dto.NumTarjeta,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            MedicoIds = dto.MedicoIds
        };
    }

    /// <summary>
    /// Convierte un <see cref="PacienteModel"/> en un
    /// <see cref="PacienteDto"/>.
    /// </summary>
    /// <param name="model">Modelo interno del paciente.</param>
    /// <returns>DTO del paciente.</returns>
    public static PacienteDto ToDto(
        this PacienteModel model)
    {
        return new PacienteDto
        {
            Id = model.Id,
            Nombre = model.Nombre,
            Apellidos = model.Apellidos,
            Usuario = model.Usuario,
            Clave = null,
            NSS = model.NSS,
            NumTarjeta = model.NumTarjeta,
            Telefono = model.Telefono,
            Direccion = model.Direccion,
            MedicoIds = model.MedicoIds
        };
    }

    /// <summary>
    /// Convierte un <see cref="MedicoDto"/> en un
    /// <see cref="MedicoModel"/>.
    /// </summary>
    /// <param name="dto">DTO del médico.</param>
    /// <returns>Modelo interno del médico.</returns>
    public static MedicoModel ToModel(
        this MedicoDto dto)
    {
        return new MedicoModel
        {
            Id = dto.Id,
            Nombre = dto.Nombre,
            Apellidos = dto.Apellidos,
            Usuario = dto.Usuario,
            Clave = dto.Clave,
            NumColegiado = dto.NumColegiado,
            PacienteIds = dto.PacienteIds
        };
    }

    /// <summary>
    /// Convierte un <see cref="MedicoModel"/> en un
    /// <see cref="MedicoDto"/>.
    /// </summary>
    /// <param name="model">Modelo interno del médico.</param>
    /// <returns>DTO del médico.</returns>
    public static MedicoDto ToDto(
        this MedicoModel model)
    {
        return new MedicoDto
        {
            Id = model.Id,
            Nombre = model.Nombre,
            Apellidos = model.Apellidos,
            Usuario = model.Usuario,
            Clave = null,
            NumColegiado = model.NumColegiado,
            PacienteIds = model.PacienteIds
        };
    }

    /// <summary>
    /// Convierte un <see cref="CitaDto"/> en un
    /// <see cref="CitaModel"/>.
    /// </summary>
    /// <param name="dto">DTO de la cita.</param>
    /// <returns>Modelo interno de la cita.</returns>
    public static CitaModel ToModel(
        this CitaDto dto)
    {
        return new CitaModel
        {
            Id = dto.Id,
            FechaHora = dto.FechaHora,
            MotivoCita = dto.MotivoCita,
            PacienteId = dto.PacienteId,
            MedicoId = dto.MedicoId,
            DiagnosticoId = dto.DiagnosticoId
        };
    }

    /// <summary>
    /// Convierte un <see cref="CitaModel"/> en un
    /// <see cref="CitaDto"/>.
    /// </summary>
    /// <param name="model">Modelo interno de la cita.</param>
    /// <returns>DTO de la cita.</returns>
    public static CitaDto ToDto(
        this CitaModel model)
    {
        return new CitaDto
        {
            Id = model.Id,
            FechaHora = model.FechaHora,
            MotivoCita = model.MotivoCita,
            PacienteId = model.PacienteId,
            MedicoId = model.MedicoId,
            DiagnosticoId = model.DiagnosticoId
        };
    }

    /// <summary>
    /// Convierte un <see cref="DiagnosticoDto"/> en un
    /// <see cref="DiagnosticoModel"/>.
    /// </summary>
    /// <param name="dto">DTO del diagnóstico.</param>
    /// <returns>Modelo interno del diagnóstico.</returns>
    public static DiagnosticoModel ToModel(
        this DiagnosticoDto dto)
    {
        return new DiagnosticoModel
        {
            Id = dto.Id,
            ValoracionEspecialista =
                dto.ValoracionEspecialista,
            Enfermedad = dto.Enfermedad
        };
    }

    /// <summary>
    /// Convierte un <see cref="DiagnosticoModel"/> en un
    /// <see cref="DiagnosticoDto"/>.
    /// </summary>
    /// <param name="model">Modelo interno del diagnóstico.</param>
    /// <returns>DTO del diagnóstico.</returns>
    public static DiagnosticoDto ToDto(
        this DiagnosticoModel model)
    {
        return new DiagnosticoDto
        {
            Id = model.Id,
            ValoracionEspecialista =
                model.ValoracionEspecialista,
            Enfermedad = model.Enfermedad
        };
    }
}