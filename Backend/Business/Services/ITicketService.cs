using ParkingApi.Models;
using System.Security.Claims;

namespace ParkingApi.Business.Services;

public interface ITicketService
{
    public byte[] GenerateTicketPdf(int reservaId);
    public bool EsAdmin(ClaimsPrincipal user);
    public bool TieneAcceso(int reservaId, ClaimsPrincipal user);
}
