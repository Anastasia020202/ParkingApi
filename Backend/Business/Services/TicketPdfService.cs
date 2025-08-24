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
        public byte[] GenerateTicketPdf(Reserva reserva)
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
                AddInfoRow(document, "Número de Ticket:", reserva.NumeroTicket, font, boldFont);
                AddInfoRow(document, "Fecha de Emisión:", reserva.FechaEmision.ToString("dd/MM/yyyy HH:mm"), font, boldFont);
                AddInfoRow(document, "Estado:", reserva.Estado, font, boldFont);

                // Separador
                document.Add(new Paragraph("").SetMarginBottom(20));

                // Información de la reserva
                AddInfoRow(document, "Plaza:", reserva.Plaza?.Numero ?? "N/A", font, boldFont);
                AddInfoRow(document, "Tipo de Plaza:", reserva.Plaza?.Tipo ?? "N/A", font, boldFont);
                AddInfoRow(document, "Fecha de Inicio:", reserva.FechaInicio.ToString("dd/MM/yyyy HH:mm"), font, boldFont);
                
                if (reserva.FechaFin.HasValue)
                {
                    AddInfoRow(document, "Fecha de Fin:", reserva.FechaFin.Value.ToString("dd/MM/yyyy HH:mm"), font, boldFont);
                }

                AddInfoRow(document, "Usuario:", reserva.Usuario?.Correo ?? "N/A", font, boldFont);
                
                if (reserva.Vehiculo != null)
                {
                    AddInfoRow(document, "Vehículo:", reserva.Vehiculo.Matricula, font, boldFont);
                    AddInfoRow(document, "Marca:", reserva.Vehiculo.Marca, font, boldFont);
                    AddInfoRow(document, "Modelo:", reserva.Vehiculo.Modelo, font, boldFont);
                }

                // Separador
                document.Add(new Paragraph("").SetMarginBottom(20));

                // Información de precios
                AddInfoRow(document, "Precio por Hora:", $"€{reserva.Plaza?.PrecioHora:F2}", font, boldFont);
                AddInfoRow(document, "Total a Pagar:", $"€{reserva.Importe:F2}", font, boldFont);

                // Observaciones
                if (!string.IsNullOrEmpty(reserva.Observaciones))
                {
                    document.Add(new Paragraph("").SetMarginBottom(20));
                    AddInfoRow(document, "Observaciones:", reserva.Observaciones, font, boldFont);
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

        public bool SaveTicketPdf(Reserva reserva, string filePath)
        {
            try
            {
                byte[] pdfBytes = GenerateTicketPdf(reserva);
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
