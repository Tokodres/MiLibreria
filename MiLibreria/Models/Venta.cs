using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiLibreria.Models
{
    public class Venta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VentaId { get; set; }
        
        public int ClienteId { get; set; }
        public DateTime FechaVenta { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal Total { get; set; }

        [Required]
        [StringLength(20)]
        public string NumeroFactura { get; set; }

        [StringLength(20)]
        public string EstadoVenta { get; set; }

        [StringLength(20)]
        public string MetodoPago { get; set; }

        public virtual Cliente Cliente { get; set; }
        public virtual ICollection<DetalleVenta> DetallesVenta { get; set; }
    }
}
