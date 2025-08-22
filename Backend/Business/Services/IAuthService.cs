using ParkingApi.Models;
using ParkingApi.Models.DTOs;

namespace ParkingApi.Business.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(UsuarioLoginDto loginDto);
        Task<UsuarioReadDto> RegisterAsync(UsuarioRegisterDto registerDto);
    }
}
