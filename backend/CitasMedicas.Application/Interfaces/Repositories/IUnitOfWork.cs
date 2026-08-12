using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Application.Interfaces.Repositories;

/// <summary>
/// Define una unidad de trabajo encargada de coordinar los distintos
/// repositorios de la aplicación, persistir los cambios pendientes
/// y gestionar transacciones sobre la base de datos.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Obtiene el repositorio específico de pacientes.
    /// </summary>
    IPacienteRepository Pacientes { get; }

    /// <summary>
    /// Obtiene el repositorio específico de médicos.
    /// </summary>
    IMedicoRepository Medicos { get; }

    /// <summary>
    /// Obtiene el repositorio de citas.
    /// </summary>
    IGenericRepository<Cita> Citas { get; }

    /// <summary>
    /// Obtiene el repositorio de diagnósticos.
    /// </summary>
    IGenericRepository<Diagnostico> Diagnosticos { get; }

    /// <summary>
    /// Guarda en la base de datos todos los cambios pendientes
    /// realizados a través de los repositorios.
    /// </summary>
    /// <returns>
    /// Número de registros afectados por la operación.
    /// </returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Inicia una nueva transacción en la base de datos.
    /// </summary>
    Task BeginTransactionAsync();

    /// <summary>
    /// Confirma la transacción activa y persiste definitivamente
    /// las operaciones realizadas dentro de ella.
    /// </summary>
    Task CommitTransactionAsync();

    /// <summary>
    /// Revierte la transacción activa, deshaciendo las operaciones
    /// realizadas desde el inicio de la transacción.
    /// </summary>
    Task RollbackTransactionAsync();
}