using AutoMapper;
using ConferenSpace.Application.DTOs;
using ConferenSpace.Domain.Entities;

namespace ConferenSpace.Application.MappingProfiles;

/// <summary>
/// Perfil de mapeo de AutoMapper para las entidades de ConferenSpace.
/// </summary>
public class ConferenSpaceMappingProfile : Profile
{
    public ConferenSpaceMappingProfile()
    {
        // Mapeo: Salon
        CreateMap<Salon, SalonDTO>().ReverseMap();

        // Mapeo: Solicitante
        CreateMap<Solicitante, SolicitanteDTO>().ReverseMap();

        // Mapeo: Recurso
        CreateMap<Recurso, RecursoDTO>().ReverseMap();

        // Mapeo: Reserva
        CreateMap<Reserva, ReservaDTO>()
            .ForMember(d => d.Recursos, opt => opt.MapFrom(s => s.ReservaRecursos))
            .ReverseMap()
            .ForMember(d => d.ReservaRecursos, opt => opt.Ignore());

        // Mapeo: ReservaRecurso
        CreateMap<ReservaRecurso, ReservaRecursoDTO>().ReverseMap();
    }
}
