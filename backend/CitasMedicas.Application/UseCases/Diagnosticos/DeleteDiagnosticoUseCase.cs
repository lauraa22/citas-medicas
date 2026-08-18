using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Diagnosticos;

/// <summary>
/// Caso de uso encargado de eliminar un diagnóstico.
/// </summary>
public class DeleteDiagnosticoUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia del caso de uso.
    /// </summary>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para gestionar la persistencia.
    /// </param>
    public DeleteDiagnosticoUseCase(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Elimina un diagnóstico por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del diagnóstico.
    /// </param>
    /// <returns>
    /// True si el diagnóstico se eliminó;
    /// false si no existe.
    /// </returns>
    public async Task<bool> ExecuteAsync(int id)
    {
        var diagnostico =
            await _unitOfWork.Diagnosticos.GetByIdAsync(id);

        if (diagnostico is null)
            return false;

        _unitOfWork.Diagnosticos.Delete(
            diagnostico);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}