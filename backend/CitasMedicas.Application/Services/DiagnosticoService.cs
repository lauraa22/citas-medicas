using AutoMapper;
using CitasMedicas.Application.DTOs.Diagnosticos;
using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Application.Interfaces.Services;
using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Application.Services;

public class DiagnosticoService : IDiagnosticoService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DiagnosticoService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<DiagnosticoDto>> GetAllAsync()
    {
        var diagnosticos =
            await _unitOfWork.Diagnosticos.GetAllAsync();

        return _mapper.Map<IEnumerable<DiagnosticoDto>>(diagnosticos);
    }

    public async Task<DiagnosticoDto?> GetByIdAsync(int id)
    {
        var diagnostico =
            await _unitOfWork.Diagnosticos.GetByIdAsync(id);

        if (diagnostico is null)
            return null;

        return _mapper.Map<DiagnosticoDto>(diagnostico);
    }

    public async Task<DiagnosticoDto> CreateAsync(
        DiagnosticoCreateDto dto)
    {
        var diagnostico =
            _mapper.Map<Diagnostico>(dto);

        await _unitOfWork.Diagnosticos.AddAsync(diagnostico);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DiagnosticoDto>(diagnostico);
    }

    public async Task<bool> UpdateAsync(
        int id,
        DiagnosticoUpdateDto dto)
    {
        var diagnostico =
            await _unitOfWork.Diagnosticos.GetByIdAsync(id);

        if (diagnostico is null)
            return false;

        _mapper.Map(dto, diagnostico);

        _unitOfWork.Diagnosticos.Update(diagnostico);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var diagnostico =
            await _unitOfWork.Diagnosticos.GetByIdAsync(id);

        if (diagnostico is null)
            return false;

        _unitOfWork.Diagnosticos.Delete(diagnostico);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}