using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiLibreria.Models
{
    public class DetalleCompra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetalleCompraId { get; set; }
        public int CompraId { get; set; }
        public int LibroId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Cantidad { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal PrecioUnitario { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal Subtotal { get; set; }

        public virtual Compra Compra { get; set; }
        public virtual Libro Libro { get; set; }
    }
}
