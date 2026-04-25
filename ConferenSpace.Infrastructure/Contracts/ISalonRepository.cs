using ConferenSpace.Domain.Entities;

namespace ConferenSpace.Infrastructure.Contracts;

public interface ISalonRepository
{
    Task<IEnumerable<Salon>> ObtenerTodosAsync();
    Task<Salon?> ObtenerPorIdAsync(int id);
    Task<Salon> CrearAsync(Salon salon);
    Task<Salon> ActualizarAsync(Salon salon);
    Task<bool> EliminarAsync(int id);
}
