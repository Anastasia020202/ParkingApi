using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ParkingApi.Models;
using ParkingApi.Business.Services;
using System.Security.Claims;

namespace ParkingApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VehiculosController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;

        public VehiculosController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        [Authorize]
        [HttpGet(Name = "GetAllVehiculos")]
        public ActionResult<IEnumerable<Vehiculo>> GetAllVehiculos()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_vehiculoService.EsAdmin(HttpContext.User))
                {
                    return Unauthorized();
                }

                var vehiculos = _vehiculoService.GetAllVehiculos();
                return Ok(vehiculos);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [Authorize]
        [HttpGet("{id}", Name = "GetVehiculo")]
        public IActionResult GetVehiculo(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_vehiculoService.TieneAcceso(id, HttpContext.User))
                {
                    return Unauthorized();
                }

                var vehiculo = _vehiculoService.GetVehiculoById(id);
                return Ok(vehiculo);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"El vehículo de id {id} no existe");
            }
        }

        [Authorize]
        [HttpGet("usuario/{usuarioId}", Name = "GetUsuarioVehiculos")]
        public IActionResult GetUsuarioVehiculos(int usuarioId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (!_vehiculoService.TieneAcceso(usuarioId, HttpContext.User))
                {
                    return Unauthorized();
                }

                var vehiculos = _vehiculoService.GetVehiculosByUsuarioId(usuarioId);
                return Ok(vehiculos);
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
        [HttpPost]
        public IActionResult CreateVehiculo([FromBody] Vehiculo vehiculo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Cogiendo id del JWT
                var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var vehiculoCreado = _vehiculoService.CreateVehiculo(usuarioId, vehiculo);

                return Ok(vehiculoCreado.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
