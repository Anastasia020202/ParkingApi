using ParkingApi.DTOs;
using ParkingApi.Models;
using ParkingApi.Repositories;

namespace ParkingApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public AuthService(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<string> LoginAsync(UsuarioLoginDto loginDto)
        {
            // Buscar usuario por email
            var usuario = _usuarioRepository.GetByEmail(loginDto.Email);

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            // Verificar password (por ahora simple, después implementaremos hash)
            if (usuario.Password != loginDto.Password)
                throw new Exception("Password incorrecto");

            if (!usuario.Activo)
                throw new Exception("Usuario inactivo");

            // Por ahora retornamos un token simple, después implementaremos JWT
            return $"token_simple_{usuario.Id}_{usuario.Rol}";
        }

        public async Task<UsuarioReadDto> RegisterAsync(UsuarioRegisterDto registerDto)
        {
            // Validar que las passwords coincidan
            if (registerDto.Password != registerDto.ConfirmPassword)
                throw new Exception("Las passwords no coinciden");

            // Verificar que el email no exista
            var usuarioExistente = _usuarioRepository.GetByEmail(registerDto.Email);

            if (usuarioExistente != null)
                throw new Exception("El email ya está registrado");

            // Crear nuevo usuario
            var nuevoUsuario = new Usuario
            {
                Email = registerDto.Email,
                Password = registerDto.Password, // Después implementaremos hash
                Rol = "User", // Por defecto es User
                FechaCreacion = DateTime.UtcNow,
                Activo = true
            };

            var usuarioCreado = _usuarioRepository.Add(nuevoUsuario);

            // Retornar DTO sin password
            return new UsuarioReadDto
            {
                Id = usuarioCreado.Id,
                Email = usuarioCreado.Email,
                Rol = usuarioCreado.Rol,
                FechaCreacion = usuarioCreado.FechaCreacion,
                Activo = usuarioCreado.Activo
            };
        }
    }
}
