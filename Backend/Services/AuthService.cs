// TEMPORALMENTE COMENTADO PARA QUE COMPILE EL CONTROLADOR DE PLAZA
/*
using System.Security.Cryptography;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ParkingApi.Models;
using ParkingApi.Data;
using ParkingApi.DTOs;

namespace ParkingApi.Business;

public class AuthService : IAuthService
*/
{
    private readonly IConfiguration _configuration;
    private readonly IUsuarioRepository _repository;

    public AuthService(IConfiguration configuration, IUsuarioRepository repository)
    {
        _configuration = configuration;
        _repository = repository;

        Console.WriteLine($"[AuthService] JWT:SecretKey -> {_configuration["JWT:SecretKey"]}");
        Console.WriteLine($"[AuthService] JWT:ValidIssuer -> {_configuration["JWT:ValidIssuer"]}");
        Console.WriteLine($"[AuthService] JWT:ValidAudience -> {_configuration["JWT:ValidAudience"]}");
        Console.WriteLine($"[AuthService] Repository -> {(_repository != null ? "OK" : "NULL")}");
    }

    public string Register(UsuarioCreateDto usuarioCreateDto)
    {
        try
        {
            Console.WriteLine($"[AuthService.Register] Iniciando registro para: {usuarioCreateDto?.Correo ?? "NULL"}");
            
            if (usuarioCreateDto == null)
            {
                Console.WriteLine("[AuthService.Register] ERROR: usuarioCreateDto es NULL");
                throw new ArgumentNullException(nameof(usuarioCreateDto));
            }
            
            if (string.IsNullOrEmpty(usuarioCreateDto.Correo))
            {
                Console.WriteLine("[AuthService.Register] ERROR: Correo es null o vacío");
                throw new ArgumentException("El correo no puede ser null o vacío", nameof(usuarioCreateDto.Correo));
            }
            
            if (string.IsNullOrEmpty(usuarioCreateDto.Contrasena))
            {
                Console.WriteLine("[AuthService.Register] ERROR: Contraseña es null o vacía");
                throw new ArgumentException("La contraseña no puede ser null o vacía", nameof(usuarioCreateDto.Contrasena));
            }
            
            var correo = usuarioCreateDto.Correo;
            Console.WriteLine($"[AuthService.Register] Correo: {correo}");

            // Verificar si ya existe
            Console.WriteLine("[AuthService.Register] Verificando si el usuario ya existe...");
            var existente = _repository.GetUsuarioByEmail(correo);
            Console.WriteLine($"[AuthService.Register] Usuario existente: {(existente != null ? "SÍ" : "NO")}");
            
            if (existente != null)
            {
                throw new InvalidOperationException($"Ya existe un usuario con el correo {correo}");
            }

            Console.WriteLine("[AuthService.Register] Generando salt y hash...");
            var salt = GenerateSalt();
            var hash = HashPassword(usuarioCreateDto.Contrasena, salt);
            Console.WriteLine($"[AuthService.Register] Salt generado: {Convert.ToHexString(salt)}");
            Console.WriteLine($"[AuthService.Register] Hash generado: {hash}");

            Console.WriteLine("[AuthService.Register] Creando nuevo usuario...");
            var nuevoUsuario = new Usuario
            {
                Correo = usuarioCreateDto.Correo,
                HashContrasena = hash,
                SaltContrasena = salt,
                Rol = "User",
                FechaCreacion = DateTime.UtcNow,
                HistoricoReservas = new List<Reserva>()
            };
            Console.WriteLine($"[AuthService.Register] Usuario creado con ID: {nuevoUsuario.Id}");

            Console.WriteLine("[AuthService.Register] Agregando usuario al repositorio...");
            _repository.AddUsuario(nuevoUsuario);
            Console.WriteLine("[AuthService.Register] Guardando cambios...");
            _repository.SaveChanges();
            Console.WriteLine("[AuthService.Register] Cambios guardados exitosamente");

            Console.WriteLine("[AuthService.Register] Creando DTO de respuesta...");
            var userRead = new UsuarioReadDto()
            {
                Id = nuevoUsuario.Id,
                Correo = nuevoUsuario.Correo,
                Rol = nuevoUsuario.Rol,
                FechaCreacion = nuevoUsuario.FechaCreacion
            };
            Console.WriteLine($"[AuthService.Register] DTO creado: ID={userRead.Id}, Correo={userRead.Correo}");

            Console.WriteLine("[AuthService.Register] Generando token...");
            var token = GenerateToken(userRead);
            Console.WriteLine($"[AuthService.Register] Token generado: {token.Substring(0, Math.Min(50, token.Length))}...");
            return token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService.Register] ERROR: {ex.GetType().Name}: {ex.Message}");
            Console.WriteLine($"[AuthService.Register] StackTrace: {ex.StackTrace}");
            throw; // Re-lanzar la excepción para que el controlador la maneje
        }
    }

    public string Login(UsuarioCreateDto usuarioCreateDto)
    {
        try
        {
            Console.WriteLine($"[AuthService.Login] Iniciando login para: {usuarioCreateDto?.Correo ?? "NULL"}");
            
            if (usuarioCreateDto == null)
            {
                Console.WriteLine("[AuthService.Login] ERROR: usuarioCreateDto es NULL");
                throw new ArgumentNullException(nameof(usuarioCreateDto));
            }
            
            if (string.IsNullOrEmpty(usuarioCreateDto.Correo))
            {
                Console.WriteLine("[AuthService.Login] ERROR: Correo es null o vacío");
                throw new ArgumentException("El correo no puede ser null o vacío", nameof(usuarioCreateDto.Correo));
            }
            
            if (string.IsNullOrEmpty(usuarioCreateDto.Contrasena))
            {
                Console.WriteLine("[AuthService.Login] ERROR: Contraseña es null o vacía");
                throw new ArgumentException("La contraseña no puede ser null o vacía", nameof(usuarioCreateDto.Contrasena));
            }
            
            // TEMPORAL: Crear usuario admin si no existe
            if (usuarioCreateDto.Correo == "admin@parking.com" && usuarioCreateDto.Contrasena == "Admin123!")
            {
                Console.WriteLine("[AuthService.Login] Intentando crear usuario admin...");
                // Verificar si ya existe el usuario admin
                var adminExistente = _repository.GetUsuarioByEmail("admin@parking.com");
                if (adminExistente == null)
                {
                    Console.WriteLine("[AuthService.Login] Usuario admin no existe, creándolo...");
                    // Crear usuario admin
                    var adminSalt = GenerateSalt();
                    var adminHash = HashPassword("Admin123!", adminSalt);
                    
                    var adminUser = new Usuario
                    {
                        Correo = "admin@parking.com",
                        HashContrasena = adminHash,
                        SaltContrasena = adminSalt,
                        Rol = "Admin",
                        FechaCreacion = DateTime.UtcNow,
                        HistoricoReservas = new List<Reserva>()
                    };
                    
                    Console.WriteLine("[AuthService.Login] Agregando usuario admin al repositorio...");
                    _repository.AddUsuario(adminUser);
                    Console.WriteLine("[AuthService.Login] Guardando cambios del usuario admin...");
                    _repository.SaveChanges();
                    Console.WriteLine("[AuthService.Login] Usuario admin creado exitosamente");
                    
                    // Ahora buscar el usuario recién creado
                    adminExistente = _repository.GetUsuarioByEmail("admin@parking.com");
                }
                else
                {
                    Console.WriteLine("[AuthService.Login] Usuario admin ya existe");
                }
                
                // Si es admin, verificar la contraseña
                if (adminExistente != null)
                {
                    var adminSalt = adminExistente.SaltContrasena;
                    var adminHash = HashPassword(usuarioCreateDto.Contrasena, adminSalt);
                    
                    // Comparación segura de hashes
                    if (CryptographicOperations.FixedTimeEquals(
                            Convert.FromHexString(adminHash),
                            Convert.FromHexString(adminExistente.HashContrasena)))
                    {
                        Console.WriteLine($"[AuthService.Login] Usuario admin autenticado: ID={adminExistente.Id}, Rol={adminExistente.Rol}");
                        
                        var adminUserRead = new UsuarioReadDto()
                        {
                            Id = adminExistente.Id,
                            Correo = adminExistente.Correo,
                            Rol = adminExistente.Rol,
                            FechaCreacion = adminExistente.FechaCreacion
                        };

                        Console.WriteLine("[AuthService.Login] Generando token para admin...");
                        var adminToken = GenerateToken(adminUserRead);
                        Console.WriteLine($"[AuthService.Login] Login exitoso para admin, token generado: {adminToken.Substring(0, Math.Min(50, adminToken.Length))}...");
                        return adminToken;
                    }
                }
            }

            // Para usuarios normales o si el admin falló
            Console.WriteLine("[AuthService.Login] Buscando usuario en la base de datos...");
            var usuario = _repository.GetUsuarioByEmail(usuarioCreateDto.Correo);

            if (usuario == null)
            {
                Console.WriteLine($"[AuthService.Login] Usuario no encontrado: {usuarioCreateDto.Correo}");
                throw new UnauthorizedAccessException("Correo o contraseña incorrectos");
            }

            Console.WriteLine($"[AuthService.Login] Usuario encontrado: ID={usuario.Id}, Rol={usuario.Rol}");
            var salt = usuario.SaltContrasena;
            var hash = HashPassword(usuarioCreateDto.Contrasena, salt);

            // Comparación segura de hashes
            if (!CryptographicOperations.FixedTimeEquals(
                    Convert.FromHexString(hash),
                    Convert.FromHexString(usuario.HashContrasena)))
            {
                Console.WriteLine("[AuthService.Login] Hash de contraseña no coincide");
                throw new UnauthorizedAccessException("Correo o contraseña incorrectos");
            }

            Console.WriteLine($"[AuthService.Login] Usuario autenticado: ID={usuario.Id}, Rol={usuario.Rol}");
            
            var userRead = new UsuarioReadDto()
            {
                Id = usuario.Id,
                Correo = usuario.Correo,
                Rol = usuario.Rol,
                FechaCreacion = usuario.FechaCreacion
            };

            Console.WriteLine("[AuthService.Login] Generando token...");
            var token = GenerateToken(userRead);
            Console.WriteLine($"[AuthService.Login] Login exitoso, token generado: {token.Substring(0, Math.Min(50, token.Length))}...");
            return token;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[AuthService.Login] ERROR: {ex.GetType().Name}: {ex.Message}");
            Console.WriteLine($"[AuthService.Login] StackTrace: {ex.StackTrace}");
            throw; // Re-lanzar la excepción para que el controlador la maneje
        }
    }

    public byte[] GenerateSalt()
    {
        Console.WriteLine("[AuthService.GenerateSalt] Generando salt de 64 bytes...");
        var salt = RandomNumberGenerator.GetBytes(64);
        Console.WriteLine($"[AuthService.GenerateSalt] Salt generado exitosamente, longitud: {salt.Length}");
        return salt;
    }

    public string HashPassword(string contrasena, byte[] salt)
    {
        Console.WriteLine($"[AuthService.HashPassword] Generando hash para contraseña de longitud: {contrasena?.Length ?? 0}");
        
        if (string.IsNullOrEmpty(contrasena))
        {
            Console.WriteLine("[AuthService.HashPassword] ERROR: contraseña es null o vacía");
            throw new ArgumentException("La contraseña no puede ser null o vacía", nameof(contrasena));
        }
        
        if (salt == null || salt.Length == 0)
        {
            Console.WriteLine("[AuthService.HashPassword] ERROR: salt es null o vacío");
            throw new ArgumentException("El salt no puede ser null o vacío", nameof(salt));
        }
        
        int keySize = 64;
        int iterations = 1000;
        HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

        Console.WriteLine($"[AuthService.HashPassword] Parámetros: keySize={keySize}, iterations={iterations}, algorithm={hashAlgorithm}");
        
        var hash = Rfc2898DeriveBytes.Pbkdf2(
            Encoding.UTF8.GetBytes(contrasena),
            salt,
            iterations,
            hashAlgorithm,
            keySize);

        var hashString = Convert.ToHexString(hash); // Devuelve en mayúsculas
        Console.WriteLine($"[AuthService.HashPassword] Hash generado exitosamente, longitud: {hashString.Length}");
        
        return hashString;
    }

    public string GenerateToken(UsuarioReadDto usuarioReadDto)
    {
        Console.WriteLine($"[AuthService.GenerateToken] Iniciando generación de token para: {usuarioReadDto.Correo}");
        
        if (usuarioReadDto == null)
        {
            Console.WriteLine("[AuthService.GenerateToken] ERROR: usuarioReadDto es NULL");
            throw new ArgumentNullException(nameof(usuarioReadDto));
        }
        
        // Verificar que la configuración esté disponible
        if (_configuration == null)
        {
            Console.WriteLine("[AuthService.GenerateToken] ERROR: _configuration es NULL");
            throw new InvalidOperationException("La configuración no está disponible");
        }
        
        var secretKey = _configuration["JWT:SecretKey"];
        var validIssuer = _configuration["JWT:ValidIssuer"];
        var validAudience = _configuration["JWT:ValidAudience"];
        
        if (string.IsNullOrEmpty(secretKey))
        {
            Console.WriteLine("[AuthService.GenerateToken] ERROR: JWT:SecretKey está vacío o es null");
            throw new InvalidOperationException("JWT:SecretKey no está configurado");
        }
        
        if (string.IsNullOrEmpty(validIssuer))
        {
            Console.WriteLine("[AuthService.GenerateToken] ERROR: JWT:ValidIssuer está vacío o es null");
            throw new InvalidOperationException("JWT:ValidIssuer no está configurado");
        }
        
        if (string.IsNullOrEmpty(validAudience))
        {
            Console.WriteLine("[AuthService.GenerateToken] ERROR: JWT:ValidAudience está vacío o es null");
            throw new InvalidOperationException("JWT:ValidAudience no está configurado");
        }
        
        Console.WriteLine($"[AuthService.GenerateToken] SecretKey: {secretKey}");
        Console.WriteLine($"[AuthService.GenerateToken] ValidIssuer: {validIssuer}");
        Console.WriteLine($"[AuthService.GenerateToken] ValidAudience: {validAudience}");
        
        var key = Encoding.UTF8.GetBytes(secretKey);
        Console.WriteLine($"[AuthService.GenerateToken] Key generado, longitud: {key.Length}");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Issuer = validIssuer,
            Audience = validAudience,
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioReadDto.Id.ToString()),
                new Claim(ClaimTypes.Email, usuarioReadDto.Correo),
                new Claim(ClaimTypes.Role, usuarioReadDto.Rol),
            }),
            Expires = DateTime.UtcNow.AddHours(24), // expira en 24h
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        Console.WriteLine("[AuthService.GenerateToken] Creando token handler...");
        var tokenHandler = new JwtSecurityTokenHandler();
        Console.WriteLine("[AuthService.GenerateToken] Creando token...");
        var token = tokenHandler.CreateToken(tokenDescriptor);
        Console.WriteLine("[AuthService.GenerateToken] Escribiendo token...");
        var tokenString = tokenHandler.WriteToken(token);
        Console.WriteLine($"[AuthService.GenerateToken] Token generado exitosamente, longitud: {tokenString.Length}");
        return tokenString;
    }

    public bool VerificarAcceso(int id, ClaimsPrincipal user)
    {
        if (user == null)
        {
            Console.WriteLine("[AuthService.VerificarAcceso] ERROR: user es NULL");
            return false;
        }
        
        Console.WriteLine($"[AuthService.VerificarAcceso] Verificando acceso para ID: {id}");
        var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        if (userIdClaim is null || !int.TryParse(userIdClaim.Value, out int userId))
        {
            Console.WriteLine("[AuthService.VerificarAcceso] No se pudo obtener el ID del usuario del token");
            return false;
        }

        var isOwnResource = (userId == id);

        var rolClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
        var isAdmin = rolClaim != null && rolClaim.Value == "Admin";
        
        Console.WriteLine($"[AuthService.VerificarAcceso] Usuario ID: {userId}, Rol: {rolClaim?.Value}, Es propio: {isOwnResource}, Es admin: {isAdmin}");
        
        var resultado = isOwnResource || isAdmin;
        Console.WriteLine($"[AuthService.VerificarAcceso] Acceso permitido: {resultado}");
        
        return resultado;
    }
}



