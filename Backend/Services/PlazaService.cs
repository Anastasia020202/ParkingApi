using ParkingApi.Models;
using ParkingApi.Repositories;
using ParkingApi.Services;

namespace ParkingApi.Services
{
    public class PlazaService : IPlazaService
    {
        private readonly IPlazaRepository _repository;

        public PlazaService(IPlazaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Plaza> GetAllPlazas()
        {
            return _repository.GetAll();
        }

        public Plaza? GetPlazaById(int id)
        {
            return _repository.GetById(id);
        }

        public Plaza CreatePlaza(Plaza plaza)
        {
            return _repository.Add(plaza);
        }

        public Plaza? UpdatePlaza(int id, Plaza plaza)
        {
            return _repository.Update(id, plaza);
        }

        public bool DeletePlaza(int id)
        {
            return _repository.Delete(id);
        }
    }
}
