namespace ParkingApi.DTOs
{
    public class UsuarioReadDto
    {
        // Constructor sin parámetros para deserialización
        public UsuarioReadDto() { }

        // Constructor con parámetros para creación
        public UsuarioReadDto(int id, string email, string rol, DateTime fechaCreacion, bool activo)
        {
            Id = id;
            Email = email;
            Rol = rol;
            FechaCreacion = fechaCreacion;
            Activo = activo;
        }

        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Rol { get; set; } = string.Empty;
        public DateTime FechaCreacion { get; set; }
        public bool Activo { get; set; }
    }
}
