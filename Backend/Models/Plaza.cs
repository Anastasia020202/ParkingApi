using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Models
{
    public class Plaza
    {
        public int Id { get; set; }
        
        [Required]
        public string Numero { get; set; } = "";
        
        [Required]
        public string Tipo { get; set; } = "General";
        
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public decimal PrecioHora { get; set; } = 2.5m;
        
        public bool Disponible { get; set; } = true;
        
        public DateTime? ReservadaHasta { get; set; }
        
        // Navegaciones
        public List<Reserva>? Reservas { get; set; }
    }
}
