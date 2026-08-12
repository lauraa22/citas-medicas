using CitasMedicas.Application.DTOs.Diagnosticos;

namespace CitasMedicas.Application.Interfaces.Services;

/// <summary>
/// Define las operaciones disponibles para la gestión de diagnósticos.
/// </summary>
public interface IDiagnosticoService
{
    Task<IEnumerable<DiagnosticoDto>> GetAllAsync();

    Task<DiagnosticoDto?> GetByIdAsync(int id);

    Task<DiagnosticoDto> CreateAsync(DiagnosticoCreateDto dto);

    Task<bool> UpdateAsync(int id, DiagnosticoUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}