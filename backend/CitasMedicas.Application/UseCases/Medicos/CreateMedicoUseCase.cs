using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Medicos;

/// <summary>
/// Caso de uso encargado de crear un nuevo médico.
/// </summary>
public class CreateMedicoUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia del caso de uso.
    /// </summary>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para gestionar la persistencia.
    /// </param>
    /// <param name="mapper">
    /// Mapper utilizado para convertir modelos y entidades.
    /// </param>
    public CreateMedicoUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Crea un nuevo médico.
    /// </summary>
    /// <param name="model">
    /// Datos del médico que se desea crear.
    /// </param>
    /// <returns>
    /// Médico creado.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se produce si alguno de los pacientes indicados no existe.
    /// </exception>
    public async Task<MedicoModel> ExecuteAsync(
        MedicoModel model)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var medico =
                _mapper.Map<Medico>(model);

            foreach (var pacienteId
                     in model.PacienteIds.Distinct())
            {
                var paciente =
                    await _unitOfWork.Pacientes
                        .GetByIdAsync(pacienteId);

                if (paciente is null)
                {
                    throw new InvalidOperationException(
                        $"No existe el paciente con id {pacienteId}.");
                }

                medico.Pacientes.Add(paciente);
            }

            await _unitOfWork.Medicos.AddAsync(medico);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<MedicoModel>(medico);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}