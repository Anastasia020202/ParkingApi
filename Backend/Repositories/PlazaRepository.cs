using ParkingApi.Models;
using ParkingApi.Repositories;

namespace ParkingApi.Repositories
{
    public class PlazaRepository : IPlazaRepository
    {
        private static List<Plaza> _plazas = new List<Plaza>();

        public IEnumerable<Plaza> GetAll()
        {
            return _plazas;
        }

        public Plaza? GetById(int id)
        {
            return _plazas.FirstOrDefault(p => p.Id == id);
        }

        public Plaza Add(Plaza plaza)
        {
            plaza.Id = _plazas.Count > 0 ? _plazas.Max(p => p.Id) + 1 : 1;
            _plazas.Add(plaza);
            return plaza;
        }

        public Plaza? Update(int id, Plaza plaza)
        {
            var existingPlaza = _plazas.FirstOrDefault(p => p.Id == id);
            if (existingPlaza == null)
                return null;

            existingPlaza.Numero = plaza.Numero;
            existingPlaza.Tipo = plaza.Tipo;
            existingPlaza.PrecioHora = plaza.PrecioHora;
            existingPlaza.Ocupada = plaza.Ocupada;
            existingPlaza.ReservadaHasta = plaza.ReservadaHasta;

            return existingPlaza;
        }

        public bool Delete(int id)
        {
            var plaza = _plazas.FirstOrDefault(p => p.Id == id);
            if (plaza == null)
                return false;

            return _plazas.Remove(plaza);
        }
    }
}
