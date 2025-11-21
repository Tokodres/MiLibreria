using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MiLibreria.Models;

namespace MiLibreria.Models;

public partial class BDLibreriaContext : DbContext
{
    public BDLibreriaContext()
    {
    }

    public BDLibreriaContext(DbContextOptions<BDLibreriaContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<Libro> Libros { get; set; }
    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }
    public virtual DbSet<Venta> Ventas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Venta>()
            .HasOne(v => v.Cliente)
            .WithMany(c => c.Ventas)
            .HasForeignKey(v => v.ClienteId);

        modelBuilder.Entity<DetalleVenta>()
            .HasOne(d => d.Venta)
            .WithMany(v => v.DetallesVenta)
            .HasForeignKey(d => d.VentaId);


        modelBuilder.Entity<DetalleVenta>()
            .HasOne(d => d.Libro)
            .WithMany(l => l.DetallesVenta) // Si el modelo Libro tiene una propiedad DetallesVenta, esto está bien.
            .HasForeignKey(d => d.LibroId);

        modelBuilder.Entity<Venta>()
           .HasKey(v => v.VentaId)
           .HasName("PK_Ventas");
    }
}