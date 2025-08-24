using ParkingApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ParkingApi.Data.Repositories;

public class ReservaRepository : IReservaRepository
{
    private readonly ParkingDbContext _context;

    public ReservaRepository(ParkingDbContext context)
    {
        _context = context;
    }

    public void AddReserva(Reserva reserva)
    {
        _context.Reservas.Add(reserva);
    }

    public IEnumerable<Reserva> GetAllReservas()
    {
        var result = _context.Reservas.Include(p => p.Usuario)
                                     .Include(p => p.Plaza)
                                     .Include(p => p.Vehiculo)
                                     .ToList();

        return result;
    }

    public Reserva GetReservaById(int id)
    {
        var reservas = _context.Reservas.Include(p => p.Usuario)
            .Include(p => p.Plaza)
            .Include(p => p.Vehiculo);

        var reserva = reservas.FirstOrDefault(pedido => pedido.Id == id);

        if (reserva is null)
        {
            throw new KeyNotFoundException("Reserva no encontrada");
        }

        return reserva;
    }

    public IEnumerable<Reserva> GetReservasByUserId(int userId)
    {
        var reservas = _context.Reservas.Include(p => p.Usuario)
                                      .Include(p => p.Plaza)
                                      .Include(p => p.Vehiculo)
                                      .Where(pedido => pedido.UsuarioId == userId);

        if (reservas is null || !reservas.Any())
        {
            throw new KeyNotFoundException("Ninguna reserva asociada con esa id de usuario.");
        }

        var result = reservas.ToList();
        return result;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
