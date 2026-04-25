using ConferenSpace.Application.DTOs;

namespace ConferenSpace.Application.Contracts;

public interface ISalonService
{
    Task<IEnumerable<SalonDTO>> ObtenerTodosAsync();

    Task<SalonDTO?> ObtenerPorIdAsync(int id);

    Task<SalonDTO> CrearAsync(SalonDTO salonDTO);

    Task<SalonDTO> ActualizarAsync(int id, SalonDTO salonDTO);

    Task<bool> PonerFueraDeServicioAsync(int id);

    Task<bool> ReactivarAsync(int id);

    Task<IEnumerable<SalonDTO>> ObtenerDisponiblesAsync(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int capacidadMinima);

    Task<bool> EliminarAsync(int id);
}
