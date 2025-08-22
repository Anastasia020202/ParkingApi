using ParkingApi.DTOs;

namespace ParkingApi.Services
{
    public interface IAuthService
    {
        Task<string> LoginAsync(UsuarioLoginDto loginDto);
        Task<UsuarioReadDto> RegisterAsync(UsuarioRegisterDto registerDto);
    }
}
