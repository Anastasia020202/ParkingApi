using ParkingApi.Models;
using ParkingApi.Repositories;

namespace ParkingApi.Repositories
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
                Email = "admin@parking.com",
                Password = "admin123",
                Rol = "Admin",
                FechaCreacion = DateTime.UtcNow,
                Activo = true
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
            return _usuarios.FirstOrDefault(u => u.Email == email);
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

            existingUsuario.Email = usuario.Email;
            existingUsuario.Password = usuario.Password;
            existingUsuario.Rol = usuario.Rol;
            existingUsuario.Activo = usuario.Activo;

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
