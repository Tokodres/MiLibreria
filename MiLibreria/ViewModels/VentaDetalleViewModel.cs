using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MiLibreria.Models;

namespace MiLibreria.ViewModels
{
    public class VentaDetalleViewModel
    {


        [Required(ErrorMessage = "El número de factura es requerido")]
        public string NumeroFactura { get; set; }

        [Required(ErrorMessage = "La fecha de venta es requerida")]
        public DateTime FechaVenta { get; set; }

        [Required(ErrorMessage = "El cliente es requerido")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El método de pago es requerido")]
        [Display(Name = "Método de Pago")]

        public string MetodoPago { get; set; }
        public IEnumerable<Cliente> Clientes { get; set; }  // Cambiado a IEnumerable
        public List<DetalleVentaViewModel> DetallesVenta { get; set; } = new List<DetalleVentaViewModel>();
        public IEnumerable<Libro> Libros { get; set; }

        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C2}")]
        public decimal TotalVenta => DetallesVenta?.Sum(d => d.Subtotal) ?? 0;
    }
}

