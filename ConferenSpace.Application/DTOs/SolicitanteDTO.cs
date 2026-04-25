namespace ConferenSpace.Application.DTOs;
public class SolicitanteDTO
{
    public int Id { get; set; }

    public string NombreCompleto { get; set; } = string.Empty;

    public string Telefono { get; set; } = string.Empty;

    public string Correo { get; set; } = string.Empty;

    public string Departamento { get; set; } = string.Empty;

    public string NumeroIdentificacion { get; set; } = string.Empty;

    public DateTime FechaCreacion { get; set; }

    public bool EstaActivo { get; set; } = true;
}
