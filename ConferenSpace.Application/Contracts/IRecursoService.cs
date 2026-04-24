using ConferenSpace.Application.DTOs;

namespace ConferenSpace.Application.Contracts;

/// <summary>
/// Contrato para operaciones de gestión de recursos.
/// </summary>
public interface IRecursoService
{
    /// <summary>
    /// Obtiene todos los recursos registrados en el sistema.
    /// </summary>
    Task<IEnumerable<RecursoDTO>> ObtenerTodosAsync();

    /// <summary>
    /// Obtiene un recurso específico por su identificador.
    /// </summary>
    Task<RecursoDTO?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Crea un nuevo recurso en el sistema.
    /// </summary>
    Task<RecursoDTO> CrearAsync(RecursoDTO recursoDTO);

    /// <summary>
    /// Actualiza la información de un recurso existente.
    /// </summary>
    Task<RecursoDTO> ActualizarAsync(int id, RecursoDTO recursoDTO);

    /// <summary>
    /// Obtiene la cantidad disponible de un recurso específico.
    /// </summary>
    Task<int> ObtenerCantidadDisponibleAsync(int recursoId);

    /// <summary>
    /// Obtiene los recursos disponibles en una fecha y rango horario específico con cantidad mínima.
    /// </summary>
    Task<IEnumerable<RecursoDTO>> ObtenerDisponiblesAsync(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int cantidadMinima);

    /// <summary>
    /// Desactiva un recurso.
    /// </summary>
    Task<bool> DesactivarAsync(int id);

    /// <summary>
    /// Reactiva un recurso que estaba desactivado.
    /// </summary>
    Task<bool> ReactivarAsync(int id);

    /// <summary>
    /// Elimina un recurso del sistema.
    /// </summary>
    Task<bool> EliminarAsync(int id);
}
