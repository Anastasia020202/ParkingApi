using System.Security.Claims;
using ParkingApi.Models;
using ParkingApi.Models.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingApi.Business.Services
{
    public interface IVehiculoService
    {
        Task<IEnumerable<Vehiculo>> GetAllVehiculos();
        Task<Vehiculo> GetVehiculoById(int id);
        Task<IEnumerable<Vehiculo>> GetVehiculosByUsuario(int usuarioId);
        Task<Vehiculo> CreateVehiculo(VehiculoCreateDto vehiculoDto, int usuarioId);
        Task<Vehiculo> UpdateVehiculo(int id, VehiculoUpdateDto vehiculoDto);
        Task<bool> DeleteVehiculo(int id);
        Task<IEnumerable<Vehiculo>> GetVehiculosByUsuarioId(int usuarioId);
        
        // Autorizar
        bool EsAdmin(ClaimsPrincipal user);
        Task<bool> TieneAcceso(int id, ClaimsPrincipal user);
    }
}
