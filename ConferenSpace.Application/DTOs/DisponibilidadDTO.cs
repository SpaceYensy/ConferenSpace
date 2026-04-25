namespace ConferenSpace.Application.DTOs;

public class ReservaRecursoDTO
{
    public int RecursoId { get; set; }

    public RecursoDTO? Recurso { get; set; }

    public int CantidadSolicitada { get; set; }
}

public class DisponibilidadDTO
{
    public int SalonId { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public bool EstaDisponible { get; set; }

    public string? Razon { get; set; }
}
