using ConferenSpace.Application.DTOs;

namespace ConferenSpace.Application.Contracts;

public interface IReservaService
{
    Task<IEnumerable<ReservaDTO>> ObtenerTodosAsync();

    Task<ReservaDTO?> ObtenerPorIdAsync(int id);

    Task<ReservaDTO> CrearAsync(ReservaCrearDTO reservaCrearDTO);

    Task<ReservaDTO> ActualizarAsync(int id, ReservaCrearDTO reservaCrearDTO);

    Task<bool> CancelarAsync(int id);

    Task<IEnumerable<ReservaDTO>> ObtenerPorSolicitanteAsync(int solicitanteId);

    Task<IEnumerable<ReservaDTO>> ObtenerPorSalonYFechaAsync(int salonId, DateTime fecha);

    Task<IEnumerable<DisponibilidadDTO>> ObtenerDisponibilidadSalonAsync(int salonId, DateTime fechaInicio, DateTime fechaFin);

    Task<bool> ValidarDisponibilidadAsync(int salonId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, List<(int recursoId, int cantidad)>? recursosRequeridos = null);
}
