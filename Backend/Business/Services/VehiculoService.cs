using System.Security.Claims;
using ParkingApi.Models;
using ParkingApi.Models.DTOs;
using ParkingApi.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ParkingApi.Business.Services
{
    public class VehiculoService : IVehiculoService
    {
        private readonly IVehiculoRepository _repository;

        public VehiculoService(IVehiculoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // Create
        public async Task<Vehiculo> CreateVehiculo(VehiculoCreateDto vehiculoDto, int usuarioId)
        {
            var nuevoVehiculo = new Vehiculo
            {
                Matricula = vehiculoDto.Matricula,
                Marca = vehiculoDto.Marca,
                Modelo = vehiculoDto.Modelo,
                Color = vehiculoDto.Color ?? string.Empty,
                UsuarioId = usuarioId,
                FechaRegistro = DateTime.UtcNow // Inicialización según el modelo
            };

            var vehiculoCreado = await _repository.Add(nuevoVehiculo);
            return vehiculoCreado;
        }

        // Read
        public async Task<IEnumerable<Vehiculo>> GetAllVehiculos()
        {
            var vehiculos = await _repository.GetAll();
            return vehiculos.ToList();
        }

        public async Task<Vehiculo> GetVehiculoById(int id)
        {
            var vehiculo = await _repository.GetById(id);
            if (vehiculo == null)
            {
                throw new KeyNotFoundException($"No hay vehículos con el id {id}");
            }
            return vehiculo;
        }

        public async Task<IEnumerable<Vehiculo>> GetVehiculosByUsuario(int usuarioId)
        {
            var vehiculos = await _repository.GetByUsuario(usuarioId);
            return vehiculos.ToList();
        }

        // Update
        public async Task<Vehiculo> UpdateVehiculo(int id, VehiculoUpdateDto vehiculoDto)
        {
            var vehiculoExistente = await _repository.GetById(id);
            if (vehiculoExistente == null)
            {
                throw new KeyNotFoundException($"No hay vehículos con el id {id}");
            }

            vehiculoExistente.Matricula = vehiculoDto.Matricula;
            vehiculoExistente.Marca = vehiculoDto.Marca;
            vehiculoExistente.Modelo = vehiculoDto.Modelo;
            vehiculoExistente.Color = vehiculoDto.Color ?? string.Empty;

            var vehiculoActualizado = await _repository.Update(id, vehiculoExistente);
            if (vehiculoActualizado == null)
            {
                throw new InvalidOperationException("No se pudo actualizar el vehículo");
            }

            return vehiculoActualizado;
        }

        // Delete
        public async Task<bool> DeleteVehiculo(int id)
        {
            var vehiculo = await _repository.GetById(id);
            if (vehiculo == null)
            {
                throw new KeyNotFoundException($"No hay vehículos con el id {id}");
            }

            vehiculo.Activo = false; // Soft delete según el modelo
            await _repository.Update(id, vehiculo);
            return true;
        }

        // Autorizar
        public bool EsAdmin(ClaimsPrincipal user)
        {
            var rol = user.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role);

            if (rol == null)
            {
                return false;
            }

            var claimValue = rol.Value;

            return claimValue == "Admin";
        }

        public async Task<bool> TieneAcceso(int id, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return false;
            }

            var vehiculo = await _repository.GetById(id);
            if (vehiculo == null)
            {
                return false;
            }

            var isOwnResource = (vehiculo.UsuarioId == userId);

            var rolClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (rolClaim != null)
            {
                var isAdmin = rolClaim.Value == "Admin";

                if (isOwnResource || isAdmin)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<IEnumerable<Vehiculo>> GetVehiculosByUsuarioId(int userId)
        {
            var vehiculos = await GetVehiculosByUsuario(userId);
            return vehiculos.ToList();
        }
    }
}