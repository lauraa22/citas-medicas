using CitasMedicas.Application.DTOs.Pacientes;

namespace CitasMedicas.Application.Interfaces.Services;

public interface IPacienteService
{
    Task<IEnumerable<PacienteDto>> GetAllAsync();

    Task<PacienteDto?> GetByIdAsync(int id);

    Task<PacienteDto> CreateAsync(PacienteCreateDto dto);

    Task<bool> UpdateAsync(int id, PacienteUpdateDto dto);

    Task<bool> DeleteAsync(int id);
}