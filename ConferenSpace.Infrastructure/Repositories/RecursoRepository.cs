using ConferenSpace.Domain.Entities;
using ConferenSpace.Infrastructure.Contracts;
using ConferenSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenSpace.Infrastructure.Repositories;

/// <summary>
/// Repositorio para la entidad Recurso.
/// </summary>
public class RecursoRepository : IRecursoRepository
{
    private readonly ConferenSpaceDbContext _context;

    public RecursoRepository(ConferenSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Recurso>> ObtenerTodosAsync()
    {
        return await _context.Recursos
            .Where(r => r.EstaActivo)
            .Include(r => r.ReservaRecursos)
            .ToListAsync();
    }

    public async Task<Recurso?> ObtenerPorIdAsync(int id)
    {
        return await _context.Recursos
            .Include(r => r.ReservaRecursos)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Recurso> CrearAsync(Recurso recurso)
    {
        _context.Recursos.Add(recurso);
        await _context.SaveChangesAsync();
        return recurso;
    }

    public async Task<Recurso> ActualizarAsync(Recurso recurso)
    {
        _context.Recursos.Update(recurso);
        await _context.SaveChangesAsync();
        return recurso;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var recurso = await _context.Recursos.FindAsync(id);
        if (recurso == null)
            return false;

        _context.Recursos.Remove(recurso);
        await _context.SaveChangesAsync();
        return true;
    }
}
