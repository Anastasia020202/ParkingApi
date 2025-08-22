using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        
        [Required]
        [EmailAddress]
        public string Correo { get; set; } = "";
        
        [Required]
        public string HashContrasena { get; set; } = "";
        
        [Required]
        public byte[] SaltContrasena { get; set; } = Array.Empty<byte>();
        
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;
        
        [Required]
        public string Rol { get; set; } = "User";
        
        // Relaciones
        public List<Reserva>? Reservas { get; set; }
        public List<Vehiculo>? Vehiculos { get; set; }
    }
}
