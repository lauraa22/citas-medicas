using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CitasMedicas.Infrastructure.Repositories;

public class MedicoRepository
    : GenericRepository<Medico>, IMedicoRepository
{
    public MedicoRepository(CitasMedicasDbContext context)
        : base(context)
    {
    }

    public async Task<Medico?> GetByIdWithPacientesAsync(int id)
    {
        return await _context.Medicos
            .Include(m => m.Pacientes)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Medico>> GetAllWithPacientesAsync()
    {
        return await _context.Medicos
            .Include(m => m.Pacientes)
            .ToListAsync();
    }
}