using ParkingApi.Data;
using ParkingApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingApi.Data.Repositories
{
    public class VehiculoRepository : IVehiculoRepository
    {
        private readonly ParkingDbContext _context;

        public VehiculoRepository(ParkingDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Vehiculo?> GetById(int id)
        {
            return await _context.Vehiculos
                .Include(v => v.Usuario)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<IEnumerable<Vehiculo>> GetAll()
        {
            return await _context.Vehiculos
                .Include(v => v.Usuario)
                .Where(v => v.Activo)
                .ToListAsync();
        }

        public async Task<Vehiculo> Add(Vehiculo vehiculo)
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();
            return vehiculo;
        }

        public async Task<Vehiculo?> Update(int id, Vehiculo vehiculo)
        {
            var existing = await _context.Vehiculos.FindAsync(id);
            if (existing == null) return null;

            existing.Matricula = vehiculo.Matricula;
            existing.Marca = vehiculo.Marca;
            existing.Modelo = vehiculo.Modelo;
            existing.Color = vehiculo.Color;
            existing.Activo = vehiculo.Activo;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task<bool> Delete(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);
            if (vehiculo == null)
                return false;

            vehiculo.Activo = false; // Soft delete
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Vehiculo>> GetByUsuario(int usuarioId)
        {
            return await _context.Vehiculos
                .Include(v => v.Usuario)
                .Where(v => v.UsuarioId == usuarioId && v.Activo)
                .ToListAsync();
        }
    }
}