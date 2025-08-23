using System.Security.Claims;
using ParkingApi.Models;
using ParkingApi.Data.Repositories;

namespace ParkingApi.Business.Services
{
    public class PlazaService : IPlazaService
    {
        private readonly IPlazaRepository _repository;

        public PlazaService(IPlazaRepository repository)
        {
            _repository = repository;
        }

        // Create
        public Plaza CreatePlaza(Plaza plaza)
        {
            var nuevaPlaza = new Plaza
            {
                Numero = plaza.Numero,
                Tipo = plaza.Tipo,
                PrecioHora = plaza.PrecioHora,
                Disponible = plaza.Disponible
            };
            _repository.Add(nuevaPlaza);
            _repository.SaveChanges();
            return nuevaPlaza;
        }

        // Read
        public IEnumerable<Plaza> GetAllPlazas(PlazaQueryParameters? queryParameters = null)
        {
            return _repository.GetAll(queryParameters);
        }

        public Plaza? GetPlazaById(int id)
        {
            var plaza = _repository.GetById(id);
            if (plaza == null)
            {
                throw new KeyNotFoundException($"No hay plazas con el id {id}");
            }
            return plaza;
        }

        // Update
        public Plaza? UpdatePlaza(int id, Plaza plaza)
        {
            var plazaExistente = _repository.GetById(id);

            if (plazaExistente == null)
            {
                throw new KeyNotFoundException($"No hay plazas con el id {id}");
            }

            plazaExistente.Numero = plaza.Numero;
            plazaExistente.Tipo = plaza.Tipo;
            plazaExistente.PrecioHora = plaza.PrecioHora;
            plazaExistente.Disponible = plaza.Disponible;

            _repository.SaveChanges();
            return plazaExistente;
        }

        // Delete
        public bool DeletePlaza(int id)
        {
            var plaza = _repository.GetById(id);
            if (plaza == null)
            {
                throw new KeyNotFoundException($"No hay plazas con el id {id}");
            }

            _repository.Delete(id);
            _repository.SaveChanges();
            return true;
        }

        // Autorizar
        public bool EsAdmin(ClaimsPrincipal user)
        {
            var rol = user.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role);

            if (rol == null)
            {
                return false;
            }

            var claimValue = rol.Value;

            return claimValue == "Admin";
        }
    }
}
