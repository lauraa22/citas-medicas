using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Diagnosticos;

/// <summary>
/// Caso de uso encargado de obtener todos los diagnósticos registrados.
/// </summary>
public class GetDiagnosticosUseCase
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
    public GetDiagnosticosUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los diagnósticos registrados.
    /// </summary>
    /// <returns>
    /// Colección de diagnósticos.
    /// </returns>
    public async Task<IEnumerable<DiagnosticoModel>> ExecuteAsync()
    {
        var diagnosticos =
            await _unitOfWork.Diagnosticos.GetAllAsync();

        return _mapper.Map<IEnumerable<DiagnosticoModel>>(
            diagnosticos);
    }
}