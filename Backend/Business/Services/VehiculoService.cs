using ParkingApi.Models;
using ParkingApi.Data.Repositories;

namespace ParkingApi.Business.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _repository;

        public VehiculoService(IVehiculoRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Vehiculo> GetAllVehiculos()
        {
            return _repository.GetAll();
        }

        public Vehiculo? GetVehiculoById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Vehiculo> GetVehiculosByUsuarioId(int usuarioId)
        {
            return _repository.GetByUsuarioId(usuarioId);
        }

        public Vehiculo CreateVehiculo(Vehiculo vehiculo)
        {
            return _repository.Add(vehiculo);
        }

        public Vehiculo? UpdateVehiculo(int id, Vehiculo vehiculo)
        {
            return _repository.Update(id, vehiculo);
        }

        public bool DeleteVehiculo(int id)
        {
            return _repository.Delete(id);
        }
    }
}
