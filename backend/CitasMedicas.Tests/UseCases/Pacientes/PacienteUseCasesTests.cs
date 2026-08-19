using Xunit;

using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Application.UseCases.Pacientes;
using CitasMedicas.Domain.Entities;
using Moq;

namespace CitasMedicas.Tests.UseCases.Pacientes;

public class PacienteUseCasesTests
{
    [Fact]
    public async Task GetPaciente_Existing_ReturnsWithDoctorIds()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var repo, out _, out _, out _);
        var mapper = TestMocks.CreateMapper();
        var entity = new Paciente { Id = 1 };
        entity.Medicos.Add(new Medico { Id = 10 });
        repo.Setup(x => x.GetByIdWithMedicosAsync(1)).ReturnsAsync(entity);

        var result = await new GetPacienteUseCase(uow.Object, mapper.Object).ExecuteAsync(1);

        Assert.NotNull(result);
        Assert.Contains(10, result.MedicoIds);
    }

    [Fact]
    public async Task GetPaciente_Missing_ReturnsNull()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var repo, out _, out _, out _);
        var mapper = TestMocks.CreateMapper();
        repo.Setup(x => x.GetByIdWithMedicosAsync(99)).ReturnsAsync((Paciente?)null);

        Assert.Null(await new GetPacienteUseCase(uow.Object, mapper.Object).ExecuteAsync(99));
    }

    [Fact]
    public async Task GetPacientes_ReturnsAll()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var repo, out _, out _, out _);
        var mapper = new Mock<IMapper>();
        var entities = new[] { new Paciente { Id = 1 }, new Paciente { Id = 2 } };
        var models = new[] { new PacienteModel { Id = 1 }, new PacienteModel { Id = 2 } };
        repo.Setup(x => x.GetAllWithMedicosAsync()).ReturnsAsync(entities);
        mapper.Setup(x => x.Map<IEnumerable<PacienteModel>>(entities)).Returns(models);

        Assert.Equal(2, (await new GetPacientesUseCase(uow.Object, mapper.Object).ExecuteAsync()).Count());
    }

    [Fact]
    public async Task CreatePaciente_ValidDoctors_CommitsTransaction()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out var medicos, out _, out _);
        var mapper = TestMocks.CreateMapper();
        medicos.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Medico { Id = 2 });

        var result = await new CreatePacienteUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(new PacienteModel { Nombre = "Ana", MedicoIds = [2] });

        Assert.Equal("Ana", result.Nombre);
        pacientes.Verify(x => x.AddAsync(It.Is<Paciente>(p => p.Medicos.Any(m => m.Id == 2))), Times.Once);
        uow.Verify(x => x.BeginTransactionAsync(), Times.Once);
        uow.Verify(x => x.CommitTransactionAsync(), Times.Once);
        uow.Verify(x => x.RollbackTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task CreatePaciente_MissingDoctor_RollsBackAndThrows()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out var medicos, out _, out _);
        var mapper = TestMocks.CreateMapper();
        medicos.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Medico?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new CreatePacienteUseCase(uow.Object, mapper.Object)
                .ExecuteAsync(new PacienteModel { MedicoIds = [99] }));

        uow.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        uow.Verify(x => x.CommitTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdatePaciente_Missing_RollsBackAndReturnsFalse()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out _, out _, out _);
        var mapper = TestMocks.CreateMapper();
        pacientes.Setup(x => x.GetByIdWithMedicosAsync(99)).ReturnsAsync((Paciente?)null);

        var result = await new UpdatePacienteUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(99, new PacienteModel());

        Assert.False(result);
        uow.Verify(x => x.RollbackTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePaciente_BlankPassword_PreservesPasswordAndCommits()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out _, out _, out _);
        var mapper = TestMocks.CreateMapper();
        var entity = new Paciente { Id = 1, Clave = "secreta" };
        pacientes.Setup(x => x.GetByIdWithMedicosAsync(1)).ReturnsAsync(entity);

        var result = await new UpdatePacienteUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(1, new PacienteModel { Nombre = "Nuevo", Clave = "", MedicoIds = [] });

        Assert.True(result);
        Assert.Equal("secreta", entity.Clave);
        Assert.Equal("Nuevo", entity.Nombre);
        uow.Verify(x => x.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatePaciente_MissingDoctor_RollsBackAndThrows()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out var medicos, out _, out _);
        var mapper = TestMocks.CreateMapper();
        pacientes.Setup(x => x.GetByIdWithMedicosAsync(1)).ReturnsAsync(new Paciente { Id = 1, Clave = "x" });
        medicos.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Medico?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new UpdatePacienteUseCase(uow.Object, mapper.Object)
                .ExecuteAsync(1, new PacienteModel { MedicoIds = [99] }));

        uow.Verify(x => x.RollbackTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task DeletePaciente_Existing_DeletesAndSaves()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out _, out _, out _);
        var entity = new Paciente { Id = 1 };
        pacientes.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(entity);

        Assert.True(await new DeletePacienteUseCase(uow.Object).ExecuteAsync(1));
        pacientes.Verify(x => x.Delete(entity), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeletePaciente_Missing_ReturnsFalse()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out _, out _, out _);
        pacientes.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Paciente?)null);

        Assert.False(await new DeletePacienteUseCase(uow.Object).ExecuteAsync(99));
    }
}
