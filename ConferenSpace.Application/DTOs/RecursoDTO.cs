namespace ConferenSpace.Application.DTOs;

public class RecursoDTO
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public int CantidadTotal { get; set; }

    public int CantidadDisponible { get; set; }

    public decimal? CostoUnitario { get; set; }

    public DateTime FechaCreacion { get; set; }

    public bool EstaActivo { get; set; } = true;
}
