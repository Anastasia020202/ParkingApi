using ParkingApi.Models;

namespace ParkingApi.Business.Services
{
    public interface IUsuarioService
    {
        IEnumerable<Usuario> GetAllUsuarios();
        Usuario? GetUsuarioById(int id);
        Usuario? GetUsuarioByEmail(string email);
        Usuario CreateUsuario(Usuario usuario);
        Usuario? UpdateUsuario(int id, Usuario usuario);
        bool DeleteUsuario(int id);
    }
}
