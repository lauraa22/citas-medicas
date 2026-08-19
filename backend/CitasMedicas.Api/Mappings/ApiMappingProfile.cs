using AutoMapper;
using CitasMedicas.Api.DTOs;
using CitasMedicas.Application.Models;

namespace CitasMedicas.Api.Mappings;

/// <summary>
/// Configuración de AutoMapper para los DTO de la API
/// y los modelos de la capa de aplicación.
/// </summary>
public class ApiMappingProfile : Profile
{
    /// <summary>
    /// Inicializa las configuraciones de mapeo de la API.
    /// </summary>
    public ApiMappingProfile()
    {
        CreateMap<UsuarioDto, UsuarioModel>();

        CreateMap<UsuarioModel, UsuarioDto>()
            .ForMember(
                destination => destination.Clave,
                options => options.Ignore());


        CreateMap<PacienteDto, PacienteModel>();

        CreateMap<PacienteModel, PacienteDto>()
            .ForMember(
                destination => destination.Clave,
                options => options.Ignore());


        CreateMap<MedicoDto, MedicoModel>();

        CreateMap<MedicoModel, MedicoDto>()
            .ForMember(
                destination => destination.Clave,
                options => options.Ignore());


        CreateMap<CitaDto, CitaModel>()
            .ReverseMap();


        CreateMap<DiagnosticoDto, DiagnosticoModel>()
            .ReverseMap();
    }
}