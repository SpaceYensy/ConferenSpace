namespace ConferenSpace.UI.Models;

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
    public string Estado { get; set; } = string.Empty;
    public string? Notas { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
    public List<ReservaRecursoDto> Recursos { get; set; } = new();
}

public class ReservaRecursoDto
{
    public int RecursoId { get; set; }
    public Recurso? Recurso { get; set; }
    public int CantidadSolicitada { get; set; }
}