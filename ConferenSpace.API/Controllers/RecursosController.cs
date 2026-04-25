using ConferenSpace.Application.Contracts;
using ConferenSpace.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ConferenSpace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecursosController : ControllerBase
{
    private readonly IRecursoService _recursoService;
    private readonly ILogger<RecursosController> _logger;

    public RecursosController(IRecursoService recursoService, ILogger<RecursosController> logger)
    {
        _recursoService = recursoService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RecursoDTO>>> ObtenerTodos()
    {
        var recursos = await _recursoService.ObtenerTodosAsync();
        return Ok(recursos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RecursoDTO>> ObtenerPorId(int id)
    {
        var recurso = await _recursoService.ObtenerPorIdAsync(id);
        if (recurso == null)
            return NotFound(new { mensaje = $"Recurso con ID {id} no encontrado." });

        return Ok(recurso);
    }

    [HttpPost]
    public async Task<ActionResult<RecursoDTO>> Crear([FromBody] RecursoDTO recursoDTO)
    {
        try
        {
            var recursoCreado = await _recursoService.CrearAsync(recursoDTO);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = recursoCreado.Id }, recursoCreado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear recurso");
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RecursoDTO>> Actualizar(int id, [FromBody] RecursoDTO recursoDTO)
    {
        try
        {
            var recursoActualizado = await _recursoService.ActualizarAsync(id, recursoDTO);
            return Ok(recursoActualizado);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar recurso");
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("{id}/disponibilidad")]
    public async Task<ActionResult<int>> ObtenerCantidadDisponible(int id)
    {
        try
        {
            var cantidad = await _recursoService.ObtenerCantidadDisponibleAsync(id);
            return Ok(new { cantidad });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    [HttpGet("disponibles")]
    public async Task<ActionResult<IEnumerable<RecursoDTO>>> ObtenerDisponibles(
        [FromQuery] DateTime fecha,
        [FromQuery] string horaInicio,
        [FromQuery] string horaFin,
        [FromQuery] int cantidadMinima = 1)
    {
        try
        {
            if (!TimeSpan.TryParse(horaInicio, out var hora1) || !TimeSpan.TryParse(horaFin, out var hora2))
                return BadRequest(new { mensaje = "Formato de hora inválido. Use HH:mm:ss" });

            var recursos = await _recursoService.ObtenerDisponiblesAsync(fecha, hora1, hora2, cantidadMinima);
            return Ok(recursos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener recursos disponibles");
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> Desactivar(int id)
    {
        try
        {
            await _recursoService.DesactivarAsync(id);
            return Ok(new { mensaje = "Recurso desactivado correctamente." });
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
            await _recursoService.ReactivarAsync(id);
            return Ok(new { mensaje = "Recurso reactivado correctamente." });
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await _recursoService.EliminarAsync(id);
            return Ok(new { mensaje = "Recurso eliminado correctamente." });
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
