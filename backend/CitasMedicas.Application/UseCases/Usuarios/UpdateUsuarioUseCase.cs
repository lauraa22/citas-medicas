using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Usuarios;

/// <summary>
/// Caso de uso encargado de actualizar un usuario existente.
/// </summary>
public class UpdateUsuarioUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    /// <summary>
    /// Inicializa una nueva instancia del caso de uso.
    /// </summary>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para gestionar la persistencia.
    /// </param>
    public UpdateUsuarioUseCase(
        IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    /// <summary>
    /// Actualiza un usuario existente.
    /// </summary>
    /// <param name="id">
    /// Identificador del usuario.
    /// </param>
    /// <param name="model">
    /// Nuevos datos del usuario.
    /// </param>
    /// <returns>
    /// True si el usuario se actualizó;
    /// false si no existe.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// Se produce si se intenta actualizar un paciente o un médico
    /// desde la sección de usuarios.
    /// </exception>
    public async Task<bool> ExecuteAsync(
        int id,
        UsuarioModel model)
    {
        var usuario =
            await _unitOfWork.Usuarios.GetByIdAsync(id);

        if (usuario is null)
            return false;

        if (usuario is Paciente || usuario is Medico)
        {
            throw new InvalidOperationException(
                "Los pacientes y médicos deben actualizarse desde su sección correspondiente.");
        }

        usuario.Nombre = model.Nombre;
        usuario.Apellidos = model.Apellidos;
        usuario.NombreUsuario = model.Usuario;

        if (!string.IsNullOrWhiteSpace(model.Clave))
        {
            usuario.Clave = model.Clave;
        }

        _unitOfWork.Usuarios.Update(usuario);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}