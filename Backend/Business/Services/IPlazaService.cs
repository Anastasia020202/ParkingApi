using System.Security.Claims;
using ParkingApi.Models;
using ParkingApi.Models.DTOs;

namespace ParkingApi.Business.Services;

public interface IPlazaService
{
    // Create
    public Plaza CreatePlaza(PlazaCreateDto plaza);

    // Read
    public IEnumerable<Plaza> GetAllPlazas(PlazaQueryParameters query);
    public Plaza GetPlazaById(int id);

    // Update
    public void UpdatePlaza(int id, PlazaCreateDto plaza);

    // Delete
    public void DeletePlaza(int id);

    // Autorizar
    public bool EsAdmin(ClaimsPrincipal user);
}
