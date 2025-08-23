using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ParkingApi.Models;
using ParkingApi.Business.Services;
using System.Net.Mime;

namespace ParkingApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly ITicketPdfService _pdfService;

        public TicketsController(ITicketService ticketService, ITicketPdfService pdfService)
        {
            _ticketService = ticketService;
            _pdfService = pdfService;
        }

        [Authorize]
        [HttpGet(Name = "GetAllTickets")]
        public ActionResult<IEnumerable<Ticket>> GetAllTickets()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_ticketService.EsAdmin(HttpContext.User))
                {
                    return Unauthorized();
                }

                var tickets = _ticketService.GetAllTickets();
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetTicket")]
        public IActionResult GetTicket(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_ticketService.TieneAcceso(id, HttpContext.User))
                {
                    return Unauthorized();
                }

                var ticket = _ticketService.GetTicketById(id);
                return Ok(ticket);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"El ticket de id {id} no existe");
            }
        }

        [Authorize]
        [HttpGet("usuario/{usuarioId}", Name = "GetUsuarioTickets")]
        public IActionResult GetUsuarioTickets(int usuarioId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_ticketService.TieneAcceso(usuarioId, HttpContext.User))
                {
                    return Unauthorized();
                }

                var tickets = _ticketService.GetTicketsByUsuarioId(usuarioId);
                return Ok(tickets);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("{id}/pdf")]
        public IActionResult DownloadTicketPdf(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_ticketService.TieneAcceso(id, HttpContext.User))
                {
                    return Unauthorized();
                }

                var ticket = _ticketService.GetTicketById(id);
                byte[] pdfBytes = _pdfService.GenerateTicketPdf(ticket);
                string fileName = $"ticket_{ticket.NumeroTicket}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                
                return File(pdfBytes, MediaTypeNames.Application.Pdf, fileName);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"El ticket de id {id} no existe");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generando PDF: {ex.Message}");
            }
        }
    }
}
