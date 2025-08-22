using ParkingApi.Models;

namespace ParkingApi.Services
{
    public interface IVehiculoService
    {
        IEnumerable<Vehiculo> GetAllVehiculos();
        Vehiculo? GetVehiculoById(int id);
        IEnumerable<Vehiculo> GetVehiculosByUsuarioId(int usuarioId);
        Vehiculo CreateVehiculo(Vehiculo vehiculo);
        Vehiculo? UpdateVehiculo(int id, Vehiculo vehiculo);
        bool DeleteVehiculo(int id);
    }
}
