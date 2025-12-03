using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MiLibreria.Models;

namespace MiLibreria.ViewModels
{
    public class CompraDetalleViewModel
    {
        [Required(ErrorMessage = "El número de factura es requerido")]
        [StringLength(20, ErrorMessage = "El número de factura no puede superar los 20 caracteres.")]
        public string NumeroFactura { get; set; }

        [Required(ErrorMessage = "La fecha de compra es requerida")]
        [DataType(DataType.Date)]
        public DateTime FechaCompra { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "El proveedor es requerido")]
        public int ProveedorId { get; set; }

        public IEnumerable<Proveedor> Proveedores { get; set; } = new List<Proveedor>();

        public List<DetalleCompraViewModel> DetallesCompra { get; set; } = new List<DetalleCompraViewModel>();

        public IEnumerable<Libro> Libros { get; set; } = new List<Libro>();

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalCompra => DetallesCompra?.Sum(d => d.Subtotal) ?? 0;
    }
}

