using SistemaVentas.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using SistemaVentas.APIConsumer;

namespace GestionMantenimientoFlotas.MVC.Controllers
{
    public class AuthController : Controller
    {
        // GET: Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var admins = Crud<Admin>.GetAll();  // Obtener todos los administradores desde la API o base de datos
            var admin = admins.FirstOrDefault(a => a.Email == username && a.Password == password);  // Verificar si el admin existe

            if (admin != null)
            {
                // Guardar en la sesión si el login es exitoso
                HttpContext.Session.SetString("User", username);
                return RedirectToAction("Index", "Home");  // Redirigir a la página principal
            }
            else
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();  // Volver a la vista de login si hay un error
            }
        }

        // GET: Logout
        public IActionResult Logout()
        {
            // Limpiar la sesión para cerrar sesión
            HttpContext.Session.Remove("User");
            return RedirectToAction("Login");  // Redirigir al login
        }
    }
}
