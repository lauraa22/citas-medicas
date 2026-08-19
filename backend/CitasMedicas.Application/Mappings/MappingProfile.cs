using AutoMapper;
using CitasMedicas.Application.Models;
using CitasMedicas.Domain.Entities;

namespace CitasMedicas.Application.Mappings;

/// <summary>
/// Configuración de AutoMapper entre entidades de dominio
/// y modelos de aplicación.
/// </summary>
public class MappingProfile : Profile{
    public MappingProfile()
    {
        CreateMap<Usuario, UsuarioModel>()
            .ForMember(
                destination => destination.Usuario,
                options => options.MapFrom(
                    source => source.NombreUsuario))
            .ForMember(
                destination => destination.Clave,
                options => options.Ignore());

        CreateMap<UsuarioModel, Usuario>()
            .ForMember(
                destination => destination.Id,
                options => options.Ignore())
            .ForMember(
                destination => destination.NombreUsuario,
                options => options.MapFrom(
                    source => source.Usuario));



        CreateMap<Paciente, PacienteModel>()
            .ForMember(
                destination => destination.Usuario,
                options => options.MapFrom(
                    source => source.NombreUsuario))
            .ForMember(
                destination => destination.Clave,
                options => options.Ignore())
            .ForMember(
                destination => destination.MedicoIds,
                options => options.MapFrom(
                    source => source.Medicos.Select(
                        medico => medico.Id)));

        CreateMap<PacienteModel, Paciente>()
            .ForMember(
                destination => destination.Id,
                options => options.Ignore())
            .ForMember(
                destination => destination.NombreUsuario,
                options => options.MapFrom(
                    source => source.Usuario))
            .ForMember(
                destination => destination.Medicos,
                options => options.Ignore())
            .ForMember(
                destination => destination.Citas,
                options => options.Ignore());



        CreateMap<Medico, MedicoModel>()
            .ForMember(
                destination => destination.Usuario,
                options => options.MapFrom(
                    source => source.NombreUsuario))
            .ForMember(
                destination => destination.Clave,
                options => options.Ignore())
            .ForMember(
                destination => destination.PacienteIds,
                options => options.MapFrom(
                    source => source.Pacientes.Select(
                        paciente => paciente.Id)));

        CreateMap<MedicoModel, Medico>()
            .ForMember(
                destination => destination.Id,
                options => options.Ignore())
            .ForMember(
                destination => destination.NombreUsuario,
                options => options.MapFrom(
                    source => source.Usuario))
            .ForMember(
                destination => destination.Pacientes,
                options => options.Ignore())
            .ForMember(
                destination => destination.Citas,
                options => options.Ignore());



        CreateMap<Cita, CitaModel>();

        CreateMap<CitaModel, Cita>()
            .ForMember(
                destination => destination.Id,
                options => options.Ignore())
            .ForMember(
                destination => destination.Paciente,
                options => options.Ignore())
            .ForMember(
                destination => destination.Medico,
                options => options.Ignore())
            .ForMember(
                destination => destination.Diagnostico,
                options => options.Ignore());



        CreateMap<Diagnostico, DiagnosticoModel>();

        CreateMap<DiagnosticoModel, Diagnostico>()
            .ForMember(
                destination => destination.Id,
                options => options.Ignore())
            .ForMember(
                destination => destination.Cita,
                options => options.Ignore());
    }
}