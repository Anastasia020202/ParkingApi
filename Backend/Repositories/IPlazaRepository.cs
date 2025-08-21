using ParkingApi.Models;

namespace ParkingApi.Repositories
{
    public interface IPlazaRepository
    {
        IEnumerable<Plaza> GetAll();
        Plaza? GetById(int id);
        Plaza Add(Plaza plaza);
        Plaza? Update(int id, Plaza plaza);
        bool Delete(int id);
    }
}
