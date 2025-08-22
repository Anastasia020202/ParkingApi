using ParkingApi.Models;

namespace ParkingApi.Business.Services
{
    public interface IPlazaService
    {
        IEnumerable<Plaza> GetAllPlazas(PlazaQueryParameters? queryParameters = null);
        Plaza? GetPlazaById(int id);
        Plaza CreatePlaza(Plaza plaza);
        Plaza? UpdatePlaza(int id, Plaza plaza);
        bool DeletePlaza(int id);
    }
}
