using ParkingApi.Models;
using ParkingApi.Models.DTOs;
using ParkingApi.Data.Repositories;
using System.Security.Claims;

namespace ParkingApi.Business.Services;

public class PlazaService : IPlazaService
{
    private readonly IPlazaRepository _repository;

    public PlazaService(IPlazaRepository repository)
    {
        _repository = repository;
    }

    // Create
    public Plaza CreatePlaza(PlazaCreateDto plaza)
    {
        var nuevaPlaza = new Plaza
        {
            Numero = plaza.Numero,
            Tipo = plaza.Tipo,
            PrecioHora = plaza.PrecioHora,
            Disponible = plaza.Disponible
        };
        _repository.AddPlaza(nuevaPlaza);
        _repository.SaveChanges();
        // No se devuelve una plaza sin ID, porque se devuelve el objeto del repositorio
        return nuevaPlaza;
    }

    // Read
    public IEnumerable<Plaza> GetAllPlazas(PlazaQueryParameters query)
    {
        return _repository.GetAllPlazas(query);
    }

    public Plaza GetPlazaById(int id)
    {
        var plaza = _repository.GetPlaza(id);
        if (plaza == null)
        {
            throw new KeyNotFoundException($"No hay plazas con el id {id}");
        }
        return plaza;
    }

    // Update
    public void UpdatePlaza(int id, PlazaCreateDto crearPlaza)
    {
        var plaza = _repository.GetPlaza(id);

        if (plaza == null)
        {
            throw new KeyNotFoundException($"No hay plazas con el id {id}");
        }

        plaza.Numero = crearPlaza.Numero;
        plaza.Tipo = crearPlaza.Tipo;
        plaza.PrecioHora = crearPlaza.PrecioHora;
        plaza.Disponible = crearPlaza.Disponible;
        // Id no se modifica

        
        _repository.SaveChanges();
    }

    // Delete
    public void DeletePlaza(int id)
    {
        var plaza = _repository.GetPlaza(id);
        if (plaza == null)
        {
            throw new KeyNotFoundException($"No hay plazas con el id {id}");
        }

        _repository.DeletePlaza(id);
        _repository.SaveChanges();
    }

    // Autorizar
    public bool EsAdmin(ClaimsPrincipal user)
    {
        var rol = user.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role);

        if (rol == null)
        {
            return false;
        }

        var claimValue = rol.Value;

        return claimValue == "Admin";
    }
}
