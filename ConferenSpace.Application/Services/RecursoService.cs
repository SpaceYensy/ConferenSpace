using AutoMapper;
using ConferenSpace.Application.Contracts;
using ConferenSpace.Application.DTOs;
using ConferenSpace.Domain.Entities;
using ConferenSpace.Infrastructure.Contracts;

namespace ConferenSpace.Application.Services;

/// <summary>
/// Servicio de lógica de negocios para la gestión de recursos.
/// </summary>
public class RecursoService : IRecursoService
{
    private readonly IRecursoRepository _recursoRepository;
    private readonly IReservaRepository _reservaRepository;
    private readonly IMapper _mapper;

    public RecursoService(IRecursoRepository recursoRepository, IReservaRepository reservaRepository, IMapper mapper)
    {
        _recursoRepository = recursoRepository;
        _reservaRepository = reservaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<RecursoDTO>> ObtenerTodosAsync()
    {
        var recursos = await _recursoRepository.ObtenerTodosAsync();
        return _mapper.Map<IEnumerable<RecursoDTO>>(recursos);
    }

    public async Task<RecursoDTO?> ObtenerPorIdAsync(int id)
    {
        var recurso = await _recursoRepository.ObtenerPorIdAsync(id);
        return _mapper.Map<RecursoDTO?>(recurso);
    }

    public async Task<RecursoDTO> CrearAsync(RecursoDTO recursoDTO)
    {
        var recurso = _mapper.Map<Recurso>(recursoDTO);
        recurso.FechaCreacion = DateTime.UtcNow;
        recurso.CantidadDisponible = recurso.CantidadTotal;

        var recursoCreado = await _recursoRepository.CrearAsync(recurso);
        return _mapper.Map<RecursoDTO>(recursoCreado);
    }

    public async Task<RecursoDTO> ActualizarAsync(int id, RecursoDTO recursoDTO)
    {
        var recurso = await _recursoRepository.ObtenerPorIdAsync(id);
        if (recurso == null)
            throw new ArgumentException($"Recurso con ID {id} no encontrado.");

        var diferencia = recursoDTO.CantidadTotal - recurso.CantidadTotal;
        recurso.CantidadDisponible += diferencia;

        _mapper.Map(recursoDTO, recurso);

        var recursoActualizado = await _recursoRepository.ActualizarAsync(recurso);
        return _mapper.Map<RecursoDTO>(recursoActualizado);
    }

    public async Task<int> ObtenerCantidadDisponibleAsync(int recursoId)
    {
        var recurso = await _recursoRepository.ObtenerPorIdAsync(recursoId);
        if (recurso == null)
            throw new ArgumentException($"Recurso con ID {recursoId} no encontrado.");

        return recurso.CantidadDisponible;
    }

    public async Task<IEnumerable<RecursoDTO>> ObtenerDisponiblesAsync(DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, int cantidadMinima)
    {
        var todosLosRecursos = await _recursoRepository.ObtenerTodosAsync();
        var recursosDisponibles = new List<Recurso>();

        foreach (var recurso in todosLosRecursos.Where(r => r.EstaActivo))
        {
            var reservasConFlicto = await _reservaRepository.ObtenerTodosAsync();
            var cantidadComprometida = 0;

            foreach (var reserva in reservasConFlicto.Where(r => 
                r.Estado != EstadoReserva.Cancelada && 
                r.Fecha == fecha &&
                !(r.HoraFin <= horaInicio || r.HoraInicio >= horaFin)))
            {
                var recursoEnReserva = reserva.ReservaRecursos.FirstOrDefault(rr => rr.RecursoId == recurso.Id);
                if (recursoEnReserva != null)
                    cantidadComprometida += recursoEnReserva.CantidadSolicitada;
            }

            var disponibleReal = recurso.CantidadTotal - cantidadComprometida;
            if (disponibleReal >= cantidadMinima)
                recursosDisponibles.Add(recurso);
        }

        return _mapper.Map<IEnumerable<RecursoDTO>>(recursosDisponibles);
    }

    public async Task<bool> DesactivarAsync(int id)
    {
        var recurso = await _recursoRepository.ObtenerPorIdAsync(id);
        if (recurso == null)
            throw new ArgumentException($"Recurso con ID {id} no encontrado.");

        recurso.EstaActivo = false;
        await _recursoRepository.ActualizarAsync(recurso);
        return true;
    }

    public async Task<bool> ReactivarAsync(int id)
    {
        var recurso = await _recursoRepository.ObtenerPorIdAsync(id);
        if (recurso == null)
            throw new ArgumentException($"Recurso con ID {id} no encontrado.");

        recurso.EstaActivo = true;
        await _recursoRepository.ActualizarAsync(recurso);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var recurso = await _recursoRepository.ObtenerPorIdAsync(id);
        if (recurso == null)
            throw new ArgumentException($"Recurso con ID {id} no encontrado.");

        var todasLasReservas = await _reservaRepository.ObtenerTodosAsync();
        var tieneUsoEnReservas = todasLasReservas.Any(r => r.ReservaRecursos.Any(rr => rr.RecursoId == id));

        if (tieneUsoEnReservas)
            throw new InvalidOperationException($"No se puede eliminar el recurso '{recurso.Nombre}' porque está siendo usado en reservas.");

        await _recursoRepository.EliminarAsync(id);
        return true;
    }
}
