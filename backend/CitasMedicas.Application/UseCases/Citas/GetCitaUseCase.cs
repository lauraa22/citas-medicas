using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Citas;

/// <summary>
/// Caso de uso encargado de obtener una cita por su identificador.
/// </summary>
public class GetCitaUseCase
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
    /// Mapper utilizado para convertir la entidad en un modelo de aplicación.
    /// </param>
    public GetCitaUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene una cita por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador de la cita.
    /// </param>
    /// <returns>
    /// Cita encontrada o null si no existe.
    /// </returns>
    public async Task<CitaModel?> ExecuteAsync(int id)
    {
        var cita =
            await _unitOfWork.Citas.GetByIdAsync(id);

        if (cita is null)
            return null;

        return _mapper.Map<CitaModel>(cita);
    }
}