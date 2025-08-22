using ParkingApi.Models;

namespace ParkingApi.Repositories
{
    public interface IUsuarioRepository
    {
        IEnumerable<Usuario> GetAll();
        Usuario? GetById(int id);
        Usuario? GetByEmail(string email);
        Usuario Add(Usuario usuario);
        Usuario? Update(int id, Usuario usuario);
        bool Delete(int id);
    }
}
