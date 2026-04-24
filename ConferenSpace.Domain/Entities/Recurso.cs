namespace ConferenSpace.Domain.Entities;

/// <summary>
/// Representa recursos adicionales disponibles para reservas (proyectores, micrófonos, catering, etc.).
/// </summary>
public class Recurso
{
    public int Id { get; set; }

    /// <summary>
    /// Nombre del recurso (ej: "Proyector portátil", "Micrófono", "Catering para 50 personas").
    /// </summary>
    public string Nombre { get; set; } = string.Empty;

    /// <summary>
    /// Descripción detallada del recurso.
    /// </summary>
    public string? Descripcion { get; set; }

    /// <summary>
    /// Cantidad total disponible de este recurso.
    /// </summary>
    public int CantidadTotal { get; set; }

    /// <summary>
    /// Cantidad actualmente disponible (sin comprometida en reservas).
    /// </summary>
    public int CantidadDisponible { get; set; }

    /// <summary>
    /// Valor en dinero o costo de cada unidad del recurso (opcional).
    /// </summary>
    public decimal? CostoUnitario { get; set; }

    /// <summary>
    /// Fecha de creación del registro del recurso.
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indica si el recurso está disponible para ser reservado.
    /// </summary>
    public bool EstaActivo { get; set; } = true;

    /// <summary>
    /// Relación: Un recurso puede estar en muchas reservas.
    /// </summary>
    public ICollection<ReservaRecurso> ReservaRecursos { get; set; } = new List<ReservaRecurso>();
}
