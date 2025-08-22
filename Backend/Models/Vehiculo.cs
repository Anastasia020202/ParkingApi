using System.ComponentModel.DataAnnotations;

namespace ParkingApi.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }

        [Required, MaxLength(10)]
        public string Matricula { get; set; } = "";

        [Required, MaxLength(50)]
        public string Marca { get; set; } = "";

        [Required, MaxLength(50)]
        public string Modelo { get; set; } = "";

        [MaxLength(30)]
        public string Color { get; set; } = "";

        public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

        public bool Activo { get; set; } = true;

        // Relación con usuario
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        // Relación con reservas
        public List<Reserva>? Reservas { get; set; }
    }
}

