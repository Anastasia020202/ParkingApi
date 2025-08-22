using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Models
{
    public class Reserva
    {
        public int Id { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public decimal TotalAPagar { get; set; }

        [Required, MaxLength(20)]
        public string Estado { get; set; } = "Pendiente"; // Pendiente, Activa, Finalizada, Cancelada

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        // Relaciones
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int? VehiculoId { get; set; } // Opcional para SetNull
        public Vehiculo? Vehiculo { get; set; }

        public int PlazaId { get; set; }
        public Plaza? Plaza { get; set; }

        // Relación 1:1 con Ticket
        public Ticket? Ticket { get; set; }
    }
}

