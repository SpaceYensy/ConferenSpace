namespace ConferenSpace.Domain.Entities;

/// <summary>
/// Representa a un solicitante que realiza reservas en el sistema.
/// </summary>
public class Solicitante
{
    public int Id { get; set; }

    /// <summary>
    /// Nombre completo del solicitante.
    /// </summary>
    public string NombreCompleto { get; set; } = string.Empty;

    /// <summary>
    /// Número de teléfono de contacto del solicitante.
    /// </summary>
    public string Telefono { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del solicitante.
    /// </summary>
    public string Correo { get; set; } = string.Empty;

    /// <summary>
    /// Departamento o empresa a la que pertenece el solicitante.
    /// </summary>
    public string Departamento { get; set; } = string.Empty;

    /// <summary>
    /// Número de identificación único del solicitante (cédula, pasaporte, etc.).
    /// </summary>
    public string NumeroIdentificacion { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de creación del registro del solicitante.
    /// </summary>
    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Indica si el solicitante está activo en el sistema.
    /// </summary>
    public bool EstaActivo { get; set; } = true;

    /// <summary>
    /// Relación: Un solicitante puede hacer muchas reservas.
    /// </summary>
    public ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();
}
