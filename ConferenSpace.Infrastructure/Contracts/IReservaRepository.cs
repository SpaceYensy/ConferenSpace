using ConferenSpace.Domain.Entities;

namespace ConferenSpace.Infrastructure.Contracts;

/// <summary>
/// Contrato del repositorio para la entidad Reserva.
/// </summary>
public interface IReservaRepository
{
    Task<IEnumerable<Reserva>> ObtenerTodosAsync();
    Task<Reserva?> ObtenerPorIdAsync(int id);
    Task<Reserva> CrearAsync(Reserva reserva);
    Task<Reserva> ActualizarAsync(Reserva reserva);
    Task<bool> EliminarAsync(int id);
    Task<IEnumerable<Reserva>> ObtenerPorSolicitanteAsync(int solicitanteId);
    Task<IEnumerable<Reserva>> ObtenerPorSalonYFechaAsync(int salonId, DateTime fecha);
}
