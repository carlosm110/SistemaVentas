using Microsoft.AspNetCore.Mvc;
using SistemaVentas.MVC.Models;
using System.Diagnostics;

namespace SistemaVentas.MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            // Para verificar que esté entrando correctamente
            ViewBag.RoleDebug = $"IsAdmin en sesión: {isAdmin}";

            if (isAdmin == "True")
            {
                return View("AdminDashboard");
            }
            else
            {
                return View("ClientDashboard");
            }
        }
        


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
