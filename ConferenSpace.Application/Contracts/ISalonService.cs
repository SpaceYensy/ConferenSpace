using ConferenSpace.Application.DTOs;

namespace ConferenSpace.Application.Contracts;

/// <summary>
/// Contrato para operaciones de gestión de salones.
/// </summary>
public interface ISalonService
{
    /// <summary>
    /// Obtiene todos los salones registrados en el sistema.
    /// </summary>
    Task<IEnumerable<SalonDTO>> ObtenerTodosAsync();

    /// <summary>
    /// Obtiene un salón específico por su identificador.
    /// </summary>
    Task<SalonDTO?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Crea un nuevo salón en el sistema.
    /// </summary>
    Task<SalonDTO> CrearAsync(SalonDTO salonDTO);

    /// <summary>
    /// Actualiza la información de un salón existente.
    /// </summary>
    Task<SalonDTO> ActualizarAsync(int id, SalonDTO salonDTO);

    /// <summary>
    /// Pone un salón fuera de servicio temporalmente.
    /// </summary>
    Task<bool> PonerFueraDeServicioAsync(int id);

    /// <summary>
    /// Reactiva un salón que estaba fuera de servicio.
    /// </summary>
    Task<bool> ReactivarAsync(int id);

    /// <summary>
    /// Obtiene los salones disponibles para una fecha y rango horario específico.
    /// </summary>
    Task<IEnumerable<SalonDTO>> ObtenerDisponiblesAsync(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int capacidadMinima);

    /// <summary>
    /// Elimina un salón del sistema.
    /// </summary>
    Task<bool> EliminarAsync(int id);
}
