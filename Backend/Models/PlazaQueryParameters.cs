namespace ParkingApi.Models
{
    public class PlazaQueryParameters
    {
        public string? Tipo { get; set; }
        public bool SoloDisponibles { get; set; } = false;
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }
        public string? Zona { get; set; }
        public string? OrderBy { get; set; } = "Numero";
        public bool Desc { get; set; } = false;
    }
}
