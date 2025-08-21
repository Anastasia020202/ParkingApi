using ParkingApi.Models;
using ParkingApi.Repositories;

namespace ParkingApi.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private static List<Reserva> _reservas = new List<Reserva>();

        public IEnumerable<Reserva> GetAll()
        {
            return _reservas;
        }

        public Reserva? GetById(int id)
        {
            return _reservas.FirstOrDefault(r => r.Id == id);
        }

        public IEnumerable<Reserva> GetByUsuarioId(int usuarioId)
        {
            return _reservas.Where(r => r.UsuarioId == usuarioId);
        }

        public Reserva Add(Reserva reserva)
        {
            reserva.Id = _reservas.Count > 0 ? _reservas.Max(r => r.Id) + 1 : 1;
            _reservas.Add(reserva);
            return reserva;
        }

        public Reserva? Update(int id, Reserva reserva)
        {
            var existingReserva = _reservas.FirstOrDefault(r => r.Id == id);
            if (existingReserva == null)
                return null;

            existingReserva.UsuarioId = reserva.UsuarioId;
            existingReserva.PlazaId = reserva.PlazaId;
            existingReserva.FechaInicio = reserva.FechaInicio;
            existingReserva.FechaFin = reserva.FechaFin;
            existingReserva.TotalAPagar = reserva.TotalAPagar;

            return existingReserva;
        }

        public bool Delete(int id)
        {
            var reserva = _reservas.FirstOrDefault(r => r.Id == id);
            if (reserva == null)
                return false;

            return _reservas.Remove(reserva);
        }
    }
}
