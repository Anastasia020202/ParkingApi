using ParkingApi.Models;
using ParkingApi.Models.DTOs;
using ParkingApi.Data.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ParkingApi.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _configuration = configuration;
        }

        public async Task<string> LoginAsync(UsuarioLoginDto loginDto)
        {
            // Buscar usuario por email
            var usuario = _usuarioRepository.GetByEmail(loginDto.Correo);

            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            // Verificar password (por ahora simple, después implementaremos hash)
            if (usuario.HashContrasena != loginDto.Password)
                throw new Exception("Password incorrecto");

            // Generar JWT real
            return GenerateJwtToken(usuario);
        }

        public async Task<UsuarioReadDto> RegisterAsync(UsuarioRegisterDto registerDto)
        {
            // Validar que las passwords coincidan
            if (registerDto.Password != registerDto.ConfirmPassword)
                throw new Exception("Las passwords no coinciden");

            // Verificar que el email no exista
            var usuarioExistente = _usuarioRepository.GetByEmail(registerDto.Correo);

            if (usuarioExistente != null)
                throw new Exception("El email ya está registrado");

            // Crear nuevo usuario
            var nuevoUsuario = new Usuario
            {
                Correo = registerDto.Correo,
                HashContrasena = registerDto.Password, // Después implementaremos hash
                SaltContrasena = new byte[0], // Por ahora vacío
                Rol = "User", // Por defecto es User
                FechaCreacion = DateTime.UtcNow
            };

            var usuarioCreado = _usuarioRepository.Add(nuevoUsuario);

            // Retornar DTO sin password
            return new UsuarioReadDto
            {
                Id = usuarioCreado.Id,
                Correo = usuarioCreado.Correo,
                Rol = usuarioCreado.Rol,
                FechaCreacion = usuarioCreado.FechaCreacion
            };
        }

        private string GenerateJwtToken(Usuario usuario)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
