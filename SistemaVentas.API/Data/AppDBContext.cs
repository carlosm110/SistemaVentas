using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Model;

    public class AppDBContext : DbContext
    {
        public AppDBContext (DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<SistemaVentas.Model.Seat> Asientos { get; set; } = default!;

public DbSet<SistemaVentas.Model.Boleto> Boletos { get; set; } = default!;

public DbSet<SistemaVentas.Model.Categoria> Categorias { get; set; } = default!;

public DbSet<SistemaVentas.Model.Customer> Clientes { get; set; } = default!;

public DbSet<SistemaVentas.Model.Route> Rutas { get; set; } = default!;
    }
