using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Pacientes;

/// <summary>
/// Caso de uso encargado de obtener todos los pacientes registrados.
/// </summary>
public class GetPacientesUseCase
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
    public GetPacientesUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los pacientes registrados.
    /// </summary>
    /// <returns>
    /// Colección de pacientes.
    /// </returns>
    public async Task<IEnumerable<PacienteModel>> ExecuteAsync()
    {
        var pacientes =
            await _unitOfWork.Pacientes
                .GetAllWithMedicosAsync();

        return _mapper.Map<IEnumerable<PacienteModel>>(
            pacientes);
    }
}