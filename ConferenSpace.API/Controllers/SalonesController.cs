using ConferenSpace.Application.Contracts;
using ConferenSpace.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ConferenSpace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalonesController : ControllerBase
{
    private readonly ISalonService _salonService;
    private readonly ILogger<SalonesController> _logger;

    public SalonesController(ISalonService salonService, ILogger<SalonesController> logger)
    {
        _salonService = salonService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los salones disponibles.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalonDTO>>> ObtenerTodos()
    {
        var salones = await _salonService.ObtenerTodosAsync();
        return Ok(salones);
    }

    /// <summary>
    /// Obtiene un salón específico por su ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<SalonDTO>> ObtenerPorId(int id)
    {
        var salon = await _salonService.ObtenerPorIdAsync(id);
        if (salon == null)
            return NotFound(new { mensaje = $"Salón con ID {id} no encontrado." });

        return Ok(salon);
    }

    /// <summary>
    /// Crea un nuevo salón.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<SalonDTO>> Crear([FromBody] SalonDTO salonDTO)
    {
        try
        {
            var salonCreado = await _salonService.CrearAsync(salonDTO);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = salonCreado.Id }, salonCreado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear salón");
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Actualiza un salón existente.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<SalonDTO>> Actualizar(int id, [FromBody] SalonDTO salonDTO)
    {
        try
        {
            var salonActualizado = await _salonService.ActualizarAsync(id, salonDTO);
            return Ok(salonActualizado);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar salón");
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Pone un salón fuera de servicio.
    /// </summary>
    [HttpPatch("{id}/fuera-de-servicio")]
    public async Task<IActionResult> PonerFueraDeServicio(int id)
    {
        try
        {
            await _salonService.PonerFueraDeServicioAsync(id);
            return Ok(new { mensaje = "Salón puesto fuera de servicio correctamente." });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Reactiva un salón que estaba fuera de servicio.
    /// </summary>
    [HttpPatch("{id}/reactivar")]
    public async Task<IActionResult> Reactivar(int id)
    {
        try
        {
            await _salonService.ReactivarAsync(id);
            return Ok(new { mensaje = "Salón reactivado correctamente." });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Obtiene salones disponibles para una fecha y rango horario específico.
    /// </summary>
    [HttpGet("disponibles")]
    public async Task<ActionResult<IEnumerable<SalonDTO>>> ObtenerDisponibles(
        [FromQuery] DateTime fecha,
        [FromQuery] string horaInicio,
        [FromQuery] string horaFin,
        [FromQuery] int capacidadMinima = 1)
    {
        try
        {
            if (!TimeSpan.TryParse(horaInicio, out var hora1) || !TimeSpan.TryParse(horaFin, out var hora2))
                return BadRequest(new { mensaje = "Formato de hora inválido. Use HH:mm:ss" });

            var salones = await _salonService.ObtenerDisponiblesAsync(fecha, hora1, hora2, capacidadMinima);
            return Ok(salones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener salones disponibles");
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    /// <summary>
    /// Elimina un salón del sistema.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await _salonService.EliminarAsync(id);
            return Ok(new { mensaje = "Salón eliminado correctamente." });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
    }
}
