using ConferenSpace.Domain.Entities;
using ConferenSpace.Infrastructure.Contracts;
using ConferenSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenSpace.Infrastructure.Repositories;

public class SalonRepository : ISalonRepository
{
    private readonly ConferenSpaceDbContext _context;

    public SalonRepository(ConferenSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Salon>> ObtenerTodosAsync()
    {
        return await _context.Salones
            .Where(s => s.EstaActivo)
            .Include(s => s.Reservas)
            .ToListAsync();
    }

    public async Task<Salon?> ObtenerPorIdAsync(int id)
    {
        return await _context.Salones
            .Include(s => s.Reservas)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Salon> CrearAsync(Salon salon)
    {
        _context.Salones.Add(salon);
        await _context.SaveChangesAsync();
        return salon;
    }

    public async Task<Salon> ActualizarAsync(Salon salon)
    {
        _context.Salones.Update(salon);
        await _context.SaveChangesAsync();
        return salon;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var salon = await _context.Salones.FindAsync(id);
        if (salon == null)
            return false;

        _context.Salones.Remove(salon);
        await _context.SaveChangesAsync();
        return true;
    }
}
