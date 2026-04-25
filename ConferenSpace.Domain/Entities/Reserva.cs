namespace ConferenSpace.Domain.Entities;

public class Reserva
{
    public int Id { get; set; }

    public int SolicitanteId { get; set; }

    public Solicitante? Solicitante { get; set; }

    public int SalonId { get; set; }

    public Salon? Salon { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public string Proposito { get; set; } = string.Empty;

    public int CantidadAsistentes { get; set; }

    public EstadoReserva Estado { get; set; } = EstadoReserva.Confirmada;

    public string? Notas { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public DateTime? FechaModificacion { get; set; }

    public ICollection<ReservaRecurso> ReservaRecursos { get; set; } = new List<ReservaRecurso>();
}

public enum EstadoReserva
{
    Pendiente = 0,
    Confirmada = 1,
    Cancelada = 2
}
