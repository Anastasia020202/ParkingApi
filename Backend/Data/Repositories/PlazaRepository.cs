using ParkingApi.Models;

namespace ParkingApi.Data.Repositories;

public class PlazaRepository : IPlazaRepository
{

    private readonly ParkingDbContext _context;

    public PlazaRepository(ParkingDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Plaza> GetAllPlazas(PlazaQueryParameters queryParams)
    {
        var query = _context.Plazas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParams.Tipo))
        {
            query = query.Where(p => p.Tipo.ToLower().Contains(queryParams.Tipo.ToLower()));
        }

        if (queryParams.SoloDisponibles)
        {
            query = query.Where(p => p.Disponible);
        }

        if (queryParams.PrecioMin.HasValue)
        {
            query = query.Where(p => p.PrecioHora >= queryParams.PrecioMin.Value);
        }

        if (queryParams.PrecioMax.HasValue)
        {
            query = query.Where(p => p.PrecioHora <= queryParams.PrecioMax.Value);
        }

        if (!string.IsNullOrWhiteSpace(queryParams.OrderBy))
        {
            switch (queryParams.OrderBy.ToLower())
            {
                case "precio":
                    query = queryParams.Desc
                        ? query.OrderByDescending(p => p.PrecioHora)
                        : query.OrderBy(p => p.PrecioHora);
                    break;
                case "numero":
                    query = queryParams.Desc
                        ? query.OrderByDescending(p => p.Numero)
                        : query.OrderBy(p => p.Numero);
                    break;
                case "id":
                    query = queryParams.Desc
                        ? query.OrderByDescending(p => p.Id)
                        : query.OrderBy(p => p.Id);
                    break;
                default:
                    break;
            }
        }

        var result = query.ToList();

        return result;
    }

    public Plaza GetPlaza(int id)
    {
        var plaza = _context.Plazas.FirstOrDefault(plaza => plaza.Id == id);
        if (plaza is null)
        {
            throw new KeyNotFoundException("Plaza no encontrada");
        }
        return plaza;
    }

    public void AddPlaza(Plaza plaza)
    {
        _context.Plazas.Add(plaza);
    }

    public void UpdatePlaza(Plaza plaza)
    {
        _context.Plazas.Remove(plaza);
        SaveChanges();
    }

    public void DeletePlaza(int id)
    {
        var plaza = GetPlaza(id);
        if (plaza is null)
        {
            throw new KeyNotFoundException("Plaza no encontrada");
        }
        _context.Plazas.Remove(plaza);
        SaveChanges();
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
