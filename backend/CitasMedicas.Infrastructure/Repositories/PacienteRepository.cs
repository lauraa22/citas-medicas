using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CitasMedicas.Infrastructure.Repositories;

public class PacienteRepository
    : GenericRepository<Paciente>, IPacienteRepository
{
    public PacienteRepository(CitasMedicasDbContext context)
        : base(context)
    {
    }

    public async Task<Paciente?> GetByIdWithMedicosAsync(int id)
    {
        return await _context.Pacientes
            .Include(p => p.Medicos)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Paciente>> GetAllWithMedicosAsync()
    {
        return await _context.Pacientes
            .Include(p => p.Medicos)
            .ToListAsync();
    }
}