using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ParkingApi.Business.Services;
using System.Net.Mime;

namespace ParkingApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
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

                // Generar PDF del ticket basado en la reserva
                byte[] pdfBytes = _ticketService.GenerateTicketPdf(id);
                string fileName = $"ticket_reserva_{id}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                
                return File(pdfBytes, MediaTypeNames.Application.Pdf, fileName);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"La reserva de id {id} no existe");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generando PDF: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("reserva/{reservaId}/pdf")]
        public IActionResult DownloadReservaTicketPdf(int reservaId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_ticketService.TieneAcceso(reservaId, HttpContext.User))
                {
                    return Unauthorized();
                }

                // Generar PDF del ticket basado en la reserva
                byte[] pdfBytes = _ticketService.GenerateTicketPdf(reservaId);
                string fileName = $"ticket_reserva_{reservaId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
                
                return File(pdfBytes, MediaTypeNames.Application.Pdf, fileName);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"La reserva de id {reservaId} no existe");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error generando PDF: {ex.Message}");
            }
        }
    }
}
