using CitasMedicas.Application.Interfaces.Repositories;
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

    /// <summary>
    /// Repositorio específico para la gestión de pacientes.
    /// </summary>
    public IPacienteRepository Pacientes { get; }

    /// <summary>
    /// Repositorio específico para la gestión de médicos.
    /// </summary>
    public IMedicoRepository Medicos { get; }

    /// <summary>
    /// Repositorio para la gestión de citas.
    /// </summary>
    public IGenericRepository<Cita> Citas { get; }

    /// <summary>
    /// Repositorio para la gestión de diagnósticos.
    /// </summary>
    public IGenericRepository<Diagnostico> Diagnosticos { get; }

    /// <summary>
    /// Inicializa una nueva instancia de la unidad de trabajo
    /// utilizando el contexto de Entity Framework Core.
    /// </summary>
    /// <param name="context">
    /// Contexto de base de datos utilizado para acceder y persistir
    /// las entidades de la aplicación.
    /// </param>
    public UnitOfWork(CitasMedicasDbContext context)
    {
        _context = context;

        Pacientes = new PacienteRepository(context);
        Medicos = new MedicoRepository(context);
        Citas = new GenericRepository<Cita>(context);
        Diagnosticos = new GenericRepository<Diagnostico>(context);
    }

    /// <summary>
    /// Guarda en la base de datos todos los cambios pendientes
    /// realizados a través del contexto.
    /// </summary>
    /// <returns>
    /// Número de registros afectados por la operación.
    /// </returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Inicia una nueva transacción en la base de datos.
    /// Las operaciones posteriores podrán confirmarse o revertirse
    /// como una única unidad de trabajo.
    /// </summary>
    public async Task BeginTransactionAsync()
    {
        _transaction =
            await _context.Database.BeginTransactionAsync();
    }

    /// <summary>
    /// Confirma la transacción activa y guarda definitivamente
    /// las operaciones realizadas durante la misma.
    /// </summary>
    public async Task CommitTransactionAsync()
    {
        if (_transaction is null)
            return;

        await _transaction.CommitAsync();

        await _transaction.DisposeAsync();

        _transaction = null;
    }

    /// <summary>
    /// Revierte la transacción activa y deshace las operaciones
    /// realizadas desde que se inició.
    /// </summary>
    public async Task RollbackTransactionAsync()
    {
        if (_transaction is null)
            return;

        await _transaction.RollbackAsync();

        await _transaction.DisposeAsync();

        _transaction = null;
    }
}