using CitasMedicas.Application.DTOs.Medicos;

namespace CitasMedicas.Application.Interfaces.Services;

/// <summary>
/// Define las operaciones disponibles para la gestión de médicos.
/// </summary>
public interface IMedicoService
{
    Task<IEnumerable<MedicoDto>> GetAllAsync();

    Task<MedicoDto?> GetByIdAsync(int id);

    Task<MedicoDto> CreateAsync(MedicoCreateDto dto);

    Task<bool> UpdateAsync(int id, MedicoUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}