using CitasMedicas.Application.DTOs.Citas;

namespace CitasMedicas.Application.Interfaces.Services;

public interface ICitaService
{
    Task<IEnumerable<CitaDto>> GetAllAsync();

    Task<CitaDto?> GetByIdAsync(int id);

    Task<CitaDto> CreateAsync(CitaCreateDto dto);

    Task<bool> UpdateAsync(int id, CitaUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}