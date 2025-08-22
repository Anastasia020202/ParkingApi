using ParkingApi.Models;

namespace ParkingApi.Data.Repositories
{
    public interface IVehiculoRepository
    {
        IEnumerable<Vehiculo> GetAll();
        Vehiculo? GetById(int id);
        IEnumerable<Vehiculo> GetByUsuarioId(int usuarioId);
        Vehiculo Add(Vehiculo vehiculo);
        Vehiculo? Update(int id, Vehiculo vehiculo);
        bool Delete(int id);
    }
}
