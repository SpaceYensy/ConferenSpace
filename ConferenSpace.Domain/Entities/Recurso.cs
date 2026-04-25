namespace ConferenSpace.Domain.Entities;

public class Recurso
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public int CantidadTotal { get; set; }

    public int CantidadDisponible { get; set; }

    public decimal? CostoUnitario { get; set; }

    public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

    public bool EstaActivo { get; set; } = true;

    public ICollection<ReservaRecurso> ReservaRecursos { get; set; } = new List<ReservaRecurso>();
}
