using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaVentas.APIConsumer;
using SistemaVentas.Model;
using SistemaVentas.MVC.Services.Business;
using SistemaVentas.MVC.Services.Factories;
using SistemaVentas.MVC.Services.Factories.SistemaVentas.MVC.Services.Factories;
using SistemaVentas.MVC.Services.Observers;
using SistemaVentas.MVC.Services.Strategies;
using SistemaVentas.MVC.Services.Utilities;

namespace SistemaVentas.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Configuración de los endpoints de la API
            Crud<Seat>.EndPoint = "https://localhost:7269/api/Seats";
            Crud<Ticket>.EndPoint = "https://localhost:7269/api/Tickets";
            Crud<Category>.EndPoint = "https://localhost:7269/api/Categories";
            Crud<Model.Route>.EndPoint = "https://localhost:7269/api/Routes";
            Crud<Client>.EndPoint = "https://localhost:7269/api/Clients";

            var builder = WebApplication.CreateBuilder(args);

            // Configuración de dependencias
            builder.Services.AddScoped<IBoletoFactory, TicketFactory>(); // Registra TicketFactory como IBoletoFactory

            // Configuración del DbContext para conexión con PostgreSQL
            builder.Services.AddDbContext<SistemaVentasDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("SistemaVentasDBContext") ?? throw new InvalidOperationException("Connection string 'SistemaVentasDBContext' not found.")));

            // Registra el servicio TicketService como Scoped
            builder.Services.AddScoped<TicketService>();

            // Registra el generador de PDF
            builder.Services.AddTransient<IPdfGenerator, PdfGenerator>();

            // Registra HttpContextAccessor para acceder a la sesión
            builder.Services.AddHttpContextAccessor();

            // Registra las estrategias de precios
            builder.Services.AddScoped<ChildPriceStrategy>();
            builder.Services.AddScoped<AdultPriceStrategy>();
            builder.Services.AddScoped<SeniorPriceStrategy>();

            // Registra el PriceCalculator para que sea inyectado cuando sea necesario
            builder.Services.AddScoped<PriceCalculator>();

            // Registra el CustomerNotifier como scoped para ser usado en el servicio
            builder.Services.AddScoped<CustomerNotifier>();

            // Configuración de soporte para sesiones
            builder.Services.AddDistributedMemoryCache(); // Utiliza la memoria como caché
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
                options.Cookie.HttpOnly = true; // Evitar que las cookies de sesión se accedan desde el lado del cliente
                options.Cookie.IsEssential = true; // Hacer que la cookie sea esencial para el funcionamiento de la aplicación
            });

            // Agregar servicios para controladores con vistas
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configuración del pipeline de HTTP
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Habilitar el uso de sesiones
            app.UseSession();

            app.UseAuthorization();

            // Definir la ruta por defecto para el controlador y la acción
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            // Ejecutar la aplicación
            app.Run();
        }
    }
}
