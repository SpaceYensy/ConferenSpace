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

    /// <summary>
    /// Obtiene todos los solicitantes activos.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SolicitanteDTO>>> ObtenerTodos()
    {
        var solicitantes = await _solicitanteService.ObtenerTodosAsync();
        return Ok(solicitantes);
    }

    /// <summary>
    /// Obtiene un solicitante específico por su ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<SolicitanteDTO>> ObtenerPorId(int id)
    {
        var solicitante = await _solicitanteService.ObtenerPorIdAsync(id);
        if (solicitante == null)
            return NotFound(new { mensaje = $"Solicitante con ID {id} no encontrado." });

        return Ok(solicitante);
    }

    /// <summary>
    /// Crea un nuevo solicitante.
    /// </summary>
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

    /// <summary>
    /// Actualiza un solicitante existente.
    /// </summary>
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

    /// <summary>
    /// Busca solicitantes por nombre.
    /// </summary>
    [HttpGet("buscar/nombre/{nombre}")]
    public async Task<ActionResult<IEnumerable<SolicitanteDTO>>> BuscarPorNombre(string nombre)
    {
        var solicitantes = await _solicitanteService.BuscarPorNombreAsync(nombre);
        return Ok(solicitantes);
    }

    /// <summary>
    /// Busca solicitantes por departamento.
    /// </summary>
    [HttpGet("buscar/departamento/{departamento}")]
    public async Task<ActionResult<IEnumerable<SolicitanteDTO>>> BuscarPorDepartamento(string departamento)
    {
        var solicitantes = await _solicitanteService.BuscarPorDepartamentoAsync(departamento);
        return Ok(solicitantes);
    }

    /// <summary>
    /// Busca un solicitante por correo electrónico.
    /// </summary>
    [HttpGet("buscar/correo")]
    public async Task<ActionResult<SolicitanteDTO>> BuscarPorCorreo([FromQuery] string correo)
    {
        var solicitante = await _solicitanteService.BuscarPorCorreoAsync(correo);
        if (solicitante == null)
            return NotFound(new { mensaje = "Solicitante no encontrado." });

        return Ok(solicitante);
    }

    /// <summary>
    /// Desactiva un solicitante (solo si no tiene reservas activas).
    /// </summary>
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

    /// <summary>
    /// Elimina un solicitante del sistema (solo si no tiene reservas).
    /// </summary>
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
