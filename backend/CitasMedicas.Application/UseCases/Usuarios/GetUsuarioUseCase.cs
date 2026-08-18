using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Usuarios;

/// <summary>
/// Caso de uso encargado de obtener un usuario por su identificador.
/// </summary>
public class GetUsuarioUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia del caso de uso.
    /// </summary>
    /// <param name="unitOfWork">
    /// Unidad de trabajo utilizada para acceder a los repositorios.
    /// </param>
    /// <param name="mapper">
    /// Mapper utilizado para convertir la entidad
    /// en un modelo de aplicación.
    /// </param>
    public GetUsuarioUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene un usuario por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del usuario.
    /// </param>
    /// <returns>
    /// Usuario encontrado o null si no existe.
    /// </returns>
    public async Task<UsuarioModel?> ExecuteAsync(int id)
    {
        var usuario =
            await _unitOfWork.Usuarios.GetByIdAsync(id);

        if (usuario is null)
            return null;

        return _mapper.Map<UsuarioModel>(usuario);
    }
}