using System.Security.Claims;
using ParkingApi.Data.Repositories;
using ParkingApi.Models;
using ParkingApi.Models.DTOs;

namespace ParkingApi.Business.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repository;

    public UsuarioService(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    public bool EsAdmin(ClaimsPrincipal user)
    {
        var rol = user.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role);

        if (rol == null)
        {
            return false;
        }

        var claimValue = rol.Value;

        return claimValue == "Admin";
    }

    public IEnumerable<UsuarioReadDto> GetAllUsuarios(UsuarioQueryParameters query)
    {
        // Select es el equivalente en C# a map
        var listaDto = _repository.GetUsuarios(query).Select(usuario => new UsuarioReadDto
        {
            Id = usuario.Id,
            Correo = usuario.Correo,
            Rol = usuario.Rol,
            FechaCreacion = usuario.FechaCreacion
        });
        return listaDto;
    }

    public UsuarioReadDto GetUsuarioById(int id)
    {
        var usuario = _repository.GetUsuarioById(id);
        if (usuario == null)
        {
            throw new KeyNotFoundException($"No hay usuarios con el id {id}");
        }

        var usuarioDto = new UsuarioReadDto
        {
            Id = usuario.Id,
            Correo = usuario.Correo,
            Rol = usuario.Rol,
            FechaCreacion = usuario.FechaCreacion
        };

        return usuarioDto;
    }

    // Verificar que la id del user coincide con la del recurso que nos pide
    public bool TieneAcceso(int userId, ClaimsPrincipal user)
    {
        var claimId = user.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

        if (claimId == null || !int.TryParse(claimId.Value, out int resultado))
        {
            throw new UnauthorizedAccessException();
        }

        var esElUsuario = userId == resultado;

        return EsAdmin(user) || esElUsuario;
    }

    public IEnumerable<Reserva> GetReservasByUser(int userId)
    {
        // Este método debería obtener las reservas del usuario
        // Por ahora retornamos una lista vacía, pero deberías implementar
        // la lógica para obtener las reservas del repositorio
        return new List<Reserva>();
    }

    public Reserva GetReservaByUserAndId(int userId, int reservaId)
    {
        // Este método debería obtener una reserva específica del usuario
        // Por ahora lanzamos una excepción, pero deberías implementar
        // la lógica para obtener la reserva del repositorio
        throw new NotImplementedException("Método no implementado aún");
    }
}
