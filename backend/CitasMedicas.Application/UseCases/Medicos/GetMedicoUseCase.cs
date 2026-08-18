using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Interfaces.Repositories;

namespace CitasMedicas.Application.UseCases.Medicos;

/// <summary>
/// Caso de uso encargado de obtener un médico por su identificador.
/// </summary>
public class GetMedicoUseCase
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
    public GetMedicoUseCase(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene un médico por su identificador.
    /// </summary>
    /// <param name="id">
    /// Identificador del médico.
    /// </param>
    /// <returns>
    /// Médico encontrado o null si no existe.
    /// </returns>
    public async Task<MedicoModel?> ExecuteAsync(int id)
    {
        var medico =
            await _unitOfWork.Medicos
                .GetByIdWithPacientesAsync(id);

        if (medico is null)
            return null;

        return _mapper.Map<MedicoModel>(medico);
    }
}