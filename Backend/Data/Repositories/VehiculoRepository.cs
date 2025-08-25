using ParkingApi.Data;
using ParkingApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ParkingApi.Data.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly ParkingDbContext _context;

        public VehiculoRepository(ParkingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Vehiculo? GetById(int id)
        {
            return _context.Vehiculos
                .Include(v => v.Usuario)
                .FirstOrDefault(v => v.Id == id);
        }

        public IEnumerable<Vehiculo> GetAll()
        {
            return _context.Vehiculos
                .Include(v => v.Usuario)
                .Where(v => v.Activo)
                .ToList();
        }

        public Vehiculo Add(Vehiculo vehiculo)
        {
            _context.Vehiculos.Add(vehiculo);
            _context.SaveChanges();
            return vehiculo;
        }

        public Vehiculo? Update(int id, Vehiculo vehiculo)
        {
            var existing = _context.Vehiculos.Find(id);
            if (existing == null) return null;

            existing.Matricula = vehiculo.Matricula;
            existing.Marca = vehiculo.Marca;
            existing.Modelo = vehiculo.Modelo;
            existing.Color = vehiculo.Color;
            existing.Activo = vehiculo.Activo;

            _context.SaveChanges();
            return existing;
        }

        public bool Delete(int id)
        {
            var vehiculo = _context.Vehiculos.Find(id);
            if (vehiculo == null)
                return false;

            vehiculo.Activo = false; // Soft delete
            _context.SaveChanges();
            return true;
        }

        public IEnumerable<Vehiculo> GetByUsuario(int usuarioId)
        {
            return _context.Vehiculos
                .Include(v => v.Usuario)
                .Where(v => v.UsuarioId == usuarioId && v.Activo)
                .ToList();
        }
    }
}