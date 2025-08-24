using Microsoft.EntityFrameworkCore;
using ParkingApi.Models;

namespace ParkingApi.Data
{
    public class ParkingDbContext : DbContext
    {
        public ParkingDbContext(DbContextOptions<ParkingDbContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Plaza> Plazas { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Vehiculo> Vehiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuración de Usuario
            modelBuilder.Entity<Usuario>().HasData(
                // Contrasena: admin
                new Usuario
                {
                    Id = 1,
                    Correo = "admin@parking.com",
                    HashContrasena = "admin",
                    SaltContrasena = new byte[0],
                    Rol = "Admin",
                    FechaCreacion = DateTime.Now
                }
            );

            // Configuración de Plaza
            modelBuilder.Entity<Plaza>().HasData(
                new Plaza { Id = 1, Numero = "A1", Tipo = "General", PrecioHora = 2.50m, Disponible = true },
                new Plaza { Id = 2, Numero = "A2", Tipo = "General", PrecioHora = 2.50m, Disponible = true },
                new Plaza { Id = 3, Numero = "B1", Tipo = "Moto", PrecioHora = 1.50m, Disponible = true },
                new Plaza { Id = 4, Numero = "B2", Tipo = "Moto", PrecioHora = 1.50m, Disponible = true },
                new Plaza { Id = 5, Numero = "C1", Tipo = "PMR", PrecioHora = 1.00m, Disponible = true }
            );

            // Configuración de Vehiculo
            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.Property(v => v.FechaRegistro).HasDefaultValueSql("(UTC_TIMESTAMP())");
                entity.Property(v => v.Activo).HasDefaultValue(true);
                entity.Property(v => v.Matricula).IsRequired().HasMaxLength(10);
                entity.Property(v => v.Marca).IsRequired().HasMaxLength(50);
                entity.Property(v => v.Modelo).IsRequired().HasMaxLength(50);
                entity.Property(v => v.Color).HasMaxLength(30);
            });
        }
    }
}

