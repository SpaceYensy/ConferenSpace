using ConferenSpace.Application.Contracts;
using ConferenSpace.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ConferenSpace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservasController : ControllerBase
{
    private readonly IReservaService _reservaService;
    private readonly ILogger<ReservasController> _logger;

    public ReservasController(IReservaService reservaService, ILogger<ReservasController> logger)
    {
        _reservaService = reservaService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las reservas registradas.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservaDTO>>> ObtenerTodas()
    {
        var reservas = await _reservaService.ObtenerTodosAsync();
        return Ok(reservas);
    }

    /// <summary>
    /// Obtiene una reserva específica por su ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<ReservaDTO>> ObtenerPorId(int id)
    {
        var reserva = await _reservaService.ObtenerPorIdAsync(id);
        if (reserva == null)
            return NotFound(new { mensaje = $"Reserva con ID {id} no encontrada." });

        return Ok(reserva);
    }

    /// <summary>
    /// Crea una nueva reserva con validación automática de disponibilidad.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ReservaDTO>> Crear([FromBody] ReservaCrearDTO reservaCrearDTO)
    {
        try
        {
            var reservaCreada = await _reservaService.CrearAsync(reservaCrearDTO);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = reservaCreada.Id }, reservaCreada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear reserva");
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza una reserva existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<ReservaDTO>> Actualizar(int id, [FromBody] ReservaCrearDTO reservaCrearDTO)
    {
        try
        {
            var reservaActualizada = await _reservaService.ActualizarAsync(id, reservaCrearDTO);
            return Ok(reservaActualizada);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar reserva");
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Cancela una reserva existente.
    /// </summary>
    [HttpPatch("{id}/cancelar")]
    public async Task<IActionResult> Cancelar(int id)
    {
        try
        {
            await _reservaService.CancelarAsync(id);
            return Ok(new { mensaje = "Reserva cancelada correctamente." });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cancelar reserva");
            return StatusCode(StatusCodes.Status500InternalServerError, new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene las reservas de un solicitante específico.
    /// </summary>
    [HttpGet("solicitante/{solicitanteId}")]
    public async Task<ActionResult<IEnumerable<ReservaDTO>>> ObtenerPorSolicitante(int solicitanteId)
    {
        var reservas = await _reservaService.ObtenerPorSolicitanteAsync(solicitanteId);
        return Ok(reservas);
    }

    /// <summary>
    /// Obtiene las reservas de un salón en una fecha específica.
    /// </summary>
    [HttpGet("salon/{salonId}")]
    public async Task<ActionResult<IEnumerable<ReservaDTO>>> ObtenerPorSalonYFecha(int salonId, [FromQuery] DateTime fecha)
    {
        var reservas = await _reservaService.ObtenerPorSalonYFechaAsync(salonId, fecha);
        return Ok(reservas);
    }

    /// <summary>
    /// Obtiene la disponibilidad de un salón en un rango de fechas.
    /// Retorna bloques de tiempo disponibles y no disponibles.
    /// </summary>
    [HttpGet("salon/{salonId}/disponibilidad")]
    public async Task<ActionResult<IEnumerable<DisponibilidadDTO>>> ObtenerDisponibilidad(
        int salonId,
        [FromQuery] DateTime fechaInicio,
        [FromQuery] DateTime fechaFin)
    {
        try
        {
            var disponibilidades = await _reservaService.ObtenerDisponibilidadSalonAsync(salonId, fechaInicio, fechaFin);
            return Ok(disponibilidades);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Valida si un salón y recursos están disponibles sin crear una reserva.
    /// Útil para verificar disponibilidad antes de crear una reserva.
    /// </summary>
    [HttpPost("validar-disponibilidad")]
    public async Task<ActionResult<object>> ValidarDisponibilidad(
        [FromQuery] int salonId,
        [FromQuery] DateTime fecha,
        [FromQuery] string horaInicio,
        [FromQuery] string horaFin)
    {
        try
        {
            if (!TimeSpan.TryParse(horaInicio, out var hora1) || !TimeSpan.TryParse(horaFin, out var hora2))
                return BadRequest(new { mensaje = "Formato de hora inválido. Use HH:mm:ss" });

            var disponible = await _reservaService.ValidarDisponibilidadAsync(salonId, fecha, hora1, hora2);
            return Ok(new { disponible, mensaje = disponible ? "El salón está disponible" : "El salón no está disponible" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar disponibilidad");
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
