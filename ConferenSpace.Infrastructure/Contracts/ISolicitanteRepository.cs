using ConferenSpace.Domain.Entities;

namespace ConferenSpace.Infrastructure.Contracts;

public interface ISolicitanteRepository
{
    Task<IEnumerable<Solicitante>> ObtenerTodosAsync();
    Task<Solicitante?> ObtenerPorIdAsync(int id);
    Task<Solicitante> CrearAsync(Solicitante solicitante);
    Task<Solicitante> ActualizarAsync(Solicitante solicitante);
    Task<bool> EliminarAsync(int id);
}
