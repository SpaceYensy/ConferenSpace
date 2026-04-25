using ConferenSpace.Domain.Entities;
using ConferenSpace.Infrastructure.Contracts;
using ConferenSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenSpace.Infrastructure.Repositories;

public class SolicitanteRepository : ISolicitanteRepository
{
    private readonly ConferenSpaceDbContext _context;

    public SolicitanteRepository(ConferenSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Solicitante>> ObtenerTodosAsync()
    {
        return await _context.Solicitantes
            .Where(s => s.EstaActivo)
            .Include(s => s.Reservas)
            .ToListAsync();
    }

    public async Task<Solicitante?> ObtenerPorIdAsync(int id)
    {
        return await _context.Solicitantes
            .Include(s => s.Reservas)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Solicitante> CrearAsync(Solicitante solicitante)
    {
        _context.Solicitantes.Add(solicitante);
        await _context.SaveChangesAsync();
        return solicitante;
    }

    public async Task<Solicitante> ActualizarAsync(Solicitante solicitante)
    {
        _context.Solicitantes.Update(solicitante);
        await _context.SaveChangesAsync();
        return solicitante;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var solicitante = await _context.Solicitantes.FindAsync(id);
        if (solicitante == null)
            return false;

        _context.Solicitantes.Remove(solicitante);
        await _context.SaveChangesAsync();
        return true;
    }
}
