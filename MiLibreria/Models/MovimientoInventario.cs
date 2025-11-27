using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiLibreria.Models
{
    public class MovimientoInventario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MovimientoId { get; set; }
        public int LibroId { get; set; }

        [Required]
        [StringLength(20)]
        public string TipoMovimiento { get; set; } // "Entrada" o "Salida"

        [Required]
        public int Cantidad { get; set; }

        public DateTime FechaMovimiento { get; set; }

        [StringLength(50)]
        public string Referencia { get; set; } // Número de factura de compra o venta

        [Required]
        [Range(0, 9999999999.99)]
        public decimal PrecioUnitario { get; set; }

        public virtual Libro Libro { get; set; }
    }
}
