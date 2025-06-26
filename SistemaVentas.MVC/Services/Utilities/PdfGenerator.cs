using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SistemaVentas.Model;
using System.Drawing;
using System.IO;
using System.Xml.Linq;

namespace SistemaVentas.MVC.Services.Utilities
{
    public interface IPdfGenerator
    {
        byte[] GenerateTicketPdf(Ticket ticket);
    }

    public class PdfGenerator : IPdfGenerator
    {
        public byte[] GenerateTicketPdf(Ticket ticket)
        {
            using var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var fontTitle = new XFont("Verdana", 20, XFontStyle.Bold);
            gfx.DrawString($"Ticket #{ticket.TicketId}", fontTitle, XBrushes.Black,
                new XRect(0, 20, page.Width, 40), XStringFormats.Center);

            var fontBody = new XFont("Verdana", 12, XFontStyle.Regular);
            gfx.DrawString($"Ruta: {ticket.Route?.NameRoute}", fontBody, XBrushes.Black, 40, 100);
            gfx.DrawString($"Categoría: {ticket.Category?.Name}", fontBody, XBrushes.Black, 40, 120);
            gfx.DrawString($"Asiento: {ticket.Seat?.Type}", fontBody, XBrushes.Black, 40, 140);
            gfx.DrawString($"Precio: ${ticket.Price:F2}", fontBody, XBrushes.Black, 40, 160);
            gfx.DrawString($"Cliente: {ticket.Customer?.Email}", fontBody, XBrushes.Black, 40, 180);

            using var ms = new MemoryStream();
            document.Save(ms);
            return ms.ToArray();
        }
    }
}