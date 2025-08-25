namespace ParkingApi.Models.DTOs
{
    public class PlazaDto
    {
        public int Id { get; set; }
        public string Numero { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public bool Disponible { get; set; }
        public decimal PrecioHora { get; set; }
    }

    public class PlazaCreateDto
    {
        public string Numero { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal PrecioHora { get; set; }
        public bool Disponible { get; set; } = true;
    }

    public class PlazaUpdateDto
    {
        public string Numero { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public decimal PrecioHora { get; set; }
        public bool Disponible { get; set; }
    }
}





