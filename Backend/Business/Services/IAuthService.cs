using System.Security.Claims;
using ParkingApi.Models.DTOs;

namespace ParkingApi.Business.Services
{
    public interface IAuthService
    {
        public string Login(UsuarioLoginDto usuarioLoginDto);
        public string Register(UsuarioRegisterDto usuarioRegisterDto);
        public string GenerateToken(UsuarioReadDto usuarioReadDto);
        public bool VerificarAcceso(int id, ClaimsPrincipal user);
    }
}
