namespace ConferenSpace.Application.DTOs;

/// <summary>
/// Data Transfer Object para crear una nueva reserva.
/// </summary>
public class ReservaCrearDTO
{
    public int SolicitanteId { get; set; }

    public int SalonId { get; set; }

    public DateTime Fecha { get; set; }

    public TimeSpan HoraInicio { get; set; }

    public TimeSpan HoraFin { get; set; }

    public string Proposito { get; set; } = string.Empty;

    public int CantidadAsistentes { get; set; }

    public string? Notas { get; set; }

    /// <summary>
    /// Lista de recursos requeridos con sus cantidades.
    /// Formato: (RecursoId, CantidadSolicitada)
    /// </summary>
    public List<ReservaRecursoCrearDTO> Recursos { get; set; } = new();
}

/// <summary>
/// Data Transfer Object para especificar recursos en una reserva.
/// </summary>
public class ReservaRecursoCrearDTO
{
    public int RecursoId { get; set; }

    public int CantidadSolicitada { get; set; }
}
