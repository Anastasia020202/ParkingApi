namespace ParkingApi.DTOs
{
    public class UsuarioReadDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
    }
}
