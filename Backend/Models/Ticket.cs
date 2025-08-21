namespace ParkingApi.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public int ReservaId { get; set; }
        public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
        public decimal Importe { get; set; } = 0.0m;
        public bool Pagado { get; set; } = false;
        public string NumeroTicket { get; set; } = string.Empty;
    }
}
