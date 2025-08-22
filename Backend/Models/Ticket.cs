using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        
        [Required]
        public int ReservaId { get; set; }
        
        [Required]
        public string NumeroTicket { get; set; } = "";
        
        [Required]
        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
        
        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "El importe debe ser mayor o igual a 0")]
        public decimal Importe { get; set; }
        
        [Required]
        public string Estado { get; set; } = "Pendiente";
        
        public string? Observaciones { get; set; }
        
        // Navegación
        public Reserva Reserva { get; set; } = null!;
    }
}
