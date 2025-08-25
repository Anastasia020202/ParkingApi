using System.Security.Claims;
using ParkingApi.Models;
using ParkingApi.Models.DTOs;

namespace ParkingApi.Business.Services
{
    public interface IVehiculoService
    {
        Vehiculo CreateVehiculo(VehiculoCreateDto vehiculoDto, int usuarioId);
        IEnumerable<Vehiculo> GetAllVehiculos();
        Vehiculo GetVehiculoById(int id);
        IEnumerable<Vehiculo> GetVehiculosByUsuario(int usuarioId);
        Vehiculo UpdateVehiculo(int id, VehiculoUpdateDto vehiculoDto);
        bool DeleteVehiculo(int id);
        bool EsAdmin(ClaimsPrincipal user);
        bool TieneAcceso(int id, ClaimsPrincipal user);
        IEnumerable<Vehiculo> GetVehiculosByUsuarioId(int userId);
    }
}