using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Usuarios;

/// <summary>
/// Caso de uso encargado de eliminar un usuario.
/// </summary>
public class DeleteUsuarioUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia del caso de uso.
    /// </summary>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para gestionar la persistencia.
    /// </param>
    public DeleteUsuarioUseCase(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Elimina un usuario por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del usuario.
    /// </param>
    /// <returns>
    /// True si el usuario se eliminó;
    /// false si no existe.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se produce si se intenta eliminar un paciente o un médico
    /// desde la sección de usuarios.
    /// </exception>
    public async Task<bool> ExecuteAsync(int id)
    {
        var usuario =
            await _unitOfWork.Usuarios.GetByIdAsync(id);

        if (usuario is null)
            return false;

        if (usuario is Paciente || usuario is Medico)
        {
            throw new InvalidOperationException(
                "Los pacientes y médicos deben eliminarse desde su sección correspondiente.");
        }

        _unitOfWork.Usuarios.Delete(usuario);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}