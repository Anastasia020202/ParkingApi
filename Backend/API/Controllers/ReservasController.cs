using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ParkingApi.Models;
using ParkingApi.Business.Services;
using System.Security.Claims;

namespace ParkingApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [Authorize]
        [HttpGet(Name = "GetAllReservas")]
        public ActionResult<IEnumerable<Reserva>> GetAllReservas()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_reservaService.EsAdmin(HttpContext.User))
                {
                    return Unauthorized();
                }

                var reservas = _reservaService.GetAllReservas();
                return Ok(reservas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetReserva")]
        public IActionResult GetReserva(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_reservaService.TieneAcceso(id, HttpContext.User))
                {
                    return Unauthorized();
                }

                var reserva = _reservaService.GetReservaById(id);
                return Ok(reserva);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"La reserva de id {id} no existe");
            }
        }

        [Authorize]
        [HttpGet("usuario/{usuarioId}", Name = "GetUsuarioReservas")]
        public IActionResult GetUsuarioReservas(int usuarioId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_reservaService.TieneAcceso(usuarioId, HttpContext.User))
                {
                    return Unauthorized();
                }

                var reservas = _reservaService.GetReservasByUsuarioId(usuarioId);
                return Ok(reservas);
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
        [HttpPost(Name = "HacerReserva")]
        public IActionResult HacerReserva(Reserva reserva)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Cogiendo id del JWT
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var reservaCreada = _reservaService.CreateReserva(usuarioId, reserva);

                // Al solo devolver al id, no se serializa
                return Ok(reservaCreada.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
