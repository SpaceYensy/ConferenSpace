using ConferenSpace.Application.Contracts;
using ConferenSpace.Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ConferenSpace.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SolicitantesController : ControllerBase
{
    private readonly ISolicitanteService _solicitanteService;
    private readonly ILogger<SolicitantesController> _logger;

    public SolicitantesController(ISolicitanteService solicitanteService, ILogger<SolicitantesController> logger)
    {
        _solicitanteService = solicitanteService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SolicitanteDTO>>> ObtenerTodos()
    {
        var solicitantes = await _solicitanteService.ObtenerTodosAsync();
        return Ok(solicitantes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SolicitanteDTO>> ObtenerPorId(int id)
    {
        var solicitante = await _solicitanteService.ObtenerPorIdAsync(id);
        if (solicitante == null)
            return NotFound(new { mensaje = $"Solicitante con ID {id} no encontrado." });

        return Ok(solicitante);
    }

    [HttpPost]
    public async Task<ActionResult<SolicitanteDTO>> Crear([FromBody] SolicitanteDTO solicitanteDTO)
    {
        try
        {
            var solicitanteCreado = await _solicitanteService.CrearAsync(solicitanteDTO);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = solicitanteCreado.Id }, solicitanteCreado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear solicitante");
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SolicitanteDTO>> Actualizar(int id, [FromBody] SolicitanteDTO solicitanteDTO)
    {
        try
        {
            var solicitanteActualizado = await _solicitanteService.ActualizarAsync(id, solicitanteDTO);
            return Ok(solicitanteActualizado);
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar solicitante");
            return BadRequest(new { mensaje = ex.Message });
        }
    }

    [HttpGet("buscar/nombre/{nombre}")]
    public async Task<ActionResult<IEnumerable<SolicitanteDTO>>> BuscarPorNombre(string nombre)
    {
        var solicitantes = await _solicitanteService.BuscarPorNombreAsync(nombre);
        return Ok(solicitantes);
    }

    [HttpGet("buscar/departamento/{departamento}")]
    public async Task<ActionResult<IEnumerable<SolicitanteDTO>>> BuscarPorDepartamento(string departamento)
    {
        var solicitantes = await _solicitanteService.BuscarPorDepartamentoAsync(departamento);
        return Ok(solicitantes);
    }

    [HttpGet("buscar/correo")]
    public async Task<ActionResult<SolicitanteDTO>> BuscarPorCorreo([FromQuery] string correo)
    {
        var solicitante = await _solicitanteService.BuscarPorCorreoAsync(correo);
        if (solicitante == null)
            return NotFound(new { mensaje = "Solicitante no encontrado." });

        return Ok(solicitante);
    }

    [HttpPatch("{id}/desactivar")]
    public async Task<IActionResult> Desactivar(int id)
    {
        try
        {
            await _solicitanteService.DesactivarAsync(id);
            return Ok(new { mensaje = "Solicitante desactivado correctamente." });
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            await _solicitanteService.EliminarAsync(id);
            return Ok(new { mensaje = "Solicitante eliminado correctamente." });
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
