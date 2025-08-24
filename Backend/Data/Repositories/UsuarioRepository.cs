using ParkingApi.Models;

namespace ParkingApi.Data.Repositories;

public class UsuarioRepository : IUsuarioRepository
{

    private readonly ParkingDbContext _context;

    public UsuarioRepository(ParkingDbContext context)
    {
        _context = context;
    }

    public Usuario AddUsuarioFromCredentials(string correo, string hash, byte[] salt)
    {
        // Primero chequear que el correo no se esté usando ya (devolver excepción)
        if (_context.Usuarios.Any(u => u.Correo == correo))
        {
            throw new InvalidOperationException("Ya existe un usuario con ese correo.");
        }
        // Luego coger de la base de datos
        var user = new Usuario
        {
            Correo = correo,
            HashContrasena = hash,
            SaltContrasena = salt,
            Rol = "User",
            FechaCreacion = DateTime.Now
        };

        _context.Add(user);
        _context.SaveChanges();

        return user;
    }

    public Usuario GetUsuarioByEmail(string correo)
    {
        var user = _context.Usuarios.FirstOrDefault(u => u.Correo == correo);
        if (user is null)
        {
            throw new KeyNotFoundException("Usuario no encontrado");
        }
        return user;
    }

    public Usuario GetUsuarioById(int id)
    {
        var user = _context.Usuarios.FirstOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new KeyNotFoundException("Usuario no encontrado");
        }
        return user;
    }

    public IEnumerable<Usuario> GetUsuarios(UsuarioQueryParameters queryParams)
    {
        var query = _context.Usuarios.AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParams.Rol))
        {
            query = query.Where(u => u.Rol.ToLower().Contains(queryParams.Rol.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(queryParams.Correo))
        {
            query = query.Where(u => u.Correo.ToLower().Contains(queryParams.Correo.ToLower()));
        }

        if (queryParams.FechaInicio != null)
        {
            query = query.Where(u => u.FechaCreacion >= queryParams.FechaInicio);
            query = query.OrderBy(u => u.FechaCreacion);
        }

        if (queryParams.FechaFinal != null)
        {
            query = query.Where(u => u.FechaCreacion <= queryParams.FechaFinal);
            query = query.OrderBy(u => u.FechaCreacion);
        }

        var result = query.ToList();
        return result;
    }

    public void SaveChanges()
    {
        _context.SaveChanges();
    }
}
