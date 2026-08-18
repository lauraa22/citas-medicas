using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Citas;

/// <summary>
/// Caso de uso encargado de crear una nueva cita médica.
/// </summary>
public class CreateCitaUseCase
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
    public CreateCitaUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Crea una nueva cita médica.
    /// </summary>
    /// <param name="model">
    /// Datos de la cita que se desea crear.
    /// </param>
    /// <returns>
    /// Cita creada.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se produce si no existe el paciente, médico
    /// o diagnóstico indicado.
    /// </exception>
    public async Task<CitaModel> ExecuteAsync(
        CitaModel model)
    {
        var paciente =
            await _unitOfWork.Pacientes
                .GetByIdAsync(model.PacienteId);

        if (paciente is null)
        {
            throw new InvalidOperationException(
                $"No existe el paciente con id {model.PacienteId}.");
        }

        var medico =
            await _unitOfWork.Medicos
                .GetByIdAsync(model.MedicoId);

        if (medico is null)
        {
            throw new InvalidOperationException(
                $"No existe el médico con id {model.MedicoId}.");
        }

        if (model.DiagnosticoId.HasValue)
        {
            var diagnostico =
                await _unitOfWork.Diagnosticos
                    .GetByIdAsync(
                        model.DiagnosticoId.Value);

            if (diagnostico is null)
            {
                throw new InvalidOperationException(
                    $"No existe el diagnóstico con id " +
                    $"{model.DiagnosticoId.Value}.");
            }
        }

        var cita =
            _mapper.Map<Cita>(model);

        await _unitOfWork.Citas.AddAsync(cita);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CitaModel>(cita);
    }
}