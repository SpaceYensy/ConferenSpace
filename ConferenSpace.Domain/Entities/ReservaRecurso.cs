namespace ConferenSpace.Domain.Entities;

public class ReservaRecurso
{
    public int Id { get; set; }

    public int ReservaId { get; set; }

    public Reserva? Reserva { get; set; }

    public int RecursoId { get; set; }

    public Recurso? Recurso { get; set; }

    public int CantidadSolicitada { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
