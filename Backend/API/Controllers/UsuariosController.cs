using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ParkingApi.Models;
using ParkingApi.Business.Services;
using ParkingApi.Models.DTOs;

namespace ParkingApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        [Authorize]
        [HttpGet(Name = "GetAllUsuarios")]
        public ActionResult<IEnumerable<UsuarioReadDto>> GetAllUsuarios([FromQuery] UsuarioQueryParameters query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_usuarioService.EsAdmin(HttpContext.User))
            {
                return Unauthorized();
            }

            try
            {
                var usuarios = _usuarioService.GetAllUsuarios(query);
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetUsuario")]
        public IActionResult GetUsuario(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_usuarioService.TieneAcceso(id, HttpContext.User))
                {
                    return Unauthorized();
                }
                var usuario = _usuarioService.GetUsuarioById(id);
                return Ok(usuario);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"El usuario de id {id} no existe");
            }
        }

        // Ver reservas de usuario
        [Authorize]
        [HttpGet("{id}/reservas", Name = "GetUsuarioReservas")]
        public IActionResult GetUsuarioReservas(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_usuarioService.TieneAcceso(id, HttpContext.User))
                {
                    return Unauthorized();
                }

                var reservas = _usuarioService.GetReservasByUser(id);
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

        // Ver reserva singular de usuario (usuario/id/reservas/id)
        [Authorize]
        [HttpGet("{usuarioId}/reservas/{reservaId}", Name = "GetUsuarioReserva")]
        public IActionResult GetUsuarioReservaSingular(int usuarioId, int reservaId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_usuarioService.TieneAcceso(usuarioId, HttpContext.User))
                {
                    return Unauthorized();
                }

                var reserva = _usuarioService.GetReservaByUserAndId(usuarioId, reservaId);
                return Ok(reserva);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"No se han encontrado la reserva {reservaId} del usuario {usuarioId}");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
