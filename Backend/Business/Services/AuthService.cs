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
        private readonly IConfiguration _configuration;
        private readonly IUsuarioRepository _repository;

        public AuthService(IConfiguration configuration,
                           IUsuarioRepository repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public string Login(UsuarioLoginDto usuarioLoginDto)
        {
            var usuario = _repository.GetByEmail(usuarioLoginDto.Correo);

            if (usuario == null)
            {
                throw new UnauthorizedAccessException("Correo o contraseña incorrectos");
            }

            if (usuario.HashContrasena != usuarioLoginDto.Password)
            {
                throw new UnauthorizedAccessException("Correo o contraseña incorrectos");
            }

            var userRead = new UsuarioReadDto()
            {
                Id = usuario.Id,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                FechaCreacion = usuario.FechaCreacion
            };

            return GenerateToken(userRead);
        }

        public string Register(UsuarioRegisterDto usuarioRegisterDto)
        {
            if (usuarioRegisterDto.Password != usuarioRegisterDto.ConfirmPassword)
            {
                throw new InvalidOperationException("Las contraseñas no coinciden");
            }

            var usuarioExistente = _repository.GetByEmail(usuarioRegisterDto.Correo);

            if (usuarioExistente != null)
            {
                throw new InvalidOperationException("Ya existe un usuario con ese correo.");
            }

            var nuevoUsuario = new Usuario
            {
                Correo = usuarioRegisterDto.Correo,
                HashContrasena = usuarioRegisterDto.Password,
                SaltContrasena = new byte[0],
                Rol = "User",
                FechaCreacion = DateTime.Now
            };

            var usuarioCreado = _repository.Add(nuevoUsuario);

            var userRead = new UsuarioReadDto()
            {
                Id = usuarioCreado.Id,
                Correo = usuarioCreado.Correo,
                Rol = usuarioCreado.Rol,
                FechaCreacion = usuarioCreado.FechaCreacion
            };

            return GenerateToken(userRead);
        }

        public string GenerateToken(UsuarioReadDto usuarioReadDto)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:ValidIssuer"],
                Audience = _configuration["JWT:ValidAudience"],
                Subject = new ClaimsIdentity(new Claim[]
                        {
                            new Claim(ClaimTypes.NameIdentifier, Convert.ToString(usuarioReadDto.Id)),
                            new Claim(ClaimTypes.Email, usuarioReadDto.Correo),
                            new Claim(ClaimTypes.Role, usuarioReadDto.Rol),
                        }),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                            SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return tokenString;
        }

        public bool VerificarAcceso(int id, ClaimsPrincipal user)
        {
            var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return false;
            }

            var isOwnResource = (userId == id);

            var rolClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            if (rolClaim != null)
            {
                var isAdmin = rolClaim.Value == "Admin";

                if (isOwnResource || isAdmin)
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
