using AutoMapper;
using CitasMedicas.Application.DTOs.Citas;
using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Application.Interfaces.Services;
using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Application.Services;

/// <summary>
/// Servicio encargado de gestionar las citas médicas
/// y validar la existencia del paciente, médico y diagnóstico
/// asociados.
/// </summary>
public class CitaService : ICitaService{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CitaService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CitaDto>> GetAllAsync()
    {
        var citas = await _unitOfWork.Citas.GetAllAsync();

        return _mapper.Map<IEnumerable<CitaDto>>(citas);
    }

    public async Task<CitaDto?> GetByIdAsync(int id)
    {
        var cita = await _unitOfWork.Citas.GetByIdAsync(id);

        if (cita is null)
            return null;

        return _mapper.Map<CitaDto>(cita);
    }

    public async Task<CitaDto> CreateAsync(
        CitaCreateDto dto)
    {
        var paciente =
            await _unitOfWork.Pacientes.GetByIdAsync(dto.PacienteId);

        if (paciente is null)
        {
            throw new InvalidOperationException(
                $"No existe el paciente con id {dto.PacienteId}.");
        }

        var medico =
            await _unitOfWork.Medicos.GetByIdAsync(dto.MedicoId);

        if (medico is null)
        {
            throw new InvalidOperationException(
                $"No existe el médico con id {dto.MedicoId}.");
        }

        if (dto.DiagnosticoId.HasValue)
        {
            var diagnostico =
                await _unitOfWork.Diagnosticos
                    .GetByIdAsync(dto.DiagnosticoId.Value);

            if (diagnostico is null)
            {
                throw new InvalidOperationException(
                    $"No existe el diagnóstico con id {dto.DiagnosticoId.Value}.");
            }
        }

        var cita = _mapper.Map<Cita>(dto);

        await _unitOfWork.Citas.AddAsync(cita);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<CitaDto>(cita);
    }

    public async Task<bool> UpdateAsync(
        int id,
        CitaUpdateDto dto)
    {
        var cita = await _unitOfWork.Citas.GetByIdAsync(id);

        if (cita is null)
            return false;

        var paciente =
            await _unitOfWork.Pacientes.GetByIdAsync(dto.PacienteId);

        if (paciente is null)
        {
            throw new InvalidOperationException(
                $"No existe el paciente con id {dto.PacienteId}.");
        }

        var medico =
            await _unitOfWork.Medicos.GetByIdAsync(dto.MedicoId);

        if (medico is null)
        {
            throw new InvalidOperationException(
                $"No existe el médico con id {dto.MedicoId}.");
        }

        if (dto.DiagnosticoId.HasValue)
        {
            var diagnostico =
                await _unitOfWork.Diagnosticos
                    .GetByIdAsync(dto.DiagnosticoId.Value);

            if (diagnostico is null)
            {
                throw new InvalidOperationException(
                    $"No existe el diagnóstico con id {dto.DiagnosticoId.Value}.");
            }
        }

        _mapper.Map(dto, cita);

        _unitOfWork.Citas.Update(cita);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var cita = await _unitOfWork.Citas.GetByIdAsync(id);

        if (cita is null)
            return false;

        _unitOfWork.Citas.Delete(cita);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}