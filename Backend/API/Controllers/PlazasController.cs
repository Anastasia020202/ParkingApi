using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParkingApi.Business.Services;
using ParkingApi.Models;
using ParkingApi.Models.DTOs;

namespace ParkingApi.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlazasController : ControllerBase
    {
        private readonly IPlazaService _plazaService;

        public PlazasController(IPlazaService plazaService)
        {
            _plazaService = plazaService;
        }

        // GET
        [HttpGet(Name = "GetAllPlazas")]
        public ActionResult<IEnumerable<PlazaDto>> GetPlazas([FromQuery] PlazaQueryParameters query)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var plazas = _plazaService.GetAllPlazas(query);
                return Ok(plazas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("{id}", Name = "GetPlaza")]
        public IActionResult GetPlaza(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var plaza = _plazaService.GetPlazaById(id);
                return Ok(plaza);
            }
            catch (KeyNotFoundException)
            {
                return NotFound($"La plaza de id {id} no existe");
            }
        }

        // POST
        [Authorize]
        [HttpPost]
        public IActionResult CreatePlaza([FromBody] PlazaCreateDto plazaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_plazaService.EsAdmin(HttpContext.User))
            {
                return Forbid();
            }

            try
            {
                var plazaCreada = _plazaService.CreatePlaza(plazaDto);
                return Ok(plazaCreada.Id);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT
        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdatePlaza(int id, [FromBody] PlazaUpdateDto plazaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_plazaService.EsAdmin(HttpContext.User))
            {
                return Forbid();
            }

            try
            {
                // Convertir PlazaUpdateDto a PlazaCreateDto (temporal)
                var plazaCreateDto = new PlazaCreateDto
                {
                    Numero = plazaDto.Numero,
                    Tipo = plazaDto.Tipo,
                    PrecioHora = plazaDto.PrecioHora,
                    Disponible = plazaDto.Disponible
                };
                
                _plazaService.UpdatePlaza(id, plazaCreateDto);
                return Ok($"Plaza {id} actualizada correctamente");
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        // DELETE
        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeletePlaza(int id)
        {
            if (!_plazaService.EsAdmin(HttpContext.User))
            {
                return Forbid();
            }

            try
            {
                _plazaService.DeletePlaza(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
