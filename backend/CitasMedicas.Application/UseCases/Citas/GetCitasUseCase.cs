using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Citas;

/// <summary>
/// Caso de uso encargado de obtener todas las citas registradas.
/// </summary>
public class GetCitasUseCase
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
    /// Mapper utilizado para convertir las entidades de dominio
    /// en modelos de aplicación.
    /// </param>
    public GetCitasUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las citas registradas.
    /// </summary>
    /// <returns>
    /// Colección de citas.
    /// </returns>
    public async Task<IEnumerable<CitaModel>> ExecuteAsync()
    {
        var citas =
            await _unitOfWork.Citas.GetAllAsync();

        return _mapper.Map<IEnumerable<CitaModel>>(citas);
    }
}