using ParkingApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingApi.Data.Repositories
{
    public interface IVehiculoRepository
    {
        Task<Vehiculo?> GetById(int id);
        Task<IEnumerable<Vehiculo>> GetAll();
        Task<Vehiculo> Add(Vehiculo vehiculo);
        Task<Vehiculo?> Update(int id, Vehiculo vehiculo);
        Task<bool> Delete(int id);
        Task<IEnumerable<Vehiculo>> GetByUsuario(int usuarioId);
    }
}