using ConferenSpace.Domain.Entities;

namespace ConferenSpace.Application.DTOs;

public class ReservaDTO
{
    public int Id { get; set; }

    public int SolicitanteId { get; set; }

    public SolicitanteDTO? Solicitante { get; set; }

    public int SalonId { get; set; }

    public SalonDTO? Salon { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public string Proposito { get; set; } = string.Empty;

    public int CantidadAsistentes { get; set; }

    public EstadoReserva Estado { get; set; }

    public string? Notas { get; set; }

    public DateTime FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public List<ReservaRecursoDTO> Recursos { get; set; } = new();
}
