using ConferenSpace.Application.DTOs;

namespace ConferenSpace.Application.Contracts;

/// <summary>
/// Contrato para operaciones de gestión de reservas.
/// </summary>
public interface IReservaService
{
    /// <summary>
    /// Obtiene todas las reservas registradas en el sistema.
    /// </summary>
    Task<IEnumerable<ReservaDTO>> ObtenerTodosAsync();

    /// <summary>
    /// Obtiene una reserva específica por su identificador.
    /// </summary>
    Task<ReservaDTO?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Crea una nueva reserva en el sistema con validación de disponibilidad.
    /// </summary>
    Task<ReservaDTO> CrearAsync(ReservaCrearDTO reservaCrearDTO);

    /// <summary>
    /// Actualiza una reserva existente (fecha, hora, propósito, recursos).
    /// </summary>
    Task<ReservaDTO> ActualizarAsync(int id, ReservaCrearDTO reservaCrearDTO);

    /// <summary>
    /// Cancela una reserva, liberando el salón y recursos.
    /// </summary>
    Task<bool> CancelarAsync(int id);

    /// <summary>
    /// Obtiene las reservas de un solicitante específico.
    /// </summary>
    Task<IEnumerable<ReservaDTO>> ObtenerPorSolicitanteAsync(int solicitanteId);

    /// <summary>
    /// Obtiene las reservas de un salón en una fecha específica.
    /// </summary>
    Task<IEnumerable<ReservaDTO>> ObtenerPorSalonYFechaAsync(int salonId, DateTime fecha);

    /// <summary>
    /// Obtiene la disponibilidad de un salón en un rango de fechas.
    /// </summary>
    Task<IEnumerable<DisponibilidadDTO>> ObtenerDisponibilidadSalonAsync(int salonId, DateTime fechaInicio, DateTime fechaFin);

    /// <summary>
    /// Valida si un salón y los recursos están disponibles para una reserva.
    /// </summary>
    Task<bool> ValidarDisponibilidadAsync(int salonId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, List<(int recursoId, int cantidad)>? recursosRequeridos = null);
}
