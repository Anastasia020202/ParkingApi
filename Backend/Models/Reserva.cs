using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Models
{
    public class Reserva
    {
        public int Id { get; set; }
        
        [Required]
        public int UsuarioId { get; set; }
        
        [Required]
        public int PlazaId { get; set; }
        
        public int? VehiculoId { get; set; }
        
        [Required]
        public DateTime FechaInicio { get; set; }
        
        public DateTime? FechaFin { get; set; }
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El total debe ser mayor o igual a 0")]
        public decimal TotalAPagar { get; set; }
        
        [Required]
        public string Estado { get; set; } = "Activa";
        
        // Navegaciones
        public Usuario Usuario { get; set; } = null!;
        public Plaza Plaza { get; set; } = null!;
        public Vehiculo? Vehiculo { get; set; }
        public Ticket? Ticket { get; set; }
    }
}
