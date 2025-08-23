using ParkingApi.Models;

namespace ParkingApi.Business.Services
{
    public interface ITicketService
    {
        IEnumerable<Ticket> GetAllTickets();
        Ticket? GetTicketById(int id);
        Ticket? GetTicketByReservaId(int reservaId);
        IEnumerable<Ticket> GetTicketsByUsuarioId(int usuarioId);
        Ticket CreateTicket(Ticket ticket);
        Ticket? UpdateTicket(int id, Ticket ticket);
        bool DeleteTicket(int id);
    }
}
