using Xunit;
using AutoMapper;

using CitasMedicas.Application.Models;
using CitasMedicas.Application.UseCases.Citas;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces.Repositories;
using Moq;

namespace CitasMedicas.Tests.UseCases.Citas;

public class CitaUseCasesTests
{
    [Fact]
    public async Task GetCita_ExistingId_ReturnsModel()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out var citas, out _);
        var mapper = TestMocks.CreateMapper();
        citas.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Cita { Id = 1, MotivoCita = "Control" });

        var result = await new GetCitaUseCase(uow.Object, mapper.Object).ExecuteAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetCita_MissingId_ReturnsNull()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out var citas, out _);
        var mapper = TestMocks.CreateMapper();
        citas.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Cita?)null);

        var result = await new GetCitaUseCase(uow.Object, mapper.Object).ExecuteAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetCitas_ReturnsAll()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out var citas, out _);
        var mapper = new Mock<IMapper>();
        var entities = new[] { new Cita { Id = 1 }, new Cita { Id = 2 } };
        var models = new[] { new CitaModel { Id = 1 }, new CitaModel { Id = 2 } };
        citas.Setup(x => x.GetAllAsync()).ReturnsAsync(entities);
        mapper.Setup(x => x.Map<IEnumerable<CitaModel>>(entities)).Returns(models);

        var result = await new GetCitasUseCase(uow.Object, mapper.Object).ExecuteAsync();

        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task CreateCita_ValidData_AddsAndSaves()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out var medicos, out var citas, out _);
        var mapper = TestMocks.CreateMapper();
        pacientes.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Paciente { Id = 1 });
        medicos.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Medico { Id = 2 });
        var model = new CitaModel { PacienteId = 1, MedicoId = 2, MotivoCita = "Revisión" };

        var result = await new CreateCitaUseCase(uow.Object, mapper.Object).ExecuteAsync(model);

        citas.Verify(x => x.AddAsync(It.Is<Cita>(c => c.PacienteId == 1 && c.MedicoId == 2)), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
        Assert.Equal("Revisión", result.MotivoCita);
    }

    [Fact]
    public async Task CreateCita_MissingPatient_Throws()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out _, out _, out _);
        var mapper = TestMocks.CreateMapper();
        pacientes.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Paciente?)null);

        var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new CreateCitaUseCase(uow.Object, mapper.Object)
                .ExecuteAsync(new CitaModel { PacienteId = 99, MedicoId = 1 }));

        Assert.Contains("paciente", ex.Message, StringComparison.OrdinalIgnoreCase);
        uow.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task CreateCita_MissingDoctor_Throws()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out var medicos, out _, out _);
        var mapper = TestMocks.CreateMapper();
        pacientes.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Paciente { Id = 1 });
        medicos.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Medico?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new CreateCitaUseCase(uow.Object, mapper.Object)
                .ExecuteAsync(new CitaModel { PacienteId = 1, MedicoId = 99 }));
    }

    [Fact]
    public async Task CreateCita_MissingDiagnosis_Throws()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out var medicos, out _, out var diagnosticos);
        var mapper = TestMocks.CreateMapper();
        pacientes.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new Paciente { Id = 1 });
        medicos.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Medico { Id = 2 });
        diagnosticos.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Diagnostico?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new CreateCitaUseCase(uow.Object, mapper.Object)
                .ExecuteAsync(new CitaModel { PacienteId = 1, MedicoId = 2, DiagnosticoId = 99 }));
    }

    [Fact]
    public async Task UpdateCita_MissingCita_ReturnsFalse()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out var citas, out _);
        var mapper = TestMocks.CreateMapper();
        citas.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Cita?)null);

        var result = await new UpdateCitaUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(99, new CitaModel());

        Assert.False(result);
        uow.Verify(x => x.SaveChangesAsync(), Times.Never);
    }

    [Fact]
    public async Task UpdateCita_ValidData_UpdatesAndSaves()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out var pacientes, out var medicos, out var citas, out _);
        var mapper = TestMocks.CreateMapper();
        var cita = new Cita { Id = 1 };
        citas.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(cita);
        pacientes.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(new Paciente { Id = 2 });
        medicos.Setup(x => x.GetByIdAsync(3)).ReturnsAsync(new Medico { Id = 3 });

        var result = await new UpdateCitaUseCase(uow.Object, mapper.Object)
            .ExecuteAsync(1, new CitaModel { PacienteId = 2, MedicoId = 3, MotivoCita = "Nueva" });

        Assert.True(result);
        citas.Verify(x => x.Update(cita), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteCita_Existing_ReturnsTrueAndDeletes()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out var citas, out _);
        var cita = new Cita { Id = 1 };
        citas.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(cita);

        var result = await new DeleteCitaUseCase(uow.Object).ExecuteAsync(1);

        Assert.True(result);
        citas.Verify(x => x.Delete(cita), Times.Once);
        uow.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task DeleteCita_Missing_ReturnsFalse()
    {
        var uow = TestMocks.CreateUnitOfWork(out _, out _, out _, out var citas, out _);
        citas.Setup(x => x.GetByIdAsync(99)).ReturnsAsync((Cita?)null);

        var result = await new DeleteCitaUseCase(uow.Object).ExecuteAsync(99);

        Assert.False(result);
        citas.Verify(x => x.Delete(It.IsAny<Cita>()), Times.Never);
    }

    [Fact]
    public async Task UpdateCita_MissingPatient_Throws()
    {
        var uow = TestMocks.CreateUnitOfWork(
            out _,
            out var pacientes,
            out _,
            out var citas,
            out _);

        var mapper = TestMocks.CreateMapper();

        var cita = new Cita
        {
            Id = 1
        };

        citas.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(cita);

        pacientes.Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Paciente?)null);

        var model = new CitaModel
        {
            PacienteId = 99,
            MedicoId = 1
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new UpdateCitaUseCase(
                uow.Object,
                mapper.Object)
            .ExecuteAsync(
                1,
                model));

        uow.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task UpdateCita_MissingDoctor_Throws()
    {
        var uow = TestMocks.CreateUnitOfWork(
            out _,
            out var pacientes,
            out var medicos,
            out var citas,
            out _);

        var mapper = TestMocks.CreateMapper();

        var cita = new Cita
        {
            Id = 1
        };

        citas.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(cita);

        pacientes.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new Paciente
            {
                Id = 2
            });

        medicos.Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Medico?)null);

        var model = new CitaModel
        {
            PacienteId = 2,
            MedicoId = 99
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new UpdateCitaUseCase(
                uow.Object,
                mapper.Object)
            .ExecuteAsync(
                1,
                model));

        uow.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }

    [Fact]
    public async Task UpdateCita_MissingDiagnosis_Throws()
    {
        var uow = TestMocks.CreateUnitOfWork(
            out _,
            out var pacientes,
            out var medicos,
            out var citas,
            out var diagnosticos);

        var mapper = TestMocks.CreateMapper();

        var cita = new Cita
        {
            Id = 1
        };

        citas.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(cita);

        pacientes.Setup(x => x.GetByIdAsync(2))
            .ReturnsAsync(new Paciente
            {
                Id = 2
            });

        medicos.Setup(x => x.GetByIdAsync(3))
            .ReturnsAsync(new Medico
            {
                Id = 3
            });

        diagnosticos.Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Diagnostico?)null);

        var model = new CitaModel
        {
            PacienteId = 2,
            MedicoId = 3,
            DiagnosticoId = 99
        };

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            new UpdateCitaUseCase(
                uow.Object,
                mapper.Object)
            .ExecuteAsync(
                1,
                model));

        uow.Verify(
            x => x.SaveChangesAsync(),
            Times.Never);
    }


}
