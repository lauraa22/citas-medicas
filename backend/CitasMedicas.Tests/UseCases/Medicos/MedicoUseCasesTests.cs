using Xunit;

using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Application.UseCases.Medicos;
using CitasMedicas.Domain.Entities;
using Moq;

namespace CitasMedicas.Tests.UseCases.Medicos;

public class MedicoUseCasesTests
{
    [Fact]
    public async Task GetMedico_Existing_ReturnsWithPatientIds()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out var repo, out _, out _);
        var mapper = TestMocks.CreateMapper();
        var entity = new Medico { Id = 1 };
        entity.Pacientes.Add(new Paciente { Id = 10 });
        repo.Setup(x => x.GetByIdWithPacientesAsync(1)).ReturnsAsync(entity);

        var result = await new GetMedicoUseCase(uow.Object, mapper.Object).ExecuteAsync(1);

        Assert.NotNull(result);
        Assert.Contains(10, result.PacienteIds);
    }

    [Fact]
    public async Task GetMedico_Missing_ReturnsNull()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out var repo, out _, out _);
        var mapper = TestMocks.CreateMapper();
        repo.Setup(x => x.GetByIdWithPacientesAsync(99)).ReturnsAsync((Medico?)null);

        Assert.Null(await new GetMedicoUseCase(uow.Object, mapper.Object).ExecuteAsync(99));
    }

    [Fact]
    public async Task GetMedicos_ReturnsAll()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out var repo, out _, out _);
        var mapper = new Mock<IMapper>();
        var entities = new[] { new Medico { Id = 1 }, new Medico { Id = 2 } };
        var models = new[] { new MedicoModel { Id = 1 }, new MedicoModel { Id = 2 } };
        repo.Setup(x => x.GetAllWithPacientesAsync()).ReturnsAsync(entities);
        mapper.Setup(x => x.Map<IEnumerable<MedicoModel>>(entities)).Returns(models);

        Assert.Equal(2, (await new GetMedicosUseCase(uow.Object, mapper.Object).ExecuteAsync()).Count());
    }

    [Fact]
    public async Task CreateMedico_ValidPatients_CommitsTransaction()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out var medicos, out _, out _);
        var mapper = TestMocks.CreateMapper();
        pacientes.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Paciente { Id = 2 });

        var result = await new CreateMedicoUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(new MedicoModel { Nombre = "Eva", PacienteIds = [2] });

        Assert.Equal("Eva", result.Nombre);
        medicos.Verify(x => x.AddAsync(It.Is<Medico>(m => m.Pacientes.Any(p => p.Id == 2))), Times.Once);
        uow.Verify(x => x.BeginTransactionAsync(), Times.Once);
        uow.Verify(x => x.CommitTransactionAsync(), Times.Once);
        uow.Verify(x => x.RollbackTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateMedico_MissingPatient_RollsBackAndThrows()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out _, out _, out _);
        var mapper = TestMocks.CreateMapper();
        pacientes.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Paciente?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new CreateMedicoUseCase(uow.Object, mapper.Object)
                .ExecuteAsync(new MedicoModel { PacienteIds = [99] }));

        uow.Verify(x => x.RollbackTransactionAsync(), Times.Once);
        uow.Verify(x => x.CommitTransactionAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateMedico_Missing_RollsBackAndReturnsFalse()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out var medicos, out _, out _);
        var mapper = TestMocks.CreateMapper();
        medicos.Setup(x => x.GetByIdWithPacientesAsync(99)).ReturnsAsync((Medico?)null);

        var result = await new UpdateMedicoUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(99, new MedicoModel());

        Assert.False(result);
        uow.Verify(x => x.RollbackTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateMedico_BlankPassword_PreservesPasswordAndCommits()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out var medicos, out _, out _);
        var mapper = TestMocks.CreateMapper();
        var entity = new Medico { Id = 1, Clave = "secreta" };
        medicos.Setup(x => x.GetByIdWithPacientesAsync(1)).ReturnsAsync(entity);

        var result = await new UpdateMedicoUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(1, new MedicoModel { Nombre = "Nuevo", Clave = "", PacienteIds = [] });

        Assert.True(result);
        Assert.Equal("secreta", entity.Clave);
        Assert.Equal("Nuevo", entity.Nombre);
        uow.Verify(x => x.CommitTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdateMedico_MissingPatient_RollsBackAndThrows()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out var medicos, out _, out _);
        var mapper = TestMocks.CreateMapper();
        medicos.Setup(x => x.GetByIdWithPacientesAsync(1)).ReturnsAsync(new Medico { Id = 1, Clave = "x" });
        pacientes.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Paciente?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new UpdateMedicoUseCase(uow.Object, mapper.Object)
                .ExecuteAsync(1, new MedicoModel { PacienteIds = [99] }));

        uow.Verify(x => x.RollbackTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteMedico_Existing_DeletesAndSaves()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out var medicos, out _, out _);
        var entity = new Medico { Id = 1 };
        medicos.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(entity);

        Assert.True(await new DeleteMedicoUseCase(uow.Object).ExecuteAsync(1));
        medicos.Verify(x => x.Delete(entity), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteMedico_Missing_ReturnsFalse()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out var medicos, out _, out _);
        medicos.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Medico?)null);

        Assert.False(await new DeleteMedicoUseCase(uow.Object).ExecuteAsync(99));
    }
}
