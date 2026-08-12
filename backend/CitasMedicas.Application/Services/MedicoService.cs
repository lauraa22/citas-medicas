using AutoMapper;
using CitasMedicas.Application.DTOs.Medicos;
using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Application.Interfaces.Services;
using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Application.Services;

/// <summary>
/// Servicio encargado de la lógica de negocio de médicos
/// y de la gestión de sus relaciones con pacientes.
/// </summary>
public class MedicoService : IMedicoService{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public MedicoService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<MedicoDto>> GetAllAsync()
    {
        var medicos =
            await _unitOfWork.Medicos.GetAllWithPacientesAsync();

        return _mapper.Map<IEnumerable<MedicoDto>>(medicos);
    }

    public async Task<MedicoDto?> GetByIdAsync(int id)
    {
        var medico =
            await _unitOfWork.Medicos.GetByIdWithPacientesAsync(id);

        if (medico is null)
            return null;

        return _mapper.Map<MedicoDto>(medico);
    }

    public async Task<MedicoDto> CreateAsync(
        MedicoCreateDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var medico = _mapper.Map<Medico>(dto);

            foreach (var pacienteId in dto.PacienteIds.Distinct())
            {
                var paciente =
                    await _unitOfWork.Pacientes.GetByIdAsync(pacienteId);

                if (paciente is null)
                {
                    throw new InvalidOperationException(
                        $"No existe el paciente con id {pacienteId}.");
                }

                medico.Pacientes.Add(paciente);
            }

            await _unitOfWork.Medicos.AddAsync(medico);

            await _unitOfWork.SaveChangesAsync();

            await _unitOfWork.CommitTransactionAsync();

            return _mapper.Map<MedicoDto>(medico);
        }
        catch
        {
            await _unitOfWork.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool> UpdateAsync(
        int id,
        MedicoUpdateDto dto)
    {
        await _unitOfWork.BeginTransactionAsync();

        try
        {
            var medico =
                await _unitOfWork.Medicos
                    .GetByIdWithPacientesAsync(id);

            if (medico is null)
            {
                await _unitOfWork.RollbackTransactionAsync();
                return false;
            }

            _mapper.Map(dto, medico);

            medico.Pacientes.Clear();

            foreach (var pacienteId in dto.PacienteIds.Distinct())
            {
                var paciente =
                    await _unitOfWork.Pacientes.GetByIdAsync(pacienteId);

                if (paciente is null)
                {
                    throw new InvalidOperationException(
                        $"No existe el paciente con id {pacienteId}.");
                }

                medico.Pacientes.Add(paciente);
            }

            _unitOfWork.Medicos.Update(medico);

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
        var medico =
            await _unitOfWork.Medicos.GetByIdAsync(id);

        if (medico is null)
            return false;

        _unitOfWork.Medicos.Delete(medico);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}