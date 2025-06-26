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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string username, string password)
        {
            var users = Crud<Client>.GetAll();
            var user = users.FirstOrDefault(u => u.Email == username && u.Password == password);

            if (user != null)
            {
                // GUARDAR LA SESIÓN CON EL CustomerId Y CORREO
                HttpContext.Session.SetString("User", username);
                HttpContext.Session.SetInt32("CustomerId", user.ClientId);  // Usamos el ClientId
                HttpContext.Session.SetString("CustomerEmail", user.Email);  // Guardamos el correo
                HttpContext.Session.SetInt32("IsAdmin", user.IsAdmin ? 1 : 0);  // Guardar si es admin

                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("", "Username or password is incorrect");
                return View();
            }
        }

        // GET: Register
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(Client newUser)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el correo ya está registrado
                var users = Crud<Client>.GetAll();
                if (users.Any(u => u.Email == newUser.Email))
                {
                    ModelState.AddModelError("", "El correo electrónico ya está registrado.");
                    return View();
                }

                // Usar el método Crud para crear un nuevo usuario en la base de datos
                try
                {
                    var createdUser = Crud<Client>.Create(newUser);

                    // Guardar en la sesión para autenticar automáticamente
                    HttpContext.Session.SetString("User", createdUser.Email);
                    HttpContext.Session.SetInt32("CustomerId", createdUser.ClientId);  // Guardar CustomerId
                    HttpContext.Session.SetString("CustomerEmail", createdUser.Email);  // Guardar correo del cliente

                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al registrar el usuario: {ex.Message}");
                    return View();
                }
            }

            return View(newUser);
        }

        // GET: Logout
        public IActionResult Logout()
        {
            // Limpiar la sesión para cerrar sesión
            HttpContext.Session.Remove("User");
            HttpContext.Session.Remove("CustomerId");  // Limpiar el CustomerId también
            HttpContext.Session.Remove("CustomerEmail");  // Limpiar el correo del cliente
            return RedirectToAction("Login");  // Redirigir al login
        }
    }
}
