using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required, MaxLength(20)]
        public string NumeroTicket { get; set; } = "";

        [Required]
        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;

        [Required]
        public decimal Importe { get; set; }

        [Required, MaxLength(20)]
        public string Estado { get; set; } = "Emitido"; // Emitido, Pagado, Anulado

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        // Relación 1:1 con Reserva
        public int ReservaId { get; set; }
        public Reserva? Reserva { get; set; }
    }
}
