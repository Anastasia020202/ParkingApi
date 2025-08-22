namespace ParkingApi.Models.DTOs
{
    public class UsuarioReadDto
    {
        // Constructor sin parámetros para deserialización
        public UsuarioReadDto() { }

        // Constructor con parámetros para creación
        public UsuarioReadDto(int id, string correo, string rol, DateTime fechaCreacion)
        {
            Id = id;
            Correo = correo;
            Rol = rol;
            FechaCreacion = fechaCreacion;
        }

        public int Id { get; set; }
        public string Correo { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
    }
}
