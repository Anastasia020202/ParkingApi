using System.Security.Claims;
using ParkingApi.Models;
using ParkingApi.Models.DTOs;
using ParkingApi.Data.Repositories;

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
        public Vehiculo CreateVehiculo(VehiculoCreateDto vehiculoDto, int usuarioId)
        {
            var nuevoVehiculo = new Vehiculo
            {
                Matricula = vehiculoDto.Matricula,
                Marca = vehiculoDto.Marca,
                Modelo = vehiculoDto.Modelo,
                Color = vehiculoDto.Color ?? string.Empty,
                UsuarioId = usuarioId,
                FechaRegistro = DateTime.UtcNow
            };

            return _repository.Add(nuevoVehiculo);
        }

        // Read
        public IEnumerable<Vehiculo> GetAllVehiculos()
        {
            return _repository.GetAll().ToList();
        }

        public Vehiculo GetVehiculoById(int id)
        {
            var vehiculo = _repository.GetById(id);
            if (vehiculo == null)
            {
                throw new KeyNotFoundException($"No hay vehículos con el id {id}");
            }
            return vehiculo;
        }

        public IEnumerable<Vehiculo> GetVehiculosByUsuario(int usuarioId)
        {
            return _repository.GetByUsuario(usuarioId).ToList();
        }

        // Update
        public Vehiculo UpdateVehiculo(int id, VehiculoUpdateDto vehiculoDto)
        {
            var vehiculoExistente = _repository.GetById(id);
            if (vehiculoExistente == null)
            {
                throw new KeyNotFoundException($"No hay vehículos con el id {id}");
            }

            vehiculoExistente.Matricula = vehiculoDto.Matricula;
            vehiculoExistente.Marca = vehiculoDto.Marca;
            vehiculoExistente.Modelo = vehiculoDto.Modelo;
            vehiculoExistente.Color = vehiculoDto.Color ?? string.Empty;

            var vehiculoActualizado = _repository.Update(id, vehiculoExistente);
            if (vehiculoActualizado == null)
            {
                throw new InvalidOperationException("No se pudo actualizar el vehículo");
            }

            return vehiculoActualizado;
        }

        // Delete
        public bool DeleteVehiculo(int id)
        {
            var vehiculo = _repository.GetById(id);
            if (vehiculo == null)
            {
                throw new KeyNotFoundException($"No hay vehículos con el id {id}");
            }

            vehiculo.Activo = false; // Soft delete
            _repository.Update(id, vehiculo);
            return true;
        }

        // Autorizar
        public bool EsAdmin(ClaimsPrincipal user)
        {
            var rol = user.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role);
            return rol != null && rol.Value == "Admin";
        }

        public bool TieneAcceso(int id, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return false;
            }

            var vehiculo = _repository.GetById(id);
            if (vehiculo == null)
            {
                return false;
            }

            var isOwnResource = (vehiculo.UsuarioId == userId);

            var rolClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var isAdmin = rolClaim != null && rolClaim.Value == "Admin";

            return isOwnResource || isAdmin;
        }

        public IEnumerable<Vehiculo> GetVehiculosByUsuarioId(int userId)
        {
            return GetVehiculosByUsuario(userId).ToList();
        }
    }
}