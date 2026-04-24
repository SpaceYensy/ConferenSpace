namespace ConferenSpace.Domain.Entities;

/// <summary>
/// Representa un salón de conferencias disponible en el sistema.
/// </summary>
public class Salon
{
    public int Id { get; set; }

    /// <summary>
    /// Nombre del salón (ej: "Sala A", "Auditorio Principal").
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Ubicación del salón (ej: "Piso 3, Ala Oeste").
    /// </summary>
    public string Ubicacion { get; set; } = string.Empty;

    /// <summary>
    /// Capacidad máxima de personas que puede albergar.
    /// </summary>
    public int CapacidadMaxima { get; set; }

    /// <summary>
    /// Descripción de servicios integrados en el salón (ej: "Proyector, Pizarra digital, Videoconferencia").
    /// </summary>
    public string? ServiciosIntegrados { get; set; }

    /// <summary>
    /// Indica si el salón está en servicio o fuera de servicio temporalmente.
    /// </summary>
    public bool EstaActivo { get; set; } = true;

    /// <summary>
    /// Fecha de creación del registro del salón.
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Relación: Un salón puede tener muchas reservas.
    /// </summary>
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
