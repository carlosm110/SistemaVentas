using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using SistemaVentas.Model;
using SistemaVentas.MVC.Services.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SistemaVentas.MVC.Services.Observers
{
    public class CustomerNotifier : IObserver
    {
        private readonly IPdfGenerator _pdfGenerator;
        private readonly IHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string SESSION_KEY = "TicketNotifications";

        public CustomerNotifier(
            IPdfGenerator pdfGenerator,
            IHostEnvironment env,
            IHttpContextAccessor httpContextAccessor)
        {
            _pdfGenerator = pdfGenerator;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }

        public void Update(Ticket ticket)
        {
            // 1. Generar el PDF
            var pdfBytes = _pdfGenerator.GenerateTicketPdf(ticket);
            var fileName = $"ticket_{ticket.TicketId}.pdf";
            var folder = Path.Combine(_env.ContentRootPath, "wwwroot", "tickets");
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
            var fullPath = Path.Combine(folder, fileName);
            File.WriteAllBytes(fullPath, pdfBytes);

            // 2. Guardar el ID en sesión para notificar en la vista
            var session = _httpContextAccessor.HttpContext!.Session;
            var existing = session.GetString(SESSION_KEY);
            var list = existing is null
                ? new List<int>()
                : JsonSerializer.Deserialize<List<int>>(existing)!;

            list.Add(ticket.TicketId);
            session.SetString(SESSION_KEY, JsonSerializer.Serialize(list));
        }
    }
}