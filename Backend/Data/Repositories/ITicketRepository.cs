using ParkingApi.Models;

namespace ParkingApi.Data.Repositories
{
    public interface ITicketRepository
    {
        IEnumerable<Ticket> GetAll();
        Ticket? GetById(int id);
        Ticket? GetByReservaId(int reservaId);
        Ticket Add(Ticket ticket);
        Ticket? Update(int id, Ticket ticket);
        bool Delete(int id);
    }
}
