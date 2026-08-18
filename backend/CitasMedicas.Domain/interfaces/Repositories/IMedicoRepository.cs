using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Domain.Interfaces.Repositories;

public interface IMedicoRepository : IGenericRepository<Medico>
{
    Task<Medico?> GetByIdWithPacientesAsync(int id);
    Task<IEnumerable<Medico>> GetAllWithPacientesAsync();
}