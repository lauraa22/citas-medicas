using AutoMapper;
using CitasMedicas.Application.DTOs.Pacientes;
using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Application.Interfaces.Services;
using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Application.Services;

/// <summary>
/// Servicio encargado de la lógica de negocio relacionada
/// con los pacientes, incluyendo la gestión de sus relaciones
/// con médicos y el control transaccional de las operaciones.
/// </summary>
public class PacienteService : IPacienteService {
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PacienteService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PacienteDto>> GetAllAsync()
    {
        var pacientes =
            await _unitOfWork.Pacientes.GetAllWithMedicosAsync();

        return _mapper.Map<IEnumerable<PacienteDto>>(pacientes);
    }

    public async Task<PacienteDto?> GetByIdAsync(int id)
    {
        var paciente =
            await _unitOfWork.Pacientes.GetByIdWithMedicosAsync(id);

        if (paciente is null)
            return null;

        return _mapper.Map<PacienteDto>(paciente);
    }

    public async Task<PacienteDto> CreateAsync(
        PacienteCreateDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var paciente = _mapper.Map<Paciente>(dto);

            foreach (var medicoId in dto.MedicoIds.Distinct())
            {
                var medico =
                    await _unitOfWork.Medicos.GetByIdAsync(medicoId);

                if (medico is null)
                {
                    throw new InvalidOperationException(
                        $"No existe el médico con id {medicoId}.");
                }

                paciente.Medicos.Add(medico);
            }

            await _unitOfWork.Pacientes.AddAsync(paciente);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<PacienteDto>(paciente);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(
        int id,
        PacienteUpdateDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var paciente =
                await _unitOfWork.Pacientes
                    .GetByIdWithMedicosAsync(id);

            if (paciente is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }

            var claveActual = paciente.Clave;

            _mapper.Map(dto, paciente);

            if (string.IsNullOrWhiteSpace(dto.Clave))
            {
                paciente.Clave = claveActual;
            }

            paciente.Medicos.Clear();

            foreach (var medicoId in dto.MedicoIds.Distinct())
            {
                var medico =
                    await _unitOfWork.Medicos.GetByIdAsync(medicoId);

                if (medico is null)
                {
                    throw new InvalidOperationException(
                        $"No existe el médico con id {medicoId}.");
                }

                paciente.Medicos.Add(medico);
            }

            _unitOfWork.Pacientes.Update(paciente);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var paciente =
            await _unitOfWork.Pacientes.GetByIdAsync(id);

        if (paciente is null)
            return false;

        _unitOfWork.Pacientes.Delete(paciente);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}