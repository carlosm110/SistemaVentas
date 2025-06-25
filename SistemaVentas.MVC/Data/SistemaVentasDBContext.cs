using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SistemaVentas.Model;

    public class SistemaVentasDBContext : DbContext
    {
        public SistemaVentasDBContext (DbContextOptions<SistemaVentasDBContext> options)
            : base(options)
        {
        }

        public DbSet<SistemaVentas.Model.Admin> Admins { get; set; } = default!;

public DbSet<SistemaVentas.Model.Category> Categories { get; set; } = default!;

public DbSet<SistemaVentas.Model.Customer> Customers { get; set; } = default!;

public DbSet<SistemaVentas.Model.Route> Routes { get; set; } = default!;

public DbSet<SistemaVentas.Model.Seat> Seats { get; set; } = default!;

public DbSet<SistemaVentas.Model.Ticket> Tickets { get; set; } = default!;
    }
