using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Medicos;

/// <summary>
/// Caso de uso encargado de eliminar un médico.
/// </summary>
public class DeleteMedicoUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia del caso de uso.
    /// </summary>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para gestionar la persistencia.
    /// </param>
    public DeleteMedicoUseCase(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Elimina un médico por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del médico.
    /// </param>
    /// <returns>
    /// True si el médico se eliminó;
    /// false si no existe.
    /// </returns>
    public async Task<bool> ExecuteAsync(int id)
    {
        var medico =
            await _unitOfWork.Medicos.GetByIdAsync(id);

        if (medico is null)
            return false;

        _unitOfWork.Medicos.Delete(medico);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}