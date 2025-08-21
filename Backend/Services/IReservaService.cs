using ParkingApi.Models;

namespace ParkingApi.Services
{
    public interface IReservaService
    {
        IEnumerable<Reserva> GetAllReservas();
        Reserva? GetReservaById(int id);
        IEnumerable<Reserva> GetReservasByUsuarioId(int usuarioId);
        Reserva CreateReserva(Reserva reserva);
        Reserva? UpdateReserva(int id, Reserva reserva);
        bool DeleteReserva(int id);
    }
}
