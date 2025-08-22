using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ParkingApi.Models;
using ParkingApi.Services;

namespace ParkingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VehiculosController : ControllerBase
    {
        private readonly IVehiculoService _vehiculoService;

        public VehiculosController(IVehiculoService vehiculoService)
        {
            _vehiculoService = vehiculoService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Vehiculo>> GetVehiculos()
        {
            var vehiculos = _vehiculoService.GetAllVehiculos();
            return Ok(vehiculos);
        }

        [HttpGet("{id}")]
        public ActionResult<Vehiculo> GetVehiculo(int id)
        {
            var vehiculo = _vehiculoService.GetVehiculoById(id);
            if (vehiculo == null)
                return NotFound();
            return Ok(vehiculo);
        }

        [HttpGet("usuario/{usuarioId}")]
        public ActionResult<IEnumerable<Vehiculo>> GetVehiculosByUsuario(int usuarioId)
        {
            var vehiculos = _vehiculoService.GetVehiculosByUsuarioId(usuarioId);
            return Ok(vehiculos);
        }

        [HttpPost]
        public ActionResult<Vehiculo> CreateVehiculo(Vehiculo vehiculo)
        {
            var createdVehiculo = _vehiculoService.CreateVehiculo(vehiculo);
            return CreatedAtAction(nameof(GetVehiculo), new { id = createdVehiculo.Id }, createdVehiculo);
        }

        [HttpPut("{id}")]
        public ActionResult<Vehiculo> UpdateVehiculo(int id, Vehiculo vehiculo)
        {
            var updatedVehiculo = _vehiculoService.UpdateVehiculo(id, vehiculo);
            if (updatedVehiculo == null)
                return NotFound();
            
            return Ok(updatedVehiculo);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteVehiculo(int id)
        {
            var deleted = _vehiculoService.DeleteVehiculo(id);
            if (!deleted)
                return NotFound();
            
            return NoContent();
        }
    }
}
