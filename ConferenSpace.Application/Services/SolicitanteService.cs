using AutoMapper;
using ConferenSpace.Application.Contracts;
using ConferenSpace.Application.DTOs;
using ConferenSpace.Domain.Entities;
using ConferenSpace.Infrastructure.Contracts;

namespace ConferenSpace.Application.Services;

/// <summary>
/// Servicio de lógica de negocios para la gestión de solicitantes.
/// </summary>
public class SolicitanteService : ISolicitanteService
{
    private readonly ISolicitanteRepository _solicitanteRepository;
    private readonly IReservaRepository _reservaRepository;
    private readonly IMapper _mapper;

    public SolicitanteService(ISolicitanteRepository solicitanteRepository, IReservaRepository reservaRepository, IMapper mapper)
    {
        _solicitanteRepository = solicitanteRepository;
        _reservaRepository = reservaRepository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<SolicitanteDTO>> ObtenerTodosAsync()
    {
        var solicitantes = await _solicitanteRepository.ObtenerTodosAsync();
        return _mapper.Map<IEnumerable<SolicitanteDTO>>(solicitantes);
    }

    public async Task<SolicitanteDTO?> ObtenerPorIdAsync(int id)
    {
        var solicitante = await _solicitanteRepository.ObtenerPorIdAsync(id);
        return _mapper.Map<SolicitanteDTO?>(solicitante);
    }

    public async Task<SolicitanteDTO> CrearAsync(SolicitanteDTO solicitanteDTO)
    {
        var solicitante = _mapper.Map<Solicitante>(solicitanteDTO);
        solicitante.FechaCreacion = DateTime.UtcNow;

        var solicitanteCreado = await _solicitanteRepository.CrearAsync(solicitante);
        return _mapper.Map<SolicitanteDTO>(solicitanteCreado);
    }

    public async Task<SolicitanteDTO> ActualizarAsync(int id, SolicitanteDTO solicitanteDTO)
    {
        var solicitante = await _solicitanteRepository.ObtenerPorIdAsync(id);
        if (solicitante == null)
            throw new ArgumentException($"Solicitante con ID {id} no encontrado.");

        _mapper.Map(solicitanteDTO, solicitante);

        var solicitanteActualizado = await _solicitanteRepository.ActualizarAsync(solicitante);
        return _mapper.Map<SolicitanteDTO>(solicitanteActualizado);
    }

    public async Task<IEnumerable<SolicitanteDTO>> BuscarPorNombreAsync(string nombre)
    {
        var solicitantes = await _solicitanteRepository.ObtenerTodosAsync();
        var resultado = solicitantes.Where(s => s.NombreCompleto.Contains(nombre, StringComparison.OrdinalIgnoreCase));
        return _mapper.Map<IEnumerable<SolicitanteDTO>>(resultado);
    }

    public async Task<IEnumerable<SolicitanteDTO>> BuscarPorDepartamentoAsync(string departamento)
    {
        var solicitantes = await _solicitanteRepository.ObtenerTodosAsync();
        var resultado = solicitantes.Where(s => s.Departamento.Contains(departamento, StringComparison.OrdinalIgnoreCase));
        return _mapper.Map<IEnumerable<SolicitanteDTO>>(resultado);
    }

    public async Task<SolicitanteDTO?> BuscarPorCorreoAsync(string correo)
    {
        var solicitantes = await _solicitanteRepository.ObtenerTodosAsync();
        var resultado = solicitantes.FirstOrDefault(s => s.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
        return _mapper.Map<SolicitanteDTO?>(resultado);
    }

    public async Task<bool> DesactivarAsync(int id)
    {
        var solicitante = await _solicitanteRepository.ObtenerPorIdAsync(id);
        if (solicitante == null)
            throw new ArgumentException($"Solicitante con ID {id} no encontrado.");

        var tieneReservasActivas = (await _reservaRepository.ObtenerPorSolicitanteAsync(id))
            .Any(r => r.Estado != EstadoReserva.Cancelada && r.Fecha >= DateTime.UtcNow.Date);

        if (tieneReservasActivas)
            throw new InvalidOperationException("No se puede desactivar un solicitante que tiene reservas activas.");

        solicitante.EstaActivo = false;
        await _solicitanteRepository.ActualizarAsync(solicitante);
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var solicitante = await _solicitanteRepository.ObtenerPorIdAsync(id);
        if (solicitante == null)
            throw new ArgumentException($"Solicitante con ID {id} no encontrado.");

        var tieneReservas = (await _reservaRepository.ObtenerPorSolicitanteAsync(id)).Any();
        if (tieneReservas)
            throw new InvalidOperationException("No se puede eliminar un solicitante que tiene reservas asociadas.");

        await _solicitanteRepository.EliminarAsync(id);
        return true;
    }
}
