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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SalonDTO>>> ObtenerTodos()
    {
        var salones = await _salonService.ObtenerTodosAsync();
        return Ok(salones);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SalonDTO>> ObtenerPorId(int id)
    {
        var salon = await _salonService.ObtenerPorIdAsync(id);
        if (salon == null)
            return NotFound(new { mensaje = $"Salón con ID {id} no encontrado." });

        return Ok(salon);
    }

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
