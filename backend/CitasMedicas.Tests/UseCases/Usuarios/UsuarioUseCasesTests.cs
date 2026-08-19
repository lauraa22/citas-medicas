using Xunit;

using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Application.UseCases.Usuarios;
using CitasMedicas.Domain.Entities;
using Moq;

namespace CitasMedicas.Tests.UseCases.Usuarios;

public class UsuarioUseCasesTests
{
    [Fact]
    public async Task GetUsuario_Existing_ReturnsModel()
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        var mapper = TestMocks.CreateMapper();
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Usuario { Id = 1, Nombre = "Laura" });

        var result = await new GetUsuarioUseCase(uow.Object, mapper.Object).ExecuteAsync(1);

        Assert.NotNull(result);
        Assert.Equal("Laura", result.Nombre);
    }

    [Fact]
    public async Task GetUsuario_Missing_ReturnsNull()
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        var mapper = TestMocks.CreateMapper();
        repo.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Usuario?)null);

        Assert.Null(await new GetUsuarioUseCase(uow.Object, mapper.Object).ExecuteAsync(99));
    }

    [Fact]
    public async Task GetUsuarios_ReturnsAll()
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        var mapper = new Mock<IMapper>();
        var entities = new[] { new Usuario { Id = 1 }, new Usuario { Id = 2 } };
        var models = new[] { new UsuarioModel { Id = 1 }, new UsuarioModel { Id = 2 } };
        repo.Setup(x => x.GetAllAsync()).ReturnsAsync(entities);
        mapper.Setup(x => x.Map<IEnumerable<UsuarioModel>>(entities)).Returns(models);

        Assert.Equal(2, (await new GetUsuariosUseCase(uow.Object, mapper.Object).ExecuteAsync()).Count());
    }

    [Fact]
    public async Task CreateUsuario_AddsAndSaves()
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        var mapper = TestMocks.CreateMapper();

        var result = await new CreateUsuarioUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(new UsuarioModel { Nombre = "Ana", Usuario = "ana", Clave = "1234" });

        repo.Verify(x => x.AddAsync(It.Is<Usuario>(u => u.NombreUsuario == "ana")), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        Assert.Equal("Ana", result.Nombre);
    }

    [Fact]
    public async Task UpdateUsuario_Missing_ReturnsFalse()
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        repo.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Usuario?)null);

        Assert.False(await new UpdateUsuarioUseCase(uow.Object)
            .ExecuteAsync(99, new UsuarioModel()));
    }

    [Fact]
    public async Task UpdateUsuario_BlankPassword_PreservesPassword()
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        var entity = new Usuario { Id = 1, Nombre = "Old", NombreUsuario = "old", Clave = "secreta" };
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(entity);

        var result = await new UpdateUsuarioUseCase(uow.Object)
            .ExecuteAsync(1, new UsuarioModel { Nombre = "Nuevo", Usuario = "nuevo", Clave = "" });

        Assert.True(result);
        Assert.Equal("secreta", entity.Clave);
        Assert.Equal("Nuevo", entity.Nombre);
        repo.Verify(x => x.Update(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateUsuario_WithPassword_ChangesPassword()
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        var entity = new Usuario { Id = 1, Clave = "old" };
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(entity);

        await new UpdateUsuarioUseCase(uow.Object)
            .ExecuteAsync(1, new UsuarioModel { Clave = "new" });

        Assert.Equal("new", entity.Clave);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task UpdateUsuario_DerivedUser_Throws(bool paciente)
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        Usuario entity = paciente ? new Paciente { Id = 1 } : new Medico { Id = 1 };
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(entity);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new UpdateUsuarioUseCase(uow.Object).ExecuteAsync(1, new UsuarioModel()));
    }

    [Fact]
    public async Task DeleteUsuario_Missing_ReturnsFalse()
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        repo.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Usuario?)null);

        Assert.False(await new DeleteUsuarioUseCase(uow.Object).ExecuteAsync(99));
    }

    [Fact]
    public async Task DeleteUsuario_BaseUser_DeletesAndSaves()
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        var entity = new Usuario { Id = 1 };
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(entity);

        Assert.True(await new DeleteUsuarioUseCase(uow.Object).ExecuteAsync(1));
        repo.Verify(x => x.Delete(entity), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task DeleteUsuario_DerivedUser_Throws(bool paciente)
    {
        var uow = TestMocks.CreateUnitOfWork(out var repo, out _, out _, out _, out _);
        Usuario entity = paciente ? new Paciente { Id = 1 } : new Medico { Id = 1 };
        repo.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(entity);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new DeleteUsuarioUseCase(uow.Object).ExecuteAsync(1));
    }
}
