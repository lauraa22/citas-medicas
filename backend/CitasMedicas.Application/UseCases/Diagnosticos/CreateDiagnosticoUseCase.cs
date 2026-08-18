using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Entities;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Diagnosticos;

/// <summary>
/// Caso de uso encargado de crear un nuevo diagnóstico.
/// </summary>
public class CreateDiagnosticoUseCase
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
    public CreateDiagnosticoUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Crea un nuevo diagnóstico.
    /// </summary>
    /// <param name="model">
    /// Datos del diagnóstico que se desea crear.
    /// </param>
    /// <returns>
    /// Diagnóstico creado.
    /// </returns>
    public async Task<DiagnosticoModel> ExecuteAsync(
        DiagnosticoModel model)
    {
        var diagnostico =
            _mapper.Map<Diagnostico>(model);

        await _unitOfWork.Diagnosticos.AddAsync(
            diagnostico);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DiagnosticoModel>(
            diagnostico);
    }
}