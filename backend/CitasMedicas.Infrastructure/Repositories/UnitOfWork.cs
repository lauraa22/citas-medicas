using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace CitasMedicas.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly CitasMedicasDbContext _context;
    private IDbContextTransaction? _transaction;

    public IPacienteRepository Pacientes { get; }

    public IMedicoRepository Medicos { get; }

    public IGenericRepository<Cita> Citas { get; }

    public IGenericRepository<Diagnostico> Diagnosticos { get; }

    public UnitOfWork(CitasMedicasDbContext context)
    {
        _context = context;

        Pacientes = new PacienteRepository(context);

        Medicos = new MedicoRepository(context);

        Citas = new GenericRepository<Cita>(context);

        Diagnosticos = new GenericRepository<Diagnostico>(context);
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task BeginTransactionAsync()
    {
        _transaction =
            await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_transaction is null)
            return;

        await _transaction.CommitAsync();

        await _transaction.DisposeAsync();

        _transaction = null;
    }

    public async Task RollbackTransactionAsync()
    {
        if (_transaction is null)
            return;

        await _transaction.RollbackAsync();

        await _transaction.DisposeAsync();

        _transaction = null;
    }
}