using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Medicos;

/// <summary>
/// Caso de uso encargado de actualizar un médico existente.
/// </summary>
public class UpdateMedicoUseCase
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
    public UpdateMedicoUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Actualiza un médico existente.
    /// </summary>
    /// <param name="id">
    /// Identificador del médico.
    /// </param>
    /// <param name="model">
    /// Nuevos datos del médico.
    /// </param>
    /// <returns>
    /// True si el médico se actualizó;
    /// false si no existe.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se produce si alguno de los pacientes indicados no existe.
    /// </exception>
    public async Task<bool> ExecuteAsync(
        int id,
        MedicoModel model)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var medico =
                await _unitOfWork.Medicos
                    .GetByIdWithPacientesAsync(id);

            if (medico is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }

            var claveActual = medico.Clave;

            _mapper.Map(model, medico);

            if (string.IsNullOrWhiteSpace(model.Clave))
            {
                medico.Clave = claveActual;
            }

            medico.Pacientes.Clear();

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

            _unitOfWork.Medicos.Update(medico);

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