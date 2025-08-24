using System.Security.Claims;
using ParkingApi.Models;
using ParkingApi.Models.DTOs;

namespace ParkingApi.Business.Services;

public interface IUsuarioService
{
    public UsuarioReadDto GetUsuarioById(int id);
    public IEnumerable<UsuarioReadDto> GetAllUsuarios(UsuarioQueryParameters query);
    public bool EsAdmin(ClaimsPrincipal user);
    public bool TieneAcceso(int userId, ClaimsPrincipal user);
    public IEnumerable<Reserva> GetReservasByUser(int userId);
    public Reserva GetReservaByUserAndId(int userId, int reservaId);
}
