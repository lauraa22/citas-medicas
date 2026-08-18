using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Diagnosticos;

/// <summary>
/// Caso de uso encargado de obtener un diagnóstico
/// por su identificador.
/// </summary>
public class GetDiagnosticoUseCase
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
    public GetDiagnosticoUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene un diagnóstico por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del diagnóstico.
    /// </param>
    /// <returns>
    /// Diagnóstico encontrado o null si no existe.
    /// </returns>
    public async Task<DiagnosticoModel?> ExecuteAsync(int id)
    {
        var diagnostico =
            await _unitOfWork.Diagnosticos.GetByIdAsync(id);

        if (diagnostico is null)
            return null;

        return _mapper.Map<DiagnosticoModel>(diagnostico);
    }
}