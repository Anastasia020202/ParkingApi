using ParkingApi.Data.Repositories;
using ParkingApi.Models;
using System.Security.Claims;

namespace ParkingApi.Business.Services;

public class TicketService : ITicketService
{
    private readonly IReservaRepository _reservaRepository;

    public TicketService(IReservaRepository reservaRepository)
    {
        _reservaRepository = reservaRepository;
    }

    public byte[] GenerateTicketPdf(int reservaId)
    {
        var reserva = _reservaRepository.GetReservaById(reservaId);
        if (reserva == null)
        {
            throw new KeyNotFoundException("Reserva no encontrada");
        }

        // Por ahora retornamos un array de bytes vacío
        // Aquí deberías integrar con ITicketPdfService para generar el PDF real
        // return await _ticketPdfService.GeneratePdf(reserva);
        
        // Placeholder: retornar un PDF simple
        var pdfContent = $"Ticket para Reserva {reserva.NumeroTicket}\n" +
                        $"Usuario: {reserva.UsuarioId}\n" +
                        $"Plaza: {reserva.PlazaId}\n" +
                        $"Importe: {reserva.Importe:C}\n" +
                        $"Estado: {reserva.Estado}\n" +
                        $"Fecha: {reserva.FechaEmision:dd/MM/yyyy HH:mm}";
        
        return System.Text.Encoding.UTF8.GetBytes(pdfContent);
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

    public bool TieneAcceso(int reservaId, ClaimsPrincipal user)
    {
        var claimId = user.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier);

        if (claimId == null || !int.TryParse(claimId.Value, out int resultado))
        {
            throw new UnauthorizedAccessException();
        }

        var reserva = _reservaRepository.GetReservaById(reservaId);
        if (reserva == null)
        {
            return false;
        }

        var esElUsuario = resultado == reserva.UsuarioId;

        return EsAdmin(user) || esElUsuario;
    }
}
