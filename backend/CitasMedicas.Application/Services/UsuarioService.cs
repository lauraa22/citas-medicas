using AutoMapper;
using CitasMedicas.Application.DTOs.Usuarios;
using CitasMedicas.Application.Interfaces.Repositories;
using CitasMedicas.Application.Interfaces.Services;
using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Application.Services;

/// <summary>
/// Servicio encargado de la lógica de negocio relacionada
/// con la gestión de usuarios.
/// </summary>
public class UsuarioService : IUsuarioService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UsuarioService(
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UsuarioDto>> GetAllAsync()
    {
        var usuarios =
            await _unitOfWork.Usuarios.GetAllAsync();

        return _mapper.Map<IEnumerable<UsuarioDto>>(
            usuarios);
    }

    public async Task<UsuarioDto?> GetByIdAsync(int id)
    {
        var usuario =
            await _unitOfWork.Usuarios.GetByIdAsync(id);

        if (usuario is null)
            return null;

        return _mapper.Map<UsuarioDto>(usuario);
    }

    public async Task<UsuarioDto> CreateAsync(
        UsuarioCreateDto dto)
    {
        var usuarios =
            await _unitOfWork.Usuarios.GetAllAsync();

        var exists = usuarios.Any(
            usuario =>
                usuario.NombreUsuario.Equals(
                    dto.Usuario,
                    StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            throw new InvalidOperationException(
                "El nombre de usuario ya existe.");
        }

        var usuario =
            _mapper.Map<Usuario>(dto);

        await _unitOfWork.Usuarios.AddAsync(
            usuario);

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<UsuarioDto>(
            usuario);
    }

   public async Task<bool> UpdateAsync(
        int id,
        UsuarioUpdateDto dto)
    {
        var usuario =
            await _unitOfWork.Usuarios.GetByIdAsync(id);

        if (usuario is null)
            return false;

        var usuarios =
            await _unitOfWork.Usuarios.GetAllAsync();

        var exists = usuarios.Any(
            existing =>
                existing.Id != id &&
                existing.NombreUsuario.Equals(
                    dto.Usuario,
                    StringComparison.OrdinalIgnoreCase));

        if (exists)
        {
            throw new InvalidOperationException(
                "El nombre de usuario ya existe.");
        }

        usuario.Nombre = dto.Nombre;
        usuario.Apellidos = dto.Apellidos;
        usuario.NombreUsuario = dto.Usuario;

        if (!string.IsNullOrWhiteSpace(dto.Clave))
        {
            usuario.Clave = dto.Clave;
        }

        _unitOfWork.Usuarios.Update(usuario);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var usuario =
            await _unitOfWork.Usuarios.GetByIdAsync(id);

        if (usuario is null)
            return false;

        /*
         * Evitamos eliminar mediante /api/usuarios
         * objetos que realmente sean pacientes o médicos.
         * Esos tipos tienen sus propios endpoints.
         */
        if (usuario is Paciente ||
            usuario is Medico)
        {
            throw new InvalidOperationException(
                "Los pacientes y médicos deben eliminarse desde su sección correspondiente.");
        }

        _unitOfWork.Usuarios.Delete(usuario);

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}