using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Usuarios;

/// <summary>
/// Caso de uso encargado de crear un nuevo usuario.
/// </summary>
public class CreateUsuarioUseCase
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
    /// Mapper utilizado para convertir modelos y entidades.
    /// </param>
    public CreateUsuarioUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Crea un nuevo usuario.
    /// </summary>
    /// <param name="model">
    /// Datos del usuario que se desea crear.
    /// </param>
    /// <returns>
    /// Usuario creado.
    /// </returns>
    public async Task<UsuarioModel> ExecuteAsync(
        UsuarioModel model)
    {
        var usuario =
            _mapper.Map<Usuario>(model);

        await _unitOfWork.Usuarios.AddAsync(usuario);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UsuarioModel>(usuario);
    }
}