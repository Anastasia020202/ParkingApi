using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using ParkingApi.Models;
using ParkingApi.Models.DTOs;
using ParkingApi.Data.Repositories;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ParkingApi.Business.Services;

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

    public string Register(UsuarioRegisterDto usuarioRegisterDto)
    {
        var correo = usuarioRegisterDto.Correo;
        var salt = GenerateSalt();
        var hash = HashPassword(usuarioRegisterDto.Password, salt);
        var user = _repository.AddUsuarioFromCredentials(correo, hash, salt);

        var userRead = new UsuarioReadDto()
        {
            Id = user.Id,
            Correo = user.Correo,
            Rol = user.Rol,
            FechaCreacion = user.FechaCreacion
        };

        return GenerateToken(userRead);
    }

    public string Login(UsuarioLoginDto usuarioLoginDto)
    {
        var usuario = _repository.GetUsuarioByEmail(usuarioLoginDto.Correo);

        if (usuario == null)
        {
            throw new UnauthorizedAccessException("Correo o contraseña incorrectos");
        }

        var salt = usuario.SaltContrasena;
        var hash = HashPassword(usuarioLoginDto.Password, salt);

        if (hash != usuario.HashContrasena)
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

    public byte[] GenerateSalt()
    {
        return RandomNumberGenerator.GetBytes(64);
    }

    public string HashPassword(string contrasena, byte[] salt)
    {
        int keySize = 64;
        int iterations = 1000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        var hash = Rfc2898DeriveBytes.Pbkdf2(Encoding.UTF8.GetBytes(contrasena),
                                             salt,
                                             iterations,
                                             hashAlgorithm,
                                             keySize);

        return Convert.ToHexString(hash);
    }

    public string GenerateToken(UsuarioReadDto usuarioReadDto)
    {
        var secretKey = _configuration["JWT:SecretKey"];
        if (string.IsNullOrEmpty(secretKey))
        {
            throw new InvalidOperationException("La clave JWT no está configurada en appsettings.json");
        }

        var key = Encoding.UTF8.GetBytes(secretKey);
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
            return false;
        }
        var isAdmin = rolClaim!.Value == "Admin";

        if (isOwnResource || isAdmin)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
