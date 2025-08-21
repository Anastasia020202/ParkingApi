namespace ParkingApi.Models
{
    public class PlazaQueryParameters
    {
        public bool? Ocupada { get; set; }
        public string? Tipo { get; set; }
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public string? Orden { get; set; } = "id";
        public bool? SoloDisponibles { get; set; }
    }
}
