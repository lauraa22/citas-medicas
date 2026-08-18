using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Pacientes;

/// <summary>
/// Caso de uso encargado de crear un nuevo paciente.
/// </summary>
public class CreatePacienteUseCase
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
    public CreatePacienteUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Crea un nuevo paciente.
    /// </summary>
    /// <param name="model">
    /// Datos del paciente que se desea crear.
    /// </param>
    /// <returns>
    /// Paciente creado.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se produce si alguno de los médicos indicados no existe.
    /// </exception>
    public async Task<PacienteModel> ExecuteAsync(
        PacienteModel model)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var paciente =
                _mapper.Map<Paciente>(model);

            foreach (var medicoId
                     in model.MedicoIds.Distinct())
            {
                var medico =
                    await _unitOfWork.Medicos
                        .GetByIdAsync(medicoId);

                if (medico is null)
                {
                    throw new InvalidOperationException(
                        $"No existe el médico con id {medicoId}.");
                }

                paciente.Medicos.Add(medico);
            }

            await _unitOfWork.Pacientes
                .AddAsync(paciente);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<PacienteModel>(
                paciente);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}