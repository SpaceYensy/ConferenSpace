namespace ConferenSpace.Domain.Entities;

/// <summary>
/// Representa una reserva de salón para un evento específico.
/// </summary>
public class Reserva
{
    public int Id { get; set; }

    /// <summary>
    /// Identificador del solicitante que realiza la reserva.
    /// </summary>
    public int SolicitanteId { get; set; }

    /// <summary>
    /// Referencia a la entidad Solicitante.
    /// </summary>
    public Solicitante? Solicitante { get; set; }

    /// <summary>
    /// Identificador del salón reservado.
    /// </summary>
    public int SalonId { get; set; }

    /// <summary>
    /// Referencia a la entidad Salon.
    /// </summary>
    public Salon? Salon { get; set; }

    /// <summary>
    /// Fecha del evento (solo la fecha, sin hora).
    /// </summary>
    public DateTime Fecha { get; set; }

    /// <summary>
    /// Hora de inicio de la reserva.
    /// </summary>
    public TimeSpan HoraInicio { get; set; }

    /// <summary>
    /// Hora de fin de la reserva.
    /// </summary>
    public TimeSpan HoraFin { get; set; }

    /// <summary>
    /// Propósito o descripción del evento (ej: "Reunión de junta directiva", "Capacitación técnica").
    /// </summary>
    public string Proposito { get; set; } = string.Empty;

    /// <summary>
    /// Cantidad de asistentes esperados en el evento.
    /// </summary>
    public int CantidadAsistentes { get; set; }

    /// <summary>
    /// Estado de la reserva (Pendiente, Confirmada, Cancelada).
    /// </summary>
    public EstadoReserva Estado { get; set; } = EstadoReserva.Confirmada;

    /// <summary>
    /// Notas o comentarios adicionales sobre la reserva.
    /// </summary>
    public string? Notas { get; set; }

    /// <summary>
    /// Fecha de creación de la reserva en el sistema.
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de la última modificación de la reserva.
    /// </summary>
    public DateTime? FechaModificacion { get; set; }

    /// <summary>
    /// Relación: Una reserva puede tener muchos recursos.
    /// </summary>
    public ICollection<ReservaRecurso> ReservaRecursos { get; set; } = new List<ReservaRecurso>();
}

/// <summary>
/// Enumeración que define los posibles estados de una reserva.
/// </summary>
public enum EstadoReserva
{
    Pendiente = 0,
    Confirmada = 1,
    Cancelada = 2
}
