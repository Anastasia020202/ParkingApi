using ParkingApi.Models;
using ParkingApi.Repositories;

namespace ParkingApi.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private static List<Vehiculo> _vehiculos = new List<Vehiculo>();

        public IEnumerable<Vehiculo> GetAll()
        {
            return _vehiculos;
        }

        public Vehiculo? GetById(int id)
        {
            return _vehiculos.FirstOrDefault(v => v.Id == id);
        }

        public IEnumerable<Vehiculo> GetByUsuarioId(int usuarioId)
        {
            return _vehiculos.Where(v => v.UsuarioId == usuarioId);
        }

        public Vehiculo Add(Vehiculo vehiculo)
        {
            vehiculo.Id = _vehiculos.Count > 0 ? _vehiculos.Max(v => v.Id) + 1 : 1;
            _vehiculos.Add(vehiculo);
            return vehiculo;
        }

        public Vehiculo? Update(int id, Vehiculo vehiculo)
        {
            var existingVehiculo = _vehiculos.FirstOrDefault(v => v.Id == id);
            if (existingVehiculo == null)
                return null;

            existingVehiculo.UsuarioId = vehiculo.UsuarioId;
            existingVehiculo.Matricula = vehiculo.Matricula;
            existingVehiculo.Marca = vehiculo.Marca;
            existingVehiculo.Modelo = vehiculo.Modelo;
            existingVehiculo.Color = vehiculo.Color;

            return existingVehiculo;
        }

        public bool Delete(int id)
        {
            var vehiculo = _vehiculos.FirstOrDefault(v => v.Id == id);
            if (vehiculo == null)
                return false;

            return _vehiculos.Remove(vehiculo);
        }
    }
}
