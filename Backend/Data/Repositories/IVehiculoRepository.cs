using ParkingApi.Models;
using System.Collections.Generic;

namespace ParkingApi.Data.Repositories
{
    public interface IVehiculoRepository
    {
        Vehiculo? GetById(int id);
        IEnumerable<Vehiculo> GetAll();
        Vehiculo Add(Vehiculo vehiculo);
        Vehiculo? Update(int id, Vehiculo vehiculo);
        bool Delete(int id);
        IEnumerable<Vehiculo> GetByUsuario(int usuarioId);
    }
}