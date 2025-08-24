using ParkingApi.Models;

namespace ParkingApi.Data.Repositories;

public interface IReservaRepository
{
    public void AddReserva(Reserva reserva);

    public IEnumerable<Reserva> GetAllReservas();
    public IEnumerable<Reserva> GetReservasByUserId(int userId);
    public Reserva GetReservaById(int id);

    public void SaveChanges();
}
