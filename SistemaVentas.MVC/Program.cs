using SistemaVentas.APIConsumer;
using SistemaVentas.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace SistemaVentas.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Crud<Seat>.EndPoint = "https://localhost:7269/api/Seats";
            Crud<Ticket>.EndPoint = "https://localhost:7269/api/Tickets";
            Crud<Category>.EndPoint = "https://localhost:7269/api/Categories";
            Crud<Model.Route>.EndPoint = "https://localhost:7269/api/Routes";
            Crud<Client>.EndPoint = "https://localhost:7269/api/Clients";



            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<SistemaVentasDBContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("SistemaVentasDBContext") ?? throw new InvalidOperationException("Connection string 'SistemaVentasDBContext' not found.")));

            // Agregar soporte para sesiones
            builder.Services.AddDistributedMemoryCache(); // Utiliza la memoria como caché
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiración de la sesión
                options.Cookie.HttpOnly = true; // Evitar que las cookies de sesión se accedan desde el lado del cliente
                options.Cookie.IsEssential = true; // Hacer que la cookie sea esencial para el funcionamiento de la aplicación
            });

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            // Habilitar sesión
            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Auth}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
