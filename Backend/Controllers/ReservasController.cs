using Microsoft.AspNetCore.Mvc;
using ParkingApi.Models;
using ParkingApi.Services;

namespace ParkingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Reserva>> GetReservas(
            [FromQuery] int? usuarioId,
            [FromQuery] int? plazaId,
            [FromQuery] DateTime? fechaDesde,
            [FromQuery] DateTime? fechaHasta,
            [FromQuery] string? orden,
            [FromQuery] bool? soloActivas)
        {
            var queryParameters = new ReservaQueryParameters
            {
                UsuarioId = usuarioId,
                PlazaId = plazaId,
                FechaDesde = fechaDesde,
                FechaHasta = fechaHasta,
                Orden = orden,
                SoloActivas = soloActivas
            };

            var reservas = _reservaService.GetAllReservas(queryParameters);
            return Ok(reservas);
        }

        [HttpGet("{id}")]
        public ActionResult<Reserva> GetReserva(int id)
        {
            var reserva = _reservaService.GetReservaById(id);
            if (reserva == null)
                return NotFound();
            return Ok(reserva);
        }

        [HttpGet("usuario/{usuarioId}")]
        public ActionResult<IEnumerable<Reserva>> GetReservasByUsuario(int usuarioId)
        {
            var reservas = _reservaService.GetReservasByUsuarioId(usuarioId);
            return Ok(reservas);
        }

        [HttpPost]
        public ActionResult<Reserva> CreateReserva(Reserva reserva)
        {
            var createdReserva = _reservaService.CreateReserva(reserva);
            return CreatedAtAction(nameof(GetReserva), new { id = createdReserva.Id }, createdReserva);
        }

        [HttpPut("{id}")]
        public ActionResult<Reserva> UpdateReserva(int id, Reserva reserva)
        {
            var updatedReserva = _reservaService.UpdateReserva(id, reserva);
            if (updatedReserva == null)
                return NotFound();
            
            return Ok(updatedReserva);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteReserva(int id)
        {
            var deleted = _reservaService.DeleteReserva(id);
            if (!deleted)
                return NotFound();
            
            return NoContent();
        }
    }
}
