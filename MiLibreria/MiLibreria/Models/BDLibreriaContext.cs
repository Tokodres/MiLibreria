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

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<Libro> Libros { get; set; }

    public virtual DbSet<DetalleCompra> DetalleCompras { get; set; }

    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    public virtual DbSet<Proveedor> Proveedores { get; set; }

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
            .WithMany(l => l.DetallesVenta)
            .HasForeignKey(d => d.LibroId);

        modelBuilder.Entity<Venta>()
           .HasKey(v => v.VentaId)
           .HasName("PK_Ventas");

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.CompraId);
            entity.Property(e => e.NumeroFactura)
                  .IsRequired()
                  .HasMaxLength(20);

            entity.HasOne(e => e.Proveedor)
                  .WithMany(p => p.Compras)
                  .HasForeignKey(e => e.ProveedorId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DetalleCompra>()
         .HasKey(d => d.DetalleCompraId);

        modelBuilder.Entity<DetalleCompra>()
            .HasOne(d => d.Compra)
            .WithMany(c => c.Detallescompra)
            .HasForeignKey(d => d.CompraId);

        modelBuilder.Entity<DetalleCompra>()
            .HasOne(d => d.Libro)
            .WithMany(l => l.DetallesCompra)
            .HasForeignKey(d => d.LibroId);
    }

    }
    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=(LocalDb)\\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\\BDLibreria.mdf; Integrated Security=True");
    */


