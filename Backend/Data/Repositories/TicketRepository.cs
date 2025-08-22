using ParkingApi.Models;
using ParkingApi.Data.Repositories;

namespace ParkingApi.Data.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private static List<Ticket> _tickets = new List<Ticket>();

        public IEnumerable<Ticket> GetAll()
        {
            return _tickets;
        }

        public Ticket? GetById(int id)
        {
            return _tickets.FirstOrDefault(t => t.Id == id);
        }

        public Ticket? GetByReservaId(int reservaId)
        {
            return _tickets.FirstOrDefault(t => t.ReservaId == reservaId);
        }

        public Ticket Add(Ticket ticket)
        {
            ticket.Id = _tickets.Count > 0 ? _tickets.Max(t => t.Id) + 1 : 1;
            ticket.NumeroTicket = $"T-{DateTime.Now:yyyyMMdd}-{ticket.Id:D4}";
            _tickets.Add(ticket);
            return ticket;
        }

        public Ticket? Update(int id, Ticket ticket)
        {
            var existingTicket = _tickets.FirstOrDefault(t => t.Id == id);
            if (existingTicket == null)
                return null;

            existingTicket.ReservaId = ticket.ReservaId;
            existingTicket.Importe = ticket.Importe;
            existingTicket.Estado = ticket.Estado;

            return existingTicket;
        }

        public bool Delete(int id)
        {
            var ticket = _tickets.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
                return false;

            return _tickets.Remove(ticket);
        }
    }
}
