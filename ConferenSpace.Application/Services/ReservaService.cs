using AutoMapper;
using ConferenSpace.Application.Contracts;
using ConferenSpace.Application.DTOs;
using ConferenSpace.Domain.Entities;
using ConferenSpace.Infrastructure.Contracts;

namespace ConferenSpace.Application.Services;

/// <summary>
/// Servicio de lógica de negocios para la gestión de reservas.
/// Este es el núcleo del sistema, encargado de validar disponibilidad y gestionar conflictos.
/// </summary>
public class ReservaService : IReservaService
{
    private readonly IReservaRepository _reservaRepository;
    private readonly ISalonRepository _salonRepository;
    private readonly ISolicitanteRepository _solicitanteRepository;
    private readonly IRecursoRepository _recursoRepository;
    private readonly IMapper _mapper;

    public ReservaService(
        IReservaRepository reservaRepository,
        ISalonRepository salonRepository,
        ISolicitanteRepository solicitanteRepository,
        IRecursoRepository recursoRepository,
        IMapper mapper)
    {
        _reservaRepository = reservaRepository;
        _salonRepository = salonRepository;
        _solicitanteRepository = solicitanteRepository;
        _recursoRepository = recursoRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ReservaDTO>> ObtenerTodosAsync()
    {
        var reservas = await _reservaRepository.ObtenerTodosAsync();
        return _mapper.Map<IEnumerable<ReservaDTO>>(reservas);
    }

    public async Task<ReservaDTO?> ObtenerPorIdAsync(int id)
    {
        var reserva = await _reservaRepository.ObtenerPorIdAsync(id);
        return _mapper.Map<ReservaDTO?>(reserva);
    }

    public async Task<ReservaDTO> CrearAsync(ReservaCrearDTO reservaCrearDTO)
    {
        // Validar que el salón existe
        var salon = await _salonRepository.ObtenerPorIdAsync(reservaCrearDTO.SalonId);
        if (salon == null || !salon.EstaActivo)
            throw new ArgumentException("El salón seleccionado no existe o está fuera de servicio.");

        // Validar que el solicitante existe
        var solicitante = await _solicitanteRepository.ObtenerPorIdAsync(reservaCrearDTO.SolicitanteId);
        if (solicitante == null || !solicitante.EstaActivo)
            throw new ArgumentException("El solicitante seleccionado no existe o está inactivo.");

        // Validar que la cantidad de asistentes no excede la capacidad
        if (reservaCrearDTO.CantidadAsistentes > salon.CapacidadMaxima)
            throw new InvalidOperationException($"La cantidad de asistentes ({reservaCrearDTO.CantidadAsistentes}) excede la capacidad del salón ({salon.CapacidadMaxima}).");

        // Validar que la hora de fin es mayor a la hora de inicio
        if (reservaCrearDTO.HoraFin <= reservaCrearDTO.HoraInicio)
            throw new InvalidOperationException("La hora de fin debe ser mayor a la hora de inicio.");

        // Validar disponibilidad del salón
        if (!await ValidarDisponibilidadAsync(
            reservaCrearDTO.SalonId,
            reservaCrearDTO.Fecha,
            reservaCrearDTO.HoraInicio,
            reservaCrearDTO.HoraFin))
        {
            throw new InvalidOperationException("El salón no está disponible en el horario solicitado.");
        }

        // Validar disponibilidad de recursos
        if (reservaCrearDTO.Recursos.Any())
        {
            foreach (var recursoSolicitado in reservaCrearDTO.Recursos)
            {
                var recurso = await _recursoRepository.ObtenerPorIdAsync(recursoSolicitado.RecursoId);
                if (recurso == null || !recurso.EstaActivo)
                    throw new ArgumentException($"El recurso con ID {recursoSolicitado.RecursoId} no existe o no está disponible.");

                var disponible = await ObtenerCantidadDisponibleEnHorarioAsync(
                    recursoSolicitado.RecursoId,
                    reservaCrearDTO.Fecha,
                    reservaCrearDTO.HoraInicio,
                    reservaCrearDTO.HoraFin);

                if (disponible < recursoSolicitado.CantidadSolicitada)
                    throw new InvalidOperationException(
                        $"No hay suficientes {recurso.Nombre}. Disponible: {disponible}, Solicitado: {recursoSolicitado.CantidadSolicitada}");
            }
        }

        // Crear la reserva
        var reserva = new Reserva
        {
            SolicitanteId = reservaCrearDTO.SolicitanteId,
            SalonId = reservaCrearDTO.SalonId,
            Fecha = reservaCrearDTO.Fecha,
            HoraInicio = reservaCrearDTO.HoraInicio,
            HoraFin = reservaCrearDTO.HoraFin,
            Proposito = reservaCrearDTO.Proposito,
            CantidadAsistentes = reservaCrearDTO.CantidadAsistentes,
            Notas = reservaCrearDTO.Notas,
            Estado = EstadoReserva.Confirmada,
            FechaCreacion = DateTime.UtcNow
        };

        // Agregar recursos a la reserva
        foreach (var recursoSolicitado in reservaCrearDTO.Recursos)
        {
            reserva.ReservaRecursos.Add(new ReservaRecurso
            {
                RecursoId = recursoSolicitado.RecursoId,
                CantidadSolicitada = recursoSolicitado.CantidadSolicitada
            });
        }

        var reservaCreada = await _reservaRepository.CrearAsync(reserva);
        return _mapper.Map<ReservaDTO>(reservaCreada);
    }

    public async Task<ReservaDTO> ActualizarAsync(int id, ReservaCrearDTO reservaCrearDTO)
    {
        var reserva = await _reservaRepository.ObtenerPorIdAsync(id);
        if (reserva == null)
            throw new ArgumentException($"Reserva con ID {id} no encontrado.");

        if (reserva.Estado == EstadoReserva.Cancelada)
            throw new InvalidOperationException("No se puede modificar una reserva cancelada.");

        // Validar disponibilidad con los nuevos datos (excluyendo la reserva actual)
        var reservasDelSalon = await _reservaRepository.ObtenerPorSalonYFechaAsync(
            reservaCrearDTO.SalonId, reservaCrearDTO.Fecha);

        var hayConflicto = reservasDelSalon
            .Where(r => r.Id != id && r.Estado != EstadoReserva.Cancelada)
            .Any(r => !(r.HoraFin <= reservaCrearDTO.HoraInicio || r.HoraInicio >= reservaCrearDTO.HoraFin));

        if (hayConflicto)
            throw new InvalidOperationException("El nuevo horario tiene conflictos con otras reservas.");

        reserva.Fecha = reservaCrearDTO.Fecha;
        reserva.HoraInicio = reservaCrearDTO.HoraInicio;
        reserva.HoraFin = reservaCrearDTO.HoraFin;
        reserva.Proposito = reservaCrearDTO.Proposito;
        reserva.CantidadAsistentes = reservaCrearDTO.CantidadAsistentes;
        reserva.Notas = reservaCrearDTO.Notas;
        reserva.FechaModificacion = DateTime.UtcNow;

        // Actualizar recursos
        reserva.ReservaRecursos.Clear();
        foreach (var recursoSolicitado in reservaCrearDTO.Recursos)
        {
            reserva.ReservaRecursos.Add(new ReservaRecurso
            {
                RecursoId = recursoSolicitado.RecursoId,
                CantidadSolicitada = recursoSolicitado.CantidadSolicitada
            });
        }

        var reservaActualizada = await _reservaRepository.ActualizarAsync(reserva);
        return _mapper.Map<ReservaDTO>(reservaActualizada);
    }

    public async Task<bool> CancelarAsync(int id)
    {
        var reserva = await _reservaRepository.ObtenerPorIdAsync(id);
        if (reserva == null)
            throw new ArgumentException($"Reserva con ID {id} no encontrado.");

        reserva.Estado = EstadoReserva.Cancelada;
        reserva.FechaModificacion = DateTime.UtcNow;

        await _reservaRepository.ActualizarAsync(reserva);
        return true;
    }

    public async Task<IEnumerable<ReservaDTO>> ObtenerPorSolicitanteAsync(int solicitanteId)
    {
        var reservas = await _reservaRepository.ObtenerPorSolicitanteAsync(solicitanteId);
        return _mapper.Map<IEnumerable<ReservaDTO>>(reservas);
    }

    public async Task<IEnumerable<ReservaDTO>> ObtenerPorSalonYFechaAsync(int salonId, DateTime fecha)
    {
        var reservas = await _reservaRepository.ObtenerPorSalonYFechaAsync(salonId, fecha);
        return _mapper.Map<IEnumerable<ReservaDTO>>(reservas);
    }

    public async Task<IEnumerable<DisponibilidadDTO>> ObtenerDisponibilidadSalonAsync(int salonId, DateTime fechaInicio, DateTime fechaFin)
    {
        var salon = await _salonRepository.ObtenerPorIdAsync(salonId);
        if (salon == null)
            throw new ArgumentException($"Salón con ID {salonId} no encontrado.");

        var disponibilidades = new List<DisponibilidadDTO>();
        var fechaActual = fechaInicio.Date;

        while (fechaActual <= fechaFin.Date)
        {
            var reservasDelDia = await _reservaRepository.ObtenerPorSalonYFechaAsync(salonId, fechaActual);
            var reservasConfirmadas = reservasDelDia.Where(r => r.Estado != EstadoReserva.Cancelada).OrderBy(r => r.HoraInicio).ToList();

            if (!reservasConfirmadas.Any())
            {
                // El día completo está disponible
                disponibilidades.Add(new DisponibilidadDTO
                {
                    SalonId = salonId,
                    Fecha = fechaActual,
                    HoraInicio = TimeSpan.Zero,
                    HoraFin = TimeSpan.FromHours(24),
                    EstaDisponible = true
                });
            }
            else
            {
                // Agregar bloques disponibles entre reservas
                var horaActual = TimeSpan.Zero;

                foreach (var reserva in reservasConfirmadas)
                {
                    if (reserva.HoraInicio > horaActual)
                    {
                        disponibilidades.Add(new DisponibilidadDTO
                        {
                            SalonId = salonId,
                            Fecha = fechaActual,
                            HoraInicio = horaActual,
                            HoraFin = reserva.HoraInicio,
                            EstaDisponible = true
                        });
                    }

                    horaActual = reserva.HoraFin;
                }

                // Agregar el bloque final del día si queda disponible
                if (horaActual < TimeSpan.FromHours(24))
                {
                    disponibilidades.Add(new DisponibilidadDTO
                    {
                        SalonId = salonId,
                        Fecha = fechaActual,
                        HoraInicio = horaActual,
                        HoraFin = TimeSpan.FromHours(24),
                        EstaDisponible = true
                    });
                }
            }

            fechaActual = fechaActual.AddDays(1);
        }

        return disponibilidades;
    }

    public async Task<bool> ValidarDisponibilidadAsync(int salonId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin, List<(int recursoId, int cantidad)>? recursosRequeridos = null)
    {
        var salon = await _salonRepository.ObtenerPorIdAsync(salonId);
        if (salon == null || !salon.EstaActivo)
            return false;

        // Verificar conflictos de reserva con el salón
        var reservasDelSalon = await _reservaRepository.ObtenerPorSalonYFechaAsync(salonId, fecha);
        var hayConflicto = reservasDelSalon.Any(r =>
            r.Estado != EstadoReserva.Cancelada &&
            !(r.HoraFin <= horaInicio || r.HoraInicio >= horaFin));

        if (hayConflicto)
            return false;

        // Verificar disponibilidad de recursos si se especifican
        if (recursosRequeridos != null && recursosRequeridos.Any())
        {
            foreach (var (recursoId, cantidadRequerida) in recursosRequeridos)
            {
                var disponible = await ObtenerCantidadDisponibleEnHorarioAsync(recursoId, fecha, horaInicio, horaFin);
                if (disponible < cantidadRequerida)
                    return false;
            }
        }

        return true;
    }

    private async Task<int> ObtenerCantidadDisponibleEnHorarioAsync(int recursoId, DateTime fecha, TimeSpan horaInicio, TimeSpan horaFin)
    {
        var recurso = await _recursoRepository.ObtenerPorIdAsync(recursoId);
        if (recurso == null)
            return 0;

        var todasLasReservas = await _reservaRepository.ObtenerTodosAsync();
        var cantidadComprometida = 0;

        foreach (var reserva in todasLasReservas.Where(r =>
            r.Estado != EstadoReserva.Cancelada &&
            r.Fecha == fecha &&
            !(r.HoraFin <= horaInicio || r.HoraInicio >= horaFin)))
        {
            var recursoEnReserva = reserva.ReservaRecursos.FirstOrDefault(rr => rr.RecursoId == recursoId);
            if (recursoEnReserva != null)
                cantidadComprometida += recursoEnReserva.CantidadSolicitada;
        }

        return recurso.CantidadTotal - cantidadComprometida;
    }
}
