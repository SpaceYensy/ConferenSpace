using ConferenSpace.Domain.Entities;
using ConferenSpace.Infrastructure.Contracts;
using ConferenSpace.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ConferenSpace.Infrastructure.Repositories;
public class ReservaRepository : IReservaRepository
{
    private readonly ConferenSpaceDbContext _context;

    public ReservaRepository(ConferenSpaceDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Reserva>> ObtenerTodosAsync()
    {
        return await _context.Reservas
            .Include(r => r.Solicitante)
            .Include(r => r.Salon)
            .Include(r => r.ReservaRecursos)
                .ThenInclude(rr => rr.Recurso)
            .ToListAsync();
    }

    public async Task<Reserva?> ObtenerPorIdAsync(int id)
    {
        return await _context.Reservas
            .Include(r => r.Solicitante)
            .Include(r => r.Salon)
            .Include(r => r.ReservaRecursos)
                .ThenInclude(rr => rr.Recurso)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<Reserva> CrearAsync(Reserva reserva)
    {
        _context.Reservas.Add(reserva);
        await _context.SaveChangesAsync();
        return reserva;
    }

    public async Task<Reserva> ActualizarAsync(Reserva reserva)
    {
        _context.Reservas.Update(reserva);
        await _context.SaveChangesAsync();
        return reserva;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var reserva = await _context.Reservas.FindAsync(id);
        if (reserva == null)
            return false;

        _context.Reservas.Remove(reserva);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Reserva>> ObtenerPorSolicitanteAsync(int solicitanteId)
    {
        return await _context.Reservas
            .Where(r => r.SolicitanteId == solicitanteId)
            .Include(r => r.Solicitante)
            .Include(r => r.Salon)
            .Include(r => r.ReservaRecursos)
                .ThenInclude(rr => rr.Recurso)
            .OrderByDescending(r => r.Fecha)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reserva>> ObtenerPorSalonYFechaAsync(int salonId, DateTime fecha)
    {
        return await _context.Reservas
            .Where(r => r.SalonId == salonId && r.Fecha.Date == fecha.Date)
            .Include(r => r.Solicitante)
            .Include(r => r.Salon)
            .Include(r => r.ReservaRecursos)
                .ThenInclude(rr => rr.Recurso)
            .OrderBy(r => r.HoraInicio)
            .ToListAsync();
    }
}
