using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        
        [Required]
        public string Matricula { get; set; } = "";
        
        [Required]
        public string Marca { get; set; } = "";
        
        [Required]
        public string Modelo { get; set; } = "";
        
        public string? Color { get; set; }
        
        public int? Año { get; set; }
        
        // Navegaciones
        public Usuario Usuario { get; set; } = null!;
        public List<Reserva>? Reservas { get; set; }
    }
}
