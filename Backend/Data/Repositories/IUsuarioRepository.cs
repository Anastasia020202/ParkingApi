using ParkingApi.Models;

namespace ParkingApi.Data.Repositories;

public interface IUsuarioRepository
{
    public Usuario AddUsuarioFromCredentials(string correo, string hash, byte[] salt);
    public Usuario GetUsuarioByEmail(string correo);
    public Usuario GetUsuarioById(int id);
    public IEnumerable<Usuario> GetUsuarios(UsuarioQueryParameters query);
    public void SaveChanges();
}
