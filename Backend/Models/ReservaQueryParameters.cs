namespace ParkingApi.Models
{
    public class ReservaQueryParameters
    {
        public int? UsuarioId { get; set; }
        public int? PlazaId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public string? Orden { get; set; } = "id";
        public bool? SoloActivas { get; set; }
    }
}
