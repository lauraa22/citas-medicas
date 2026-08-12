using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Infrastructure.Persistence;

namespace CitasMedicas.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CitasMedicasDbContext _context;

    public IGenericRepository<Paciente> Pacientes { get; }
    public IGenericRepository<Medico> Medicos { get; }
    public IGenericRepository<Cita> Citas { get; }
    public IGenericRepository<Diagnostico> Diagnosticos { get; }

    public UnitOfWork(CitasMedicasDbContext context)
    {
        _context = context;

        Pacientes = new GenericRepository<Paciente>(context);
        Medicos = new GenericRepository<Medico>(context);
        Citas = new GenericRepository<Cita>(context);
        Diagnosticos = new GenericRepository<Diagnostico>(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}