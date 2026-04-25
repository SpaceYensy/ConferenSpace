namespace ConferenSpace.Application.DTOs;

public class SalonDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string Ubicacion { get; set; } = string.Empty;

    public int CapacidadMaxima { get; set; }

    public string? ServiciosIntegrados { get; set; }

    public bool EstaActivo { get; set; } = true;

    public DateTime FechaCreacion { get; set; }
}
