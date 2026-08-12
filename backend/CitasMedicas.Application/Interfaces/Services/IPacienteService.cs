using CitasMedicas.Application.DTOs.Pacientes;

namespace CitasMedicas.Application.Interfaces.Services;

/// <summary>
/// Define las operaciones disponibles para la gestión de pacientes.
/// </summary>
public interface IPacienteService
{
    /// <summary>
    /// Obtiene todos los pacientes.
    /// </summary>
    Task<IEnumerable<PacienteDto>> GetAllAsync();

    /// <summary>
    /// Obtiene un paciente por su identificador.
    /// </summary>
    Task<PacienteDto?> GetByIdAsync(int id);

    /// <summary>
    /// Crea un nuevo paciente.
    /// </summary>
    Task<PacienteDto> CreateAsync(PacienteCreateDto dto);

    /// <summary>
    /// Actualiza un paciente existente.
    /// </summary>
    Task<bool> UpdateAsync(int id, PacienteUpdateDto dto);

    /// <summary>
    /// Elimina un paciente.
    /// </summary>
    Task<bool> DeleteAsync(int id);
}