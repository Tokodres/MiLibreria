using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiLibreria.Models
{
    public class Compra
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CompraId { get; set; }
        public int ProveedorId { get; set; }
        public DateTime FechaCompra { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal Total { get; set; }

        [Required]
        [StringLength(20)]
        public string NumeroFactura { get; set; }

        [StringLength(20)]
        public string EstadoCompra { get; set; }

        public virtual Proveedor Proveedor { get; set; }
        public virtual ICollection<DetalleCompra> Detallescompra { get; set; }




    }
}
