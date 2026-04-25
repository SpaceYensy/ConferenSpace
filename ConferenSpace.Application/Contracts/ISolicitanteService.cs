using ConferenSpace.Application.DTOs;

namespace ConferenSpace.Application.Contracts;

public interface ISolicitanteService
{
    Task<IEnumerable<SolicitanteDTO>> ObtenerTodosAsync();

    Task<SolicitanteDTO?> ObtenerPorIdAsync(int id);

    Task<SolicitanteDTO> CrearAsync(SolicitanteDTO solicitanteDTO);

    Task<SolicitanteDTO> ActualizarAsync(int id, SolicitanteDTO solicitanteDTO);

    Task<IEnumerable<SolicitanteDTO>> BuscarPorNombreAsync(string nombre);

    Task<IEnumerable<SolicitanteDTO>> BuscarPorDepartamentoAsync(string departamento);

    Task<SolicitanteDTO?> BuscarPorCorreoAsync(string correo);

    Task<bool> DesactivarAsync(int id);

    Task<bool> EliminarAsync(int id);
}
