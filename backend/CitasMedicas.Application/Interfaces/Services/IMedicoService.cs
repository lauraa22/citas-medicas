using CitasMedicas.Application.DTOs.Medicos;

namespace CitasMedicas.Application.Interfaces.Services;

public interface IMedicoService
{
    Task<IEnumerable<MedicoDto>> GetAllAsync();

    Task<MedicoDto?> GetByIdAsync(int id);

    Task<MedicoDto> CreateAsync(MedicoCreateDto dto);

    Task<bool> UpdateAsync(int id, MedicoUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}