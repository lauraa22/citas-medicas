using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Citas;

/// <summary>
/// Caso de uso encargado de eliminar una cita médica.
/// </summary>
public class DeleteCitaUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia del caso de uso.
    /// </summary>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para gestionar la persistencia.
    /// </param>
    public DeleteCitaUseCase(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Elimina una cita por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador de la cita.
    /// </param>
    /// <returns>
    /// True si la cita fue eliminada;
    /// false si no existe.
    /// </returns>
    public async Task<bool> ExecuteAsync(int id)
    {
        var cita =
            await _unitOfWork.Citas.GetByIdAsync(id);

        if (cita is null)
            return false;

        _unitOfWork.Citas.Delete(cita);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}