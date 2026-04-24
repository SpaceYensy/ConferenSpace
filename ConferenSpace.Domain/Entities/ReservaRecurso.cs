namespace ConferenSpace.Domain.Entities;

/// <summary>
/// Entidad de unión (tabla de relación) entre Reserva y Recurso.
/// Permite que una reserva tenga múltiples recursos con cantidades específicas.
/// </summary>
public class ReservaRecurso
{
    public int Id { get; set; }

    /// <summary>
    /// Identificador de la reserva.
    /// </summary>
    public int ReservaId { get; set; }

    /// <summary>
    /// Referencia a la entidad Reserva.
    /// </summary>
    public Reserva? Reserva { get; set; }

    /// <summary>
    /// Identificador del recurso.
    /// </summary>
    public int RecursoId { get; set; }

    /// <summary>
    /// Referencia a la entidad Recurso.
    /// </summary>
    public Recurso? Recurso { get; set; }

    /// <summary>
    /// Cantidad del recurso solicitado para esta reserva.
    /// </summary>
    public int CantidadSolicitada { get; set; }

    /// <summary>
    /// Fecha de creación del registro.
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
}
