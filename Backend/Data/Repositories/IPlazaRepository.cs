using ParkingApi.Models;

namespace ParkingApi.Data.Repositories;

public interface IPlazaRepository
{

    public IEnumerable<Plaza> GetAllPlazas(PlazaQueryParameters query);
    public Plaza GetPlaza(int id);
    public void AddPlaza(Plaza plaza);
    public void UpdatePlaza(Plaza plaza);
    public void DeletePlaza(int id);

    public void SaveChanges();
}
