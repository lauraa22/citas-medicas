using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Diagnosticos;

/// <summary>
/// Caso de uso encargado de actualizar un diagnóstico existente.
/// </summary>
public class UpdateDiagnosticoUseCase
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
    /// Mapper utilizado para actualizar la entidad.
    /// </param>
    public UpdateDiagnosticoUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Actualiza un diagnóstico existente.
    /// </summary>
    /// <param name="id">
    /// Identificador del diagnóstico.
    /// </param>
    /// <param name="model">
    /// Nuevos datos del diagnóstico.
    /// </param>
    /// <returns>
    /// True si el diagnóstico se actualizó;
    /// false si no existe.
    /// </returns>
    public async Task<bool> ExecuteAsync(
        int id,
        DiagnosticoModel model)
    {
        var diagnostico =
            await _unitOfWork.Diagnosticos.GetByIdAsync(id);

        if (diagnostico is null)
            return false;

        _mapper.Map(model, diagnostico);

        _unitOfWork.Diagnosticos.Update(
            diagnostico);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}