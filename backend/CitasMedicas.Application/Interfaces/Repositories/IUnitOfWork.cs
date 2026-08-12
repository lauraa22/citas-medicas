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
    /// Obtiene el repositorio de usuarios.
    /// </summary>
    IGenericRepository<Usuario> Usuarios { get; }

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

    Task<int> SaveChangesAsync();

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}