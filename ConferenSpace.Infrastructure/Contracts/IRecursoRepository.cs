using ConferenSpace.Domain.Entities;

namespace ConferenSpace.Infrastructure.Contracts;

public interface IRecursoRepository
{
    Task<IEnumerable<Recurso>> ObtenerTodosAsync();
    Task<Recurso?> ObtenerPorIdAsync(int id);
    Task<Recurso> CrearAsync(Recurso recurso);
    Task<Recurso> ActualizarAsync(Recurso recurso);
    Task<bool> EliminarAsync(int id);
}
