namespace ParkingApi.Models.DTOs
{
    public class VehiculoDto
    {
        public int Id { get; set; }
        public string Matricula { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int UsuarioId { get; set; }
    }

    public class VehiculoCreateDto
    {
        public string Matricula { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string? Color { get; set; }
    }

    public class VehiculoUpdateDto
    {
        public string Matricula { get; set; } = string.Empty;
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public string? Color { get; set; }
    }
}

