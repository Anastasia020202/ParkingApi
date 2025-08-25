namespace ParkingApi.Models.DTOs
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string NumeroTicket { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public int ReservaId { get; set; }
    }

    public class TicketReadDto
    {
        public int Id { get; set; }
        public string NumeroTicket { get; set; } = string.Empty;
        public DateTime FechaEmision { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string? Observaciones { get; set; }
        public string UsuarioCorreo { get; set; } = string.Empty;
        public string PlazaNumero { get; set; } = string.Empty;
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }
}





