namespace ConferenSpace.UI.Models;

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
    public List<ReservaRecursoCrearDTO> Recursos { get; set; } = new();
}

public class ReservaRecursoCrearDTO
{
    public int RecursoId { get; set; }
    public int CantidadSolicitada { get; set; }
}