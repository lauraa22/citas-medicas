using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IPacienteRepository Pacientes { get; }

    IMedicoRepository Medicos { get; }

    IGenericRepository<Cita> Citas { get; }

    IGenericRepository<Diagnostico> Diagnosticos { get; }

    Task<int> SaveChangesAsync();

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}