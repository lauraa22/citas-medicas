using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Medicos;

/// <summary>
/// Caso de uso encargado de obtener todos los médicos registrados.
/// </summary>
public class GetMedicosUseCase
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
    public GetMedicosUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los médicos registrados.
    /// </summary>
    /// <returns>
    /// Colección de médicos.
    /// </returns>
    public async Task<IEnumerable<MedicoModel>> ExecuteAsync()
    {
        var medicos =
            await _unitOfWork.Medicos
                .GetAllWithPacientesAsync();

        return _mapper.Map<IEnumerable<MedicoModel>>(medicos);
    }
}