using Microsoft.AspNetCore.Mvc;
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
    public ActionResult<IEnumerable<Plaza>> GetPlazas()
    {
        var plazas = _plazaService.GetAllPlazas();
        return Ok(plazas);
    }

    [HttpGet("{id}")]
    public ActionResult<Plaza> GetPlaza(int id)
    {
        var plaza = _plazaService.GetPlazaById(id);
        if (plaza == null)
            return NotFound();
        return Ok(plaza);
    }

    [HttpPost]
    public ActionResult<Plaza> CreatePlaza(Plaza plaza)
    {
        var createdPlaza = _plazaService.CreatePlaza(plaza);
        return CreatedAtAction(nameof(GetPlaza), new { id = createdPlaza.Id }, createdPlaza);
    }

    [HttpPut("{id}")]
    public ActionResult<Plaza> UpdatePlaza(int id, Plaza plaza)
    {
        var updatedPlaza = _plazaService.UpdatePlaza(id, plaza);
        if (updatedPlaza == null)
            return NotFound();
        
        return Ok(updatedPlaza);
    }

    [HttpDelete("{id}")]
    public ActionResult DeletePlaza(int id)
    {
        var deleted = _plazaService.DeletePlaza(id);
        if (!deleted)
            return NotFound();
        
        return NoContent();
    }
}
