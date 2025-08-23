using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ParkingApi.Models;
using ParkingApi.Business.Services;

namespace ParkingApi.API.Controllers;

[ApiController]
[Route("[controller]")]
public class PlazasController : ControllerBase
{
    private readonly IPlazaService _plazaService;

    public PlazasController(IPlazaService plazaService)
    {
        _plazaService = plazaService;
    }

    [HttpGet(Name = "GetAllPlazas")]
    public ActionResult<IEnumerable<Plaza>> GetPlazas([FromQuery] PlazaQueryParameters query)
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
    public IActionResult CreatePlaza([FromBody] Plaza plaza)
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
            var plazaCreada = _plazaService.CreatePlaza(plaza);
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
    public IActionResult UpdatePlaza(int id, [FromBody] Plaza plaza)
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
            _plazaService.UpdatePlaza(id, plaza);
            return Ok(id);
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
