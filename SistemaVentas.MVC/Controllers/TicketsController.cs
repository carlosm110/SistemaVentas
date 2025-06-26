using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Model;
using Microsoft.AspNetCore.Http;
using SistemaVentas.MVC.Services.Business;
using SistemaVentas.MVC.Services.Strategies;
using SistemaVentas.MVC.Services.Observers;
using SistemaVentas.MVC.Services.Utilities;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace SistemaVentas.MVC.Controllers
{
    public class TicketsController : Controller
    {
        private readonly SistemaVentasDBContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly TicketService _ticketService;
        private readonly ChildPriceStrategy _childStrategy;
        private readonly AdultPriceStrategy _adultStrategy;
        private readonly SeniorPriceStrategy _seniorStrategy;
        private readonly CustomerNotifier _notifier;

        public TicketsController(SistemaVentasDBContext context,
                                 IWebHostEnvironment env,
                                 TicketService ticketService,
                                 ChildPriceStrategy childStrategy,
                                 AdultPriceStrategy adultStrategy,
                                 SeniorPriceStrategy seniorStrategy,
                                 CustomerNotifier notifier)
        {
            _context = context;
            _env = env;
            _ticketService = ticketService;
            _childStrategy = childStrategy;
            _adultStrategy = adultStrategy;
            _seniorStrategy = seniorStrategy;
            _notifier = notifier;

            // Registrar el observador (en este caso CustomerNotifier)
            _ticketService.RegisterObserver(_notifier);
        }

        // GET: Tickets/DownloadPdf/5
        [HttpGet]
        public IActionResult DownloadPdf(int id)
        {
            var filePath = Path.Combine(_env.ContentRootPath, "wwwroot", "tickets", $"ticket_{id}.pdf");
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/pdf", $"ticket_{id}.pdf");
        }

        // GET: Tickets/Create
        public IActionResult Create()
        {
            // Obtener el CustomerId desde la sesión
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            // Si el usuario no está logueado, redirigir al login
            if (customerId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            // Obtener el correo del usuario logueado desde la sesión
            var userEmail = HttpContext.Session.GetString("CustomerEmail");

            // Crear el ticket y asignar el CustomerId
            var ticket = new Ticket
            {
                CustomerId = customerId.Value  // Asignar el CustomerId al ticket
            };

            // Rellenar los ViewData con los datos necesarios para los dropdowns
            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name");
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "NameRoute");
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "Type");

            // Pasar el correo del usuario logueado a la vista
            ViewData["UserEmail"] = userEmail;

            return View(ticket);
        }

        // POST: Tickets/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketId,Delivered,SeatId,CategoryId,RouteId")] Ticket ticket)
        {
            // Obtener el CustomerId del usuario logueado desde la sesión
            var customerId = HttpContext.Session.GetInt32("CustomerId");

            // Verificar que el CustomerId no sea null
            if (customerId == null)
            {
                return RedirectToAction("Login", "Auth");  // Redirigir al login si no está logueado
            }

            ticket.CustomerId = customerId.Value;  // Asignar el CustomerId al ticket

            ticket.Route = await _context.Routes.FindAsync(ticket.RouteId);
            ticket.Category = await _context.Categories.FindAsync(ticket.CategoryId);
            ticket.Seat = await _context.Seats.FindAsync(ticket.SeatId);

            if (ticket.Route == null || ticket.Category == null || ticket.Seat == null)
            {
                ModelState.AddModelError("", "Invalid Route, Category, or Seat selection.");
                return View(ticket);
            }

            // Seleccionar la estrategia de precio, en este caso puede ser dinámica basada en la categoría o el tipo de asiento
            IPriceStrategy strategy;
            if (ticket.Category.Name == "Niño") 
            {
                strategy = _childStrategy;
            }
            else if (ticket.Category.Name == "Tercera Edad")
            {
                strategy = _seniorStrategy;
            }
            else 
            {
                strategy = _adultStrategy;
            }

            // Calcular el precio y asignarlo al ticket
            var price = _ticketService.CalculatePrice(ticket.Route.NameRoute, ticket.Category.Name, ticket.Seat.Type, strategy);
            ticket.Price = price;

            // Crear el ticket usando el servicio
            var createdTicket = _ticketService.CreateTicket(ticket.Route.NameRoute, ticket.Category.Name, ticket.Seat.Type, strategy, HttpContext.Session);

            // Guardar el ticket en la base de datos
            //_context.Tickets.Add(createdTicket);  // Añadir el ticket al contexto de la base de datos
            await _context.SaveChangesAsync();    // Guardar los cambios en la base de datos

            return RedirectToAction("Index"); // Redirigir a la vista de lista o mostrar una confirmación
        }


        // GET: Tickets/Index
        public async Task<IActionResult> Index()
        {
            var sistemaVentasDBContext = _context.Tickets.Include(t => t.Category).Include(t => t.Customer).Include(t => t.Route).Include(t => t.Seat);
            return View(await sistemaVentasDBContext.ToListAsync());
        }

        // GET: Tickets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.Customer)
                .Include(t => t.Route)
                .Include(t => t.Seat)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // GET: Tickets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket == null)
            {
                return NotFound();
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", ticket.CategoryId);
            ViewData["CustomerId"] = new SelectList(_context.Client, "ClientId", "Email", ticket.CustomerId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "NameRoute", ticket.RouteId);
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "Type", ticket.SeatId);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketId,Price,Delivered,SeatId,CustomerId,CategoryId,RouteId")] Ticket ticket)
        {
            if (id != ticket.TicketId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticket);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketExists(ticket.TicketId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "CategoryId", "Name", ticket.CategoryId);
            ViewData["CustomerId"] = new SelectList(_context.Client, "ClientId", "Email", ticket.CustomerId);
            ViewData["RouteId"] = new SelectList(_context.Routes, "RouteId", "NameRoute", ticket.RouteId);
            ViewData["SeatId"] = new SelectList(_context.Seats, "SeatId", "Type", ticket.SeatId);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticket = await _context.Tickets
                .Include(t => t.Category)
                .Include(t => t.Customer)
                .Include(t => t.Route)
                .Include(t => t.Seat)
                .FirstOrDefaultAsync(m => m.TicketId == id);
            if (ticket == null)
            {
                return NotFound();
            }

            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ticket = await _context.Tickets.FindAsync(id);
            if (ticket != null)
            {
                _context.Tickets.Remove(ticket);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TicketExists(int id)
        {
            return _context.Tickets.Any(e => e.TicketId == id);
        }
    }
}
