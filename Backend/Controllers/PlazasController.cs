using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ParkingApi.Models;
using ParkingApi.Services;

namespace ParkingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlazasController : ControllerBase
{
    private readonly IPlazaService _plazaService;

    public PlazasController(IPlazaService plazaService)
    {
        _plazaService = plazaService;
    }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Plaza>> GetPlazas(
            [FromQuery] bool? ocupada,
            [FromQuery] string? tipo,
            [FromQuery] decimal? precioMin,
            [FromQuery] decimal? precioMax,
            [FromQuery] string? orden,
            [FromQuery] bool? soloDisponibles)
        {
            var queryParameters = new PlazaQueryParameters
            {
                Ocupada = ocupada,
                Tipo = tipo,
                PrecioMin = precioMin,
                PrecioMax = precioMax,
                Orden = orden,
                SoloDisponibles = soloDisponibles
            };

            var plazas = _plazaService.GetAllPlazas(queryParameters);
            return Ok(plazas);
        }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public ActionResult<Plaza> GetPlaza(int id)
    {
        var plaza = _plazaService.GetPlazaById(id);
        if (plaza == null)
            return NotFound();
        return Ok(plaza);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public ActionResult<Plaza> CreatePlaza(Plaza plaza)
    {
        var createdPlaza = _plazaService.CreatePlaza(plaza);
        return CreatedAtAction(nameof(GetPlaza), new { id = createdPlaza.Id }, createdPlaza);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult<Plaza> UpdatePlaza(int id, Plaza plaza)
    {
        var updatedPlaza = _plazaService.UpdatePlaza(id, plaza);
        if (updatedPlaza == null)
            return NotFound();
        
        return Ok(updatedPlaza);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public ActionResult DeletePlaza(int id)
    {
        var deleted = _plazaService.DeletePlaza(id);
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
}
