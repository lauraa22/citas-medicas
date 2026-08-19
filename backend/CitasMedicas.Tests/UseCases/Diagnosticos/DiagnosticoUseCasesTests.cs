using Xunit;

using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Application.UseCases.Diagnosticos;
using CitasMedicas.Domain.Entities;
using Moq;

namespace CitasMedicas.Tests.UseCases.Diagnosticos;

public class DiagnosticoUseCasesTests
{
    [Fact]
    public async Task GetDiagnostico_Existing_ReturnsModel()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out _, out var repo);
        var mapper = TestMocks.CreateMapper();
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Diagnostico { Id = 1, Enfermedad = "Gripe" });

        var result = await new GetDiagnosticoUseCase(uow.Object, mapper.Object).ExecuteAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Gripe", result.Enfermedad);
    }

    [Fact]
    public async Task GetDiagnostico_Missing_ReturnsNull()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out _, out var repo);
        var mapper = TestMocks.CreateMapper();
        repo.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Diagnostico?)null);

        Assert.Null(await new GetDiagnosticoUseCase(uow.Object, mapper.Object).ExecuteAsync(99));
    }

    [Fact]
    public async Task GetDiagnosticos_ReturnsAll()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out _, out var repo);
        var mapper = new Mock<IMapper>();
        var entities = new[] { new Diagnostico { Id = 1 }, new Diagnostico { Id = 2 } };
        var models = new[] { new DiagnosticoModel { Id = 1 }, new DiagnosticoModel { Id = 2 } };
        repo.Setup(x => x.GetAllAsync()).ReturnsAsync(entities);
        mapper.Setup(x => x.Map<IEnumerable<DiagnosticoModel>>(entities)).Returns(models);

        var result = await new GetDiagnosticosUseCase(uow.Object, mapper.Object).ExecuteAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateDiagnostico_AddsAndSaves()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out _, out var repo);
        var mapper = TestMocks.CreateMapper();

        var result = await new CreateDiagnosticoUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(new DiagnosticoModel { Enfermedad = "Alergia", ValoracionEspecialista = "Leve" });

        repo.Verify(x => x.AddAsync(It.IsAny<Diagnostico>()), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        Assert.Equal("Alergia", result.Enfermedad);
    }

    [Fact]
    public async Task UpdateDiagnostico_Missing_ReturnsFalse()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out _, out var repo);
        var mapper = TestMocks.CreateMapper();
        repo.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Diagnostico?)null);

        Assert.False(await new UpdateDiagnosticoUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(99, new DiagnosticoModel()));
    }

    [Fact]
    public async Task UpdateDiagnostico_Existing_UpdatesAndSaves()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out _, out var repo);
        var mapper = TestMocks.CreateMapper();
        var entity = new Diagnostico { Id = 1 };
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(entity);

        var result = await new UpdateDiagnosticoUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(1, new DiagnosticoModel { Enfermedad = "Migraña" });

        Assert.True(result);
        Assert.Equal("Migraña", entity.Enfermedad);
        repo.Verify(x => x.Update(entity), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteDiagnostico_Missing_ReturnsFalse()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out _, out var repo);
        repo.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Diagnostico?)null);

        Assert.False(await new DeleteDiagnosticoUseCase(uow.Object).ExecuteAsync(99));
    }

    [Fact]
    public async Task DeleteDiagnostico_Existing_DeletesAndSaves()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out _, out var repo);
        var entity = new Diagnostico { Id = 1 };
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(entity);

        Assert.True(await new DeleteDiagnosticoUseCase(uow.Object).ExecuteAsync(1));
        repo.Verify(x => x.Delete(entity), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
    }
}
