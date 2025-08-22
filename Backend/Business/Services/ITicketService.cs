using ParkingApi.Models;

namespace ParkingApi.Services
{
    public interface ITicketService
    {
        IEnumerable<Ticket> GetAllTickets();
        Ticket? GetTicketById(int id);
        Ticket? GetTicketByReservaId(int reservaId);
        Ticket CreateTicket(Ticket ticket);
        Ticket? UpdateTicket(int id, Ticket ticket);
        bool DeleteTicket(int id);
    }
}
