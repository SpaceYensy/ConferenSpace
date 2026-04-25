using AutoMapper;
using ConferenSpace.Application.DTOs;
using ConferenSpace.Domain.Entities;

namespace ConferenSpace.Application.MappingProfiles;

public class ConferenSpaceMappingProfile : Profile
{
    public ConferenSpaceMappingProfile()
    {
        CreateMap<Salon, SalonDTO>().ReverseMap();

        CreateMap<Solicitante, SolicitanteDTO>().ReverseMap();

        CreateMap<Recurso, RecursoDTO>().ReverseMap();

        CreateMap<Reserva, ReservaDTO>()
            .ForMember(d => d.Recursos, opt => opt.MapFrom(s => s.ReservaRecursos))
            .ReverseMap()
            .ForMember(d => d.ReservaRecursos, opt => opt.Ignore());

        CreateMap<ReservaRecurso, ReservaRecursoDTO>().ReverseMap();
    }
}
