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

        public IEnumerable<Plaza> GetAllPlazas(PlazaQueryParameters? queryParameters = null)
        {
            var plazas = _repository.GetAll().AsQueryable();

            // Aplicar filtros
            if (queryParameters != null)
            {
                if (queryParameters.Ocupada.HasValue)
                    plazas = plazas.Where(p => p.Ocupada == queryParameters.Ocupada.Value);

                if (!string.IsNullOrEmpty(queryParameters.Tipo))
                    plazas = plazas.Where(p => p.Tipo.Contains(queryParameters.Tipo));

                if (queryParameters.PrecioMin.HasValue)
                    plazas = plazas.Where(p => p.PrecioHora >= queryParameters.PrecioMin.Value);

                if (queryParameters.PrecioMax.HasValue)
                    plazas = plazas.Where(p => p.PrecioHora <= queryParameters.PrecioMax.Value);

                if (queryParameters.SoloDisponibles == true)
                    plazas = plazas.Where(p => !p.Ocupada);
            }

            // Aplicar ordenación
            plazas = queryParameters?.Orden?.ToLower() switch
            {
                "precio" => plazas.OrderBy(p => p.PrecioHora),
                "precio_desc" => plazas.OrderByDescending(p => p.PrecioHora),
                "tipo" => plazas.OrderBy(p => p.Tipo),
                "tipo_desc" => plazas.OrderByDescending(p => p.Tipo),
                "numero" => plazas.OrderBy(p => p.Numero),
                "numero_desc" => plazas.OrderByDescending(p => p.Numero),
                _ => plazas.OrderBy(p => p.Id)
            };

            return plazas.ToList();
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
