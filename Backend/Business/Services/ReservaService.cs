using ParkingApi.Data.Repositories;
using ParkingApi.Models;
using ParkingApi.Models.DTOs;
using System.Security.Claims;

namespace ParkingApi.Business.Services;

public class ReservaService : IReservaService
{
    private readonly IReservaRepository _pedidoRepository;
    private readonly IPlazaRepository _plazaRepository;

    public ReservaService(IReservaRepository pedidoRepository,
                         IPlazaRepository plazaRepository)
    {
        _pedidoRepository = pedidoRepository;
        _plazaRepository = plazaRepository;
    }

               public Reserva CreateReserva(int userId, ReservaCreateDto reservaCreateDto)
           {
               // Verificar que la plaza esté disponible
               var plaza = _plazaRepository.GetPlaza(reservaCreateDto.PlazaId);
               if (plaza == null)
               {
                   throw new ArgumentException("La plaza especificada no existe.");
               }

               if (!plaza.Disponible)
               {
                   throw new Exception("La plaza no está disponible.");
               }

               var nuevaReserva = new Reserva
               {
                   UsuarioId = userId,
                   PlazaId = reservaCreateDto.PlazaId,
                   VehiculoId = reservaCreateDto.VehiculoId,
                   FechaInicio = reservaCreateDto.FechaInicio,
                   FechaFin = reservaCreateDto.FechaFin,
                   Estado = reservaCreateDto.Estado ?? "Pendiente",
                   Importe = reservaCreateDto.Importe,
                   Observaciones = reservaCreateDto.Observaciones,
                   FechaEmision = DateTime.UtcNow,
                   NumeroTicket = $"T-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}",
                   TotalAPagar = reservaCreateDto.Importe
               };

               _pedidoRepository.AddReserva(nuevaReserva);
               _pedidoRepository.SaveChanges();
               return nuevaReserva;
           }

    public IEnumerable<Reserva> GetAllReservas()
    {
        var reservas = _pedidoRepository.GetAllReservas();

        var reservasDto = new List<Reserva>();

        foreach (var reserva in reservas)
        {
            var reservaDto = new Reserva
            {
                Id = reserva.Id,
                UsuarioId = reserva.UsuarioId,
                PlazaId = reserva.PlazaId,
                VehiculoId = reserva.VehiculoId,
                FechaInicio = reserva.FechaInicio,
                FechaFin = reserva.FechaFin,
                Estado = reserva.Estado
            };
            reservasDto.Add(reservaDto);
        }

        return reservasDto;
    }

    public Reserva GetReservaById(int id)
    {
        var reserva = _pedidoRepository.GetReservaById(id);
        if (reserva == null)
        {
            throw new KeyNotFoundException($"No hay reservas con el id {id}");
        }

        var reservaDto = new Reserva
        {
            Id = reserva.Id,
            UsuarioId = reserva.UsuarioId,
            PlazaId = reserva.PlazaId,
            VehiculoId = reserva.VehiculoId,
            FechaInicio = reserva.FechaInicio,
            FechaFin = reserva.FechaFin,
            Estado = reserva.Estado
        };

        return reservaDto;
    }

    public IEnumerable<Reserva> GetReservasByUser(int userId)
    {
        var reservas = _pedidoRepository.GetReservasByUserId(userId);
        if (reservas == null)
        {
            throw new KeyNotFoundException($"No hay reservas con el id de usuario {userId}");
        }

        var reservasDto = new List<Reserva>();

        foreach (var reserva in reservas)
        {
            var reservaDto = new Reserva
            {
                Id = reserva.Id,
                UsuarioId = reserva.UsuarioId,
                PlazaId = reserva.PlazaId,
                VehiculoId = reserva.VehiculoId,
                FechaInicio = reserva.FechaInicio,
                FechaFin = reserva.FechaFin,
                Estado = reserva.Estado
            };
            reservasDto.Add(reservaDto);
        }

                       return reservasDto;
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

           public bool TieneAcceso(int id, ClaimsPrincipal user)
           {
               var claimId = user.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

               if (claimId == null || !int.TryParse(claimId.Value, out int resultado))
               {
                   throw new UnauthorizedAccessException();
               }

               var esElUsuario = resultado == id;

               return EsAdmin(user) || esElUsuario;
           }

           public IEnumerable<Reserva> GetReservasByUsuarioId(int userId)
           {
               return GetReservasByUser(userId);
           }
       }
