using ConferenSpace.Application.DTOs;

namespace ConferenSpace.Application.Contracts;

/// <summary>
/// Contrato para operaciones de gestión de solicitantes.
/// </summary>
public interface ISolicitanteService
{
    /// <summary>
    /// Obtiene todos los solicitantes registrados en el sistema.
    /// </summary>
    Task<IEnumerable<SolicitanteDTO>> ObtenerTodosAsync();

    /// <summary>
    /// Obtiene un solicitante específico por su identificador.
    /// </summary>
    Task<SolicitanteDTO?> ObtenerPorIdAsync(int id);

    /// <summary>
    /// Crea un nuevo solicitante en el sistema.
    /// </summary>
    Task<SolicitanteDTO> CrearAsync(SolicitanteDTO solicitanteDTO);

    /// <summary>
    /// Actualiza la información de un solicitante existente.
    /// </summary>
    Task<SolicitanteDTO> ActualizarAsync(int id, SolicitanteDTO solicitanteDTO);

    /// <summary>
    /// Busca solicitantes por nombre.
    /// </summary>
    Task<IEnumerable<SolicitanteDTO>> BuscarPorNombreAsync(string nombre);

    /// <summary>
    /// Busca solicitantes por departamento.
    /// </summary>
    Task<IEnumerable<SolicitanteDTO>> BuscarPorDepartamentoAsync(string departamento);

    /// <summary>
    /// Busca un solicitante por correo electrónico.
    /// </summary>
    Task<SolicitanteDTO?> BuscarPorCorreoAsync(string correo);

    /// <summary>
    /// Desactiva un solicitante (solo si no tiene reservas activas).
    /// </summary>
    Task<bool> DesactivarAsync(int id);

    /// <summary>
    /// Elimina un solicitante del sistema (solo si no tiene reservas).
    /// </summary>
    Task<bool> EliminarAsync(int id);
}
