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

        public IEnumerable<Reserva> GetAllReservas(ReservaQueryParameters? queryParameters = null)
        {
            var reservas = _repository.GetAll().AsQueryable();

            // Aplicar filtros
            if (queryParameters != null)
            {
                if (queryParameters.UsuarioId.HasValue)
                    reservas = reservas.Where(r => r.UsuarioId == queryParameters.UsuarioId.Value);

                if (queryParameters.PlazaId.HasValue)
                    reservas = reservas.Where(r => r.PlazaId == queryParameters.PlazaId.Value);

                if (queryParameters.FechaDesde.HasValue)
                    reservas = reservas.Where(r => r.FechaInicio >= queryParameters.FechaDesde.Value);

                if (queryParameters.FechaHasta.HasValue)
                    reservas = reservas.Where(r => r.FechaInicio <= queryParameters.FechaHasta.Value);

                if (queryParameters.SoloActivas == true)
                    reservas = reservas.Where(r => r.FechaFin == null || r.FechaFin > DateTime.Now);
            }

            // Aplicar ordenación
            reservas = queryParameters?.Orden?.ToLower() switch
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
