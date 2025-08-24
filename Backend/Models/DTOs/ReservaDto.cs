namespace ParkingApi.Models.DTOs
{
    public class ReservaDto
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int UsuarioId { get; set; }
        public int PlazaId { get; set; }
        public int? VehiculoId { get; set; }
    }

    public class ReservaCreateDto
    {
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public int PlazaId { get; set; }
        public int? VehiculoId { get; set; }
        public decimal Importe { get; set; }
        public string? Estado { get; set; } = "Pendiente";
        public string? Observaciones { get; set; }
    }

    public class ReservaUpdateDto
    {
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public decimal? Importe { get; set; }
        public string? Observaciones { get; set; }
    }

    public class ReservaReadDto
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string UsuarioCorreo { get; set; } = string.Empty;
        public string PlazaNumero { get; set; } = string.Empty;
        public string? VehiculoMatricula { get; set; }
        public decimal Importe { get; set; }
        public string? Observaciones { get; set; }
        public DateTime FechaEmision { get; set; }
        public string NumeroTicket { get; set; } = string.Empty;
    }
}
