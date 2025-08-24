namespace ParkingApi.Models
{
    public class ReservaQueryParameters
    {
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? Estado { get; set; }
        public int? UsuarioId { get; set; }
        public int? PlazaId { get; set; }
        public int? VehiculoId { get; set; }
        public string? OrderBy { get; set; } = "FechaInicio";
        public bool Desc { get; set; } = false;
    }
}
