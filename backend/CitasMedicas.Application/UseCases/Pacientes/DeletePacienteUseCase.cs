using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Pacientes;

/// <summary>
/// Caso de uso encargado de eliminar un paciente.
/// </summary>
public class DeletePacienteUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia del caso de uso.
    /// </summary>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para gestionar la persistencia.
    /// </param>
    public DeletePacienteUseCase(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Elimina un paciente por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del paciente.
    /// </param>
    /// <returns>
    /// True si el paciente se eliminó;
    /// false si no existe.
    /// </returns>
    public async Task<bool> ExecuteAsync(int id)
    {
        var paciente =
            await _unitOfWork.Pacientes.GetByIdAsync(id);

        if (paciente is null)
            return false;

        _unitOfWork.Pacientes.Delete(paciente);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}