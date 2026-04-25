using ConferenSpace.Application.DTOs;

namespace ConferenSpace.Application.Contracts;

public interface IRecursoService
{
    Task<IEnumerable<RecursoDTO>> ObtenerTodosAsync();

    Task<RecursoDTO?> ObtenerPorIdAsync(int id);

    Task<RecursoDTO> CrearAsync(RecursoDTO recursoDTO);

    Task<RecursoDTO> ActualizarAsync(int id, RecursoDTO recursoDTO);

    Task<int> ObtenerCantidadDisponibleAsync(int recursoId);

    Task<IEnumerable<RecursoDTO>> ObtenerDisponiblesAsync(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int cantidadMinima);

    Task<bool> DesactivarAsync(int id);

    Task<bool> ReactivarAsync(int id);

    Task<bool> EliminarAsync(int id);
}
