namespace ConferenSpace.Application.DTOs;

/// <summary>
/// Data Transfer Object para información de recurso en una reserva.
/// </summary>
public class ReservaRecursoDTO
{
    public int RecursoId { get; set; }

    public RecursoDTO? Recurso { get; set; }

    public int CantidadSolicitada { get; set; }
}

/// <summary>
/// Data Transfer Object para disponibilidad de un salón.
/// </summary>
public class DisponibilidadDTO
{
    public int SalonId { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public bool EstaDisponible { get; set; }

    public string? Razon { get; set; }
}
