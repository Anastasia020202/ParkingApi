using ParkingApi.Models;
using ParkingApi.Data.Repositories;

namespace ParkingApi.Business.Services
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _repository;

        public TicketService(ITicketRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Ticket> GetAllTickets()
        {
            return _repository.GetAll();
        }

        public Ticket? GetTicketById(int id)
        {
            return _repository.GetById(id);
        }

        public Ticket? GetTicketByReservaId(int reservaId)
        {
            return _repository.GetByReservaId(reservaId);
        }

        public Ticket CreateTicket(Ticket ticket)
        {
            return _repository.Add(ticket);
        }

        public Ticket? UpdateTicket(int id, Ticket ticket)
        {
            return _repository.Update(id, ticket);
        }

        public bool DeleteTicket(int id)
        {
            return _repository.Delete(id);
        }
    }
}
