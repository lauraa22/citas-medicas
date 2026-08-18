using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Usuarios;

/// <summary>
/// Caso de uso encargado de obtener todos los usuarios registrados.
/// </summary>
public class GetUsuariosUseCase
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
    /// Mapper utilizado para convertir entidades en modelos de aplicación.
    /// </param>
    public GetUsuariosUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los usuarios registrados.
    /// </summary>
    /// <returns>
    /// Colección de usuarios.
    /// </returns>
    public async Task<IEnumerable<UsuarioModel>> ExecuteAsync()
    {
        var usuarios =
            await _unitOfWork.Usuarios.GetAllAsync();

        return _mapper.Map<IEnumerable<UsuarioModel>>(usuarios);
    }
}