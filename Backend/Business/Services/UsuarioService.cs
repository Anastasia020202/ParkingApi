using ParkingApi.Models;

using ParkingApi.Data.Repositories;

namespace ParkingApi.Business.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Usuario> GetAllUsuarios()
        {
            return _repository.GetAll();
        }

        public Usuario? GetUsuarioById(int id)
        {
            return _repository.GetById(id);
        }

        public Usuario? GetUsuarioByEmail(string email)
        {
            return _repository.GetByEmail(email);
        }

        public Usuario CreateUsuario(Usuario usuario)
        {
            return _repository.Add(usuario);
        }

        public Usuario? UpdateUsuario(int id, Usuario usuario)
        {
            return _repository.Update(id, usuario);
        }

        public bool DeleteUsuario(int id)
        {
            return _repository.Delete(id);
        }
    }
}
