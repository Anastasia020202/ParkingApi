namespace ParkingApi.Models
{
    public class Vehiculo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string Matricula { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
