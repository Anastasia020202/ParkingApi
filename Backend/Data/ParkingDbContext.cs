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
        public DbSet<Ticket> Tickets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de Usuario
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Correo).IsRequired().HasMaxLength(100);
                entity.Property(e => e.HashContrasena).IsRequired();
                entity.Property(e => e.SaltContrasena).IsRequired();
                entity.Property(e => e.Rol).IsRequired().HasMaxLength(20);
                
                // Índice único en Correo
                entity.HasIndex(e => e.Correo).IsUnique();
                
                // Seeding de Admin user (contraseña: "admin")
                entity.HasData(new Usuario
                {
                    Id = 1,
                    Correo = "admin@parking.com",
                    HashContrasena = "admin", // Contraseña en texto plano para desarrollo
                    SaltContrasena = new byte[0], // Sin salt para desarrollo
                    Rol = "Admin",
                    FechaCreacion = DateTime.UtcNow
                });
            });

            // Configuración de Plaza
            modelBuilder.Entity<Plaza>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Numero).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Tipo).IsRequired().HasMaxLength(50);
                
                // Índice único en Numero
                entity.HasIndex(e => e.Numero).IsUnique();
                
                // Seeding de plazas de ejemplo
                entity.HasData(
                    new Plaza { Id = 1, Numero = "A1", Tipo = "General", PrecioHora = 2.50m, Disponible = true },
                    new Plaza { Id = 2, Numero = "A2", Tipo = "General", PrecioHora = 2.50m, Disponible = true },
                    new Plaza { Id = 3, Numero = "B1", Tipo = "Moto", PrecioHora = 1.50m, Disponible = true },
                    new Plaza { Id = 4, Numero = "B2", Tipo = "Moto", PrecioHora = 1.50m, Disponible = true },
                    new Plaza { Id = 5, Numero = "C1", Tipo = "PMR", PrecioHora = 1.00m, Disponible = true }
                );
            });

            // Configuración de Vehiculo
            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Matricula).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Marca).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Modelo).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Color).HasMaxLength(30);
                
                // Índice único en Matricula
                entity.HasIndex(e => e.Matricula).IsUnique();
                
                // Relación con Usuario
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Vehiculos)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración de Reserva
            modelBuilder.Entity<Reserva>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FechaInicio).IsRequired();
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
                
                // Relación con Usuario
                entity.HasOne(e => e.Usuario)
                      .WithMany(u => u.Reservas)
                      .HasForeignKey(e => e.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
                
                // Relación con Plaza
                entity.HasOne(e => e.Plaza)
                      .WithMany(p => p.Reservas)
                      .HasForeignKey(e => e.PlazaId)
                      .OnDelete(DeleteBehavior.Restrict);
                
                // Relación con Vehiculo (opcional)
                entity.HasOne(e => e.Vehiculo)
                      .WithMany(v => v.Reservas)
                      .HasForeignKey(e => e.VehiculoId)
                      .OnDelete(DeleteBehavior.SetNull);
            });

            // Configuración de Ticket
            modelBuilder.Entity<Ticket>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeroTicket).IsRequired().HasMaxLength(20);
                entity.Property(e => e.FechaEmision).IsRequired();
                entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Observaciones).HasMaxLength(500);
                
                // Índice único en NumeroTicket
                entity.HasIndex(e => e.NumeroTicket).IsUnique();
                
                // Relación 1-1 con Reserva
                entity.HasOne(e => e.Reserva)
                      .WithOne(r => r.Ticket)
                      .HasForeignKey<Ticket>(e => e.ReservaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}

