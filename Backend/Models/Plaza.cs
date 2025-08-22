using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Models
{
    public class Plaza
    {
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string Numero { get; set; } = "";

        public bool Disponible { get; set; } = true;

        [Required]
        public decimal PrecioHora { get; set; }

        [Required, MaxLength(50)]
        public string Tipo { get; set; } = "General"; // Coche, Moto, PMR...

        public DateTime FechaAlta { get; set; } = DateTime.UtcNow;

        // Relación con reservas
        public List<Reserva>? Reservas { get; set; }
    }
}
