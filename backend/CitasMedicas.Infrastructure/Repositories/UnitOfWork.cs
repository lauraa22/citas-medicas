using CitasMedicas.Domain.Interfaces.Repositories;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace CitasMedicas.Infrastructure.Repositories;

/// <summary>
/// Implementación de la unidad de trabajo encargada de coordinar
/// los repositorios de la aplicación, guardar los cambios realizados
/// mediante Entity Framework Core y gestionar las transacciones
/// sobre la base de datos.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly CitasMedicasDbContext _context;
    private IDbContextTransaction? _transaction;

    public IGenericRepository<Usuario> Usuarios { get; }

    public IPacienteRepository Pacientes { get; }

    public IMedicoRepository Medicos { get; }

    public IGenericRepository<Cita> Citas { get; }

    public IGenericRepository<Diagnostico> Diagnosticos { get; }

    public UnitOfWork(CitasMedicasDbContext context)
    {
        _context = context;

        Usuarios =
            new SqlServerGenericRepository<Usuario>(context);

        Pacientes =
            new SqlServerPacienteRepository(context);

        Medicos =
            new SqlServerMedicoRepository(context);

        Citas =
            new SqlServerGenericRepository<Cita>(context);

        Diagnosticos =
            new SqlServerGenericRepository<Diagnostico>(context);
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