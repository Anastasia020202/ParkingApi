using ParkingApi.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Colors;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;

namespace ParkingApi.Business.Services
{
    public class TicketPdfService : ITicketPdfService
    {
        public byte[] GenerateTicketPdf(Ticket ticket)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(ms);
                PdfDocument pdf = new PdfDocument(writer);
                Document document = new Document(pdf, PageSize.A4);

                // Configurar fuentes
                PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
                PdfFont boldFont = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);

                // Título principal
                Paragraph title = new Paragraph("TICKET DE PARKING")
                    .SetFont(boldFont)
                    .SetFontSize(24)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetMarginBottom(20);
                document.Add(title);

                // Información del ticket
                AddInfoRow(document, "Número de Ticket:", ticket.NumeroTicket, font, boldFont);
                AddInfoRow(document, "Fecha de Emisión:", ticket.FechaEmision.ToString("dd/MM/yyyy HH:mm"), font, boldFont);
                AddInfoRow(document, "Estado:", ticket.Estado, font, boldFont);

                // Separador
                document.Add(new Paragraph("").SetMarginBottom(20));

                // Información de la reserva
                if (ticket.Reserva != null)
                {
                    AddInfoRow(document, "Plaza:", ticket.Reserva.Plaza?.Numero ?? "N/A", font, boldFont);
                    AddInfoRow(document, "Tipo de Plaza:", ticket.Reserva.Plaza?.Tipo ?? "N/A", font, boldFont);
                    AddInfoRow(document, "Fecha de Inicio:", ticket.Reserva.FechaInicio.ToString("dd/MM/yyyy HH:mm"), font, boldFont);
                    
                    if (ticket.Reserva.FechaFin.HasValue)
                    {
                        AddInfoRow(document, "Fecha de Fin:", ticket.Reserva.FechaFin.Value.ToString("dd/MM/yyyy HH:mm"), font, boldFont);
                    }

                    AddInfoRow(document, "Usuario:", ticket.Reserva.Usuario?.Correo ?? "N/A", font, boldFont);
                    
                    if (ticket.Reserva.Vehiculo != null)
                    {
                        AddInfoRow(document, "Vehículo:", ticket.Reserva.Vehiculo.Matricula, font, boldFont);
                        AddInfoRow(document, "Marca:", ticket.Reserva.Vehiculo.Marca, font, boldFont);
                        AddInfoRow(document, "Modelo:", ticket.Reserva.Vehiculo.Modelo, font, boldFont);
                    }
                }

                // Separador
                document.Add(new Paragraph("").SetMarginBottom(20));

                // Información de precios
                AddInfoRow(document, "Precio por Hora:", $"€{ticket.Reserva?.Plaza?.PrecioHora:F2}", font, boldFont);
                AddInfoRow(document, "Total a Pagar:", $"€{ticket.Importe:F2}", font, boldFont);

                // Observaciones
                if (!string.IsNullOrEmpty(ticket.Observaciones))
                {
                    document.Add(new Paragraph("").SetMarginBottom(20));
                    AddInfoRow(document, "Observaciones:", ticket.Observaciones, font, boldFont);
                }

                // Pie de página
                document.Add(new Paragraph("").SetMarginBottom(20));
                Paragraph footer = new Paragraph("Gracias por usar nuestro parking")
                    .SetFont(font)
                    .SetFontSize(12)
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontColor(ColorConstants.GRAY);
                document.Add(footer);

                document.Close();
                return ms.ToArray();
            }
        }

        public bool SaveTicketPdf(Ticket ticket, string filePath)
        {
            try
            {
                byte[] pdfBytes = GenerateTicketPdf(ticket);
                File.WriteAllBytes(filePath, pdfBytes);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void AddInfoRow(Document document, string label, string value, PdfFont font, PdfFont boldFont)
        {
            Paragraph row = new Paragraph()
                .Add(new Text(label).SetFont(boldFont).SetFontSize(12))
                .Add(new Text(" ").SetFont(font))
                .Add(new Text(value).SetFont(font).SetFontSize(12))
                .SetMarginBottom(8);
            document.Add(row);
        }
    }
}
