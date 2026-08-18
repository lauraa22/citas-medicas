using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Domain.Interfaces.Repositories;

public interface IPacienteRepository : IGenericRepository<Paciente>
{
    Task<Paciente?> GetByIdWithMedicosAsync(int id);
    Task<IEnumerable<Paciente>> GetAllWithMedicosAsync();
}