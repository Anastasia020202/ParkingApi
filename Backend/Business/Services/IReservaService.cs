using ParkingApi.Models;
using ParkingApi.Models.DTOs;
using System.Security.Claims;

namespace ParkingApi.Business.Services;

public interface IReservaService
{
    public IEnumerable<Reserva> GetAllReservas();
    public IEnumerable<Reserva> GetReservasByUser(int userId);
    public Reserva GetReservaById(int id);
    public Reserva CreateReserva(int userId, ReservaCreateDto reservaCreateDto);
    public bool EsAdmin(ClaimsPrincipal user);
    public bool TieneAcceso(int id, ClaimsPrincipal user);
    public IEnumerable<Reserva> GetReservasByUsuarioId(int userId);
}
