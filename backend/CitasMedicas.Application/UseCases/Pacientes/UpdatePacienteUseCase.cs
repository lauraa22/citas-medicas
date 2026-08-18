using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Pacientes;

/// <summary>
/// Caso de uso encargado de actualizar un paciente existente.
/// </summary>
public class UpdatePacienteUseCase
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
    public UpdatePacienteUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Actualiza un paciente existente.
    /// </summary>
    /// <param name="id">
    /// Identificador del paciente.
    /// </param>
    /// <param name="model">
    /// Nuevos datos del paciente.
    /// </param>
    /// <returns>
    /// True si el paciente se actualizó;
    /// false si no existe.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se produce si alguno de los médicos indicados no existe.
    /// </exception>
    public async Task<bool> ExecuteAsync(
        int id,
        PacienteModel model)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var paciente =
                await _unitOfWork.Pacientes
                    .GetByIdWithMedicosAsync(id);

            if (paciente is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }

            var claveActual = paciente.Clave;

            _mapper.Map(model, paciente);

            if (string.IsNullOrWhiteSpace(model.Clave))
            {
                paciente.Clave = claveActual;
            }

            paciente.Medicos.Clear();

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

            _unitOfWork.Pacientes.Update(paciente);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }
}