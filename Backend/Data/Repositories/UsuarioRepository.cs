using ParkingApi.Models;
using ParkingApi.Data.Repositories;

namespace ParkingApi.Data.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private static List<Usuario> _usuarios = new List<Usuario>();

        // Constructor estático para crear Admin por defecto
        static UsuarioRepository()
        {
            // Crear Admin por defecto
            _usuarios.Add(new Usuario
            {
                Id = 1,
                Correo = "admin@parking.com",
                HashContrasena = "admin123",
                SaltContrasena = new byte[0],
                Rol = "Admin",
                FechaCreacion = DateTime.UtcNow
            });
        }

        public IEnumerable<Usuario> GetAll()
        {
            return _usuarios;
        }

        public Usuario? GetById(int id)
        {
            return _usuarios.FirstOrDefault(u => u.Id == id);
        }

        public Usuario? GetByEmail(string email)
        {
            return _usuarios.FirstOrDefault(u => u.Correo == email);
        }

        public Usuario Add(Usuario usuario)
        {
            usuario.Id = _usuarios.Count > 0 ? _usuarios.Max(u => u.Id) + 1 : 1;
            _usuarios.Add(usuario);
            return usuario;
        }

        public Usuario? Update(int id, Usuario usuario)
        {
            var existingUsuario = _usuarios.FirstOrDefault(u => u.Id == id);
            if (existingUsuario == null)
                return null;

            existingUsuario.Correo = usuario.Correo;
            existingUsuario.HashContrasena = usuario.HashContrasena;
            existingUsuario.SaltContrasena = usuario.SaltContrasena;
            existingUsuario.Rol = usuario.Rol;

            return existingUsuario;
        }

        public bool Delete(int id)
        {
            var usuario = _usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
                return false;

            return _usuarios.Remove(usuario);
        }
    }
}
