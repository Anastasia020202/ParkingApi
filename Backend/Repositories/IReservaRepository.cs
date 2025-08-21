using ParkingApi.Models;

namespace ParkingApi.Repositories
{
    public interface IReservaRepository
    {
        IEnumerable<Reserva> GetAll();
        Reserva? GetById(int id);
        IEnumerable<Reserva> GetByUsuarioId(int usuarioId);
        Reserva Add(Reserva reserva);
        Reserva? Update(int id, Reserva reserva);
        bool Delete(int id);
    }
}
