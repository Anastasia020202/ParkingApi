using ParkingApi.Models;
using ParkingApi.Data.Repositories;

namespace ParkingApi.Business.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _repository;

        public ReservaService(IReservaRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Reserva> GetAllReservas(ReservaQueryParameters? queryParameters = null)
        {
            var reservas = _repository.GetAll().AsQueryable();

            // Aplicar filtros
            if (queryParameters != null)
            {
                if (queryParameters.UsuarioId.HasValue)
                    reservas = reservas.Where(r => r.UsuarioId == queryParameters.UsuarioId.Value);

                if (queryParameters.Desde.HasValue)
                    reservas = reservas.Where(r => r.FechaInicio >= queryParameters.Desde.Value);

                if (queryParameters.Hasta.HasValue)
                    reservas = reservas.Where(r => r.FechaInicio <= queryParameters.Hasta.Value);

                if (!string.IsNullOrEmpty(queryParameters.Estado))
                    reservas = reservas.Where(r => r.Estado == queryParameters.Estado);
            }

            // Aplicar ordenación
            reservas = queryParameters?.OrderBy?.ToLower() switch
            {
                "fechaInicio" => reservas.OrderBy(r => r.FechaInicio),
                "fechaInicio_desc" => reservas.OrderByDescending(r => r.FechaInicio),
                "usuario" => reservas.OrderBy(r => r.UsuarioId),
                "usuario_desc" => reservas.OrderByDescending(r => r.UsuarioId),
                "plaza" => reservas.OrderBy(r => r.PlazaId),
                "plaza_desc" => reservas.OrderByDescending(r => r.PlazaId),
                "total" => reservas.OrderBy(r => r.TotalAPagar),
                "total_desc" => reservas.OrderByDescending(r => r.TotalAPagar),
                _ => reservas.OrderBy(r => r.Id)
            };

            return reservas.ToList();
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
