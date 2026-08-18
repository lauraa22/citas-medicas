using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Citas;

/// <summary>
/// Caso de uso encargado de actualizar una cita médica existente.
/// </summary>
public class UpdateCitaUseCase
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
    /// Mapper utilizado para actualizar la entidad.
    /// </param>
    public UpdateCitaUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Actualiza una cita existente.
    /// </summary>
    /// <param name="id">
    /// Identificador de la cita.
    /// </param>
    /// <param name="model">
    /// Nuevos datos de la cita.
    /// </param>
    /// <returns>
    /// True si la cita se actualizó correctamente;
    /// false si no existe.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se produce si no existe el paciente, médico
    /// o diagnóstico indicado.
    /// </exception>
    public async Task<bool> ExecuteAsync(
        int id,
        CitaModel model)
    {
        var cita =
            await _unitOfWork.Citas.GetByIdAsync(id);

        if (cita is null)
            return false;

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

        _mapper.Map(model, cita);

        _unitOfWork.Citas.Update(cita);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}