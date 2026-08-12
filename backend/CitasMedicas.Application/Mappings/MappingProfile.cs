using AutoMapper;
using CitasMedicas.Application.DTOs.Citas;
using CitasMedicas.Application.DTOs.Diagnosticos;
using CitasMedicas.Application.DTOs.Medicos;
using CitasMedicas.Application.DTOs.Pacientes;
using CitasMedicas.Application.DTOs.Usuarios;
using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Usuario, UsuarioDto>()
            .ForMember(
                destination => destination.Usuario,
                options => options.MapFrom(source => source.NombreUsuario));

        CreateMap<Paciente, PacienteDto>()
            .ForMember(
                destination => destination.Usuario,
                options => options.MapFrom(source => source.NombreUsuario))
            .ForMember(
                destination => destination.MedicoIds,
                options => options.MapFrom(
                    source => source.Medicos.Select(m => m.Id)));

        CreateMap<PacienteCreateDto, Paciente>()
            .ForMember(
                destination => destination.NombreUsuario,
                options => options.MapFrom(source => source.Usuario))
            .ForMember(
                destination => destination.Medicos,
                options => options.Ignore())
            .ForMember(
                destination => destination.Citas,
                options => options.Ignore());

        CreateMap<PacienteUpdateDto, Paciente>()
            .ForMember(
                destination => destination.NombreUsuario,
                options => options.MapFrom(source => source.Usuario))
            .ForMember(
                destination => destination.Medicos,
                options => options.Ignore())
            .ForMember(
                destination => destination.Citas,
                options => options.Ignore());

        CreateMap<Medico, MedicoDto>()
            .ForMember(
                destination => destination.Usuario,
                options => options.MapFrom(source => source.NombreUsuario))
            .ForMember(
                destination => destination.PacienteIds,
                options => options.MapFrom(
                    source => source.Pacientes.Select(p => p.Id)));

        CreateMap<MedicoCreateDto, Medico>()
            .ForMember(
                destination => destination.NombreUsuario,
                options => options.MapFrom(source => source.Usuario))
            .ForMember(
                destination => destination.Pacientes,
                options => options.Ignore())
            .ForMember(
                destination => destination.Citas,
                options => options.Ignore());

        CreateMap<MedicoUpdateDto, Medico>()
            .ForMember(
                destination => destination.NombreUsuario,
                options => options.MapFrom(source => source.Usuario))
            .ForMember(
                destination => destination.Pacientes,
                options => options.Ignore())
            .ForMember(
                destination => destination.Citas,
                options => options.Ignore());

        CreateMap<Cita, CitaDto>();
        CreateMap<CitaCreateDto, Cita>();
        CreateMap<CitaUpdateDto, Cita>();

        CreateMap<Diagnostico, DiagnosticoDto>();

        CreateMap<DiagnosticoCreateDto, Diagnostico>()
            .ForMember(
                destination => destination.Cita,
                options => options.Ignore());

        CreateMap<DiagnosticoUpdateDto, Diagnostico>()
            .ForMember(
                destination => destination.Cita,
                options => options.Ignore());
    }
}