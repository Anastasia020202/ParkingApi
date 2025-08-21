using ParkingApi.Models;
using ParkingApi.Repositories;
using ParkingApi.Services;

namespace ParkingApi.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;

        public ReservaService(IReservaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Reserva> GetAllReservas()
        {
            return _repository.GetAll();
        }

        public Reserva? GetReservaById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<Reserva> GetReservasByUsuarioId(int usuarioId)
        {
            return _repository.GetByUsuarioId(usuarioId);
        }

        public Reserva CreateReserva(Reserva reserva)
        {
            return _repository.Add(reserva);
        }

        public Reserva? UpdateReserva(int id, Reserva reserva)
        {
            return _repository.Update(id, reserva);
        }

        public bool DeleteReserva(int id)
        {
            return _repository.Delete(id);
        }
    }
}
