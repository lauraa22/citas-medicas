using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IGenericRepository<Paciente> Pacientes { get; }
    IGenericRepository<Medico> Medicos { get; }
    IGenericRepository<Cita> Citas { get; }
    IGenericRepository<Diagnostico> Diagnosticos { get; }

    Task<int> SaveChangesAsync();
}