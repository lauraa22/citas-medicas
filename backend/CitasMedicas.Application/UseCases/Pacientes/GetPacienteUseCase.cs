using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Pacientes;

/// <summary>
/// Caso de uso encargado de obtener un paciente por su identificador.
/// </summary>
public class GetPacienteUseCase
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
    public GetPacienteUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene un paciente por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del paciente.
    /// </param>
    /// <returns>
    /// Paciente encontrado o null si no existe.
    /// </returns>
    public async Task<PacienteModel?> ExecuteAsync(int id)
    {
        var paciente =
            await _unitOfWork.Pacientes
                .GetByIdWithMedicosAsync(id);

        if (paciente is null)
            return null;

        return _mapper.Map<PacienteModel>(paciente);
    }
}