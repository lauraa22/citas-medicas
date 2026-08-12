using CitasMedicas.Application.DTOs.Diagnosticos;

namespace CitasMedicas.Application.Interfaces.Services;

public interface IDiagnosticoService
{
    Task<IEnumerable<DiagnosticoDto>> GetAllAsync();

    Task<DiagnosticoDto?> GetByIdAsync(int id);

    Task<DiagnosticoDto> CreateAsync(DiagnosticoCreateDto dto);

    Task<bool> UpdateAsync(int id, DiagnosticoUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}