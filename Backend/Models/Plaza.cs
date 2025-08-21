namespace ParkingApi.Models
{
    public class Plaza
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty; 
        public bool Ocupada { get; set; } = false;
        public string Tipo { get; set; } = "General"; 
        public decimal PrecioHora { get; set; } = 2.5m;
        public DateTime? ReservadaHasta { get; set; } 
    }
}
