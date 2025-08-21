using Microsoft.AspNetCore.Mvc;
using ParkingApi.Models;

namespace ParkingApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlazasController : ControllerBase
{
    // Lista temporal 
    private static List<Plaza> _plazas = new List<Plaza>();
    
    // Métodos CRUD básicos
    [HttpGet]
    public ActionResult<IEnumerable<Plaza>> GetPlazas()
    
    [HttpGet("{id}")]
    public ActionResult<Plaza> GetPlaza(int id)
    
    [HttpPost]
    public ActionResult<Plaza> CreatePlaza(Plaza plaza)
    
    [HttpPut("{id}")]
    public ActionResult<Plaza> UpdatePlaza(int id, Plaza plaza)
    
    [HttpDelete("{id}")]
    public ActionResult DeletePlaza(int id)
}
