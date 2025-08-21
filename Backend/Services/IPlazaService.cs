using ParkingApi.Models;

namespace ParkingApi.Services
{
    public interface IPlazaService
    {
        IEnumerable<Plaza> GetAllPlazas();
        Plaza? GetPlazaById(int id);
        Plaza CreatePlaza(Plaza plaza);
        Plaza? UpdatePlaza(int id, Plaza plaza);
        bool DeletePlaza(int id);
    }
}
