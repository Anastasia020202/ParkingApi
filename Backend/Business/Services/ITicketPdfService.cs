using ParkingApi.Models;

namespace ParkingApi.Business.Services
{
    public interface ITicketPdfService
    {
        /// <summary>
        /// Genera un PDF del ticket de parking
        /// </summary>
        /// <param name="ticket">Ticket a generar</param>
        /// <returns>Array de bytes del PDF generado</returns>
        byte[] GenerateTicketPdf(Ticket ticket);
        
        /// <summary>
        /// Genera un PDF del ticket y lo guarda en disco
        /// </summary>
        /// <param name="ticket">Ticket a generar</param>
        /// <param name="filePath">Ruta donde guardar el PDF</param>
        /// <returns>True si se guardó correctamente</returns>
        bool SaveTicketPdf(Ticket ticket, string filePath);
    }
}


