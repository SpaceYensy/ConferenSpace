using AutoMapper;
using ConferenSpace.Application.Contracts;
using ConferenSpace.Application.DTOs;
using ConferenSpace.Domain.Entities;
using ConferenSpace.Infrastructure.Contracts;

namespace ConferenSpace.Application.Services;

/// <summary>
/// Servicio de lógica de negocios para la gestión de salones.
/// </summary>
public class SalonService : ISalonService
{
    private readonly ISalonRepository _salonRepository;
    private readonly IReservaRepository _reservaRepository;
    private readonly IMapper _mapper;

    public SalonService(ISalonRepository salonRepository, IReservaRepository reservaRepository, IMapper mapper)
    {
        _salonRepository = salonRepository;
        _reservaRepository = reservaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SalonDTO>> ObtenerTodosAsync()
    {
        var salones = await _salonRepository.ObtenerTodosAsync();
        return _mapper.Map<IEnumerable<SalonDTO>>(salones);
    }

    public async Task<SalonDTO?> ObtenerPorIdAsync(int id)
    {
        var salon = await _salonRepository.ObtenerPorIdAsync(id);
        return _mapper.Map<SalonDTO?>(salon);
    }

    public async Task<SalonDTO> CrearAsync(SalonDTO salonDTO)
    {
        var salon = _mapper.Map<Salon>(salonDTO);
        salon.FechaCreacion = DateTime.UtcNow;
        
        var salonCreado = await _salonRepository.CrearAsync(salon);
        return _mapper.Map<SalonDTO>(salonCreado);
    }

    public async Task<SalonDTO> ActualizarAsync(int id, SalonDTO salonDTO)
    {
        var salon = await _salonRepository.ObtenerPorIdAsync(id);
        if (salon == null)
            throw new ArgumentException($"Salón con ID {id} no encontrado.");

        _mapper.Map(salonDTO, salon);
        
        var salonActualizado = await _salonRepository.ActualizarAsync(salon);
        return _mapper.Map<SalonDTO>(salonActualizado);
    }

    public async Task<bool> PonerFueraDeServicioAsync(int id)
    {
        var salon = await _salonRepository.ObtenerPorIdAsync(id);
        if (salon == null)
            throw new ArgumentException($"Salón con ID {id} no encontrado.");

        salon.EstaActivo = false;
        await _salonRepository.ActualizarAsync(salon);
        return true;
    }

    public async Task<bool> ReactivarAsync(int id)
    {
        var salon = await _salonRepository.ObtenerPorIdAsync(id);
        if (salon == null)
            throw new ArgumentException($"Salón con ID {id} no encontrado.");

        salon.EstaActivo = true;
        await _salonRepository.ActualizarAsync(salon);
        return true;
    }

    public async Task<IEnumerable<SalonDTO>> ObtenerDisponiblesAsync(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int capacidadMinima)
    {
        var todosLosComponentes = await _salonRepository.ObtenerTodosAsync();
        var salonesActivos = todosLosComponentes.Where(s => s.EstaActivo && s.CapacidadMaxima >= capacidadMinima).ToList();

        var salonesDisponibles = new List<Salon>();

        foreach (var salon in salonesActivos)
        {
            var reservasConflicto = await _reservaRepository.ObtenerPorSalonYFechaAsync(salon.Id, fecha);
            var hayConflicto = reservasConflicto.Any(r => 
                r.Estado != EstadoReserva.Cancelada &&
                !(r.HoraFin <= horaInicio || r.HoraInicio >= horaFin));

            if (!hayConflicto)
                salonesDisponibles.Add(salon);
        }

        return _mapper.Map<IEnumerable<SalonDTO>>(salonesDisponibles);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var salon = await _salonRepository.ObtenerPorIdAsync(id);
        if (salon == null)
            throw new ArgumentException($"Salón con ID {id} no encontrado.");

        var tieneReservas = (await _reservaRepository.ObtenerPorSalonYFechaAsync(id, DateTime.MinValue)).Any();
        if (tieneReservas)
            throw new InvalidOperationException($"No se puede eliminar el salón '{salon.Nombre}' porque tiene reservas asociadas.");

        await _salonRepository.EliminarAsync(id);
        return true;
    }
}
