using Microsoft.AspNetCore.Mvc;
using ParkingApi.Models;
using ParkingApi.Services;

namespace ParkingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ticket>> GetTickets()
        {
            var tickets = _ticketService.GetAllTickets();
            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public ActionResult<Ticket> GetTicket(int id)
        {
            var ticket = _ticketService.GetTicketById(id);
            if (ticket == null)
                return NotFound();
            return Ok(ticket);
        }

        [HttpGet("reserva/{reservaId}")]
        public ActionResult<Ticket> GetTicketByReserva(int reservaId)
        {
            var ticket = _ticketService.GetTicketByReservaId(reservaId);
            if (ticket == null)
                return NotFound();
            return Ok(ticket);
        }

        [HttpPost]
        public ActionResult<Ticket> CreateTicket(Ticket ticket)
        {
            var createdTicket = _ticketService.CreateTicket(ticket);
            return CreatedAtAction(nameof(GetTicket), new { id = createdTicket.Id }, createdTicket);
        }

        [HttpPut("{id}")]
        public ActionResult<Ticket> UpdateTicket(int id, Ticket ticket)
        {
            var updatedTicket = _ticketService.UpdateTicket(id, ticket);
            if (updatedTicket == null)
                return NotFound();
            
            return Ok(updatedTicket);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTicket(int id)
        {
            var deleted = _ticketService.DeleteTicket(id);
            if (!deleted)
                return NotFound();
            
            return NoContent();
        }
    }
}
