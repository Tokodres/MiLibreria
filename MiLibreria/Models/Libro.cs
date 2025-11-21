using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiLibreria.Models
{
    public class Libro
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int LibroId { get; set; }

      

        [Required(ErrorMessage = "El título es requerido")]
        [StringLength(100)]
        public string Titulo { get; set; }

        [Required(ErrorMessage = "El autor es requerido")]
        [StringLength(100)]
        public string Autor { get; set; }

        [StringLength(50)]
        public string Editorial { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal PrecioCompra { get; set; }

        [Required]
        [Range(0, 9999999999.99)]
        public decimal PrecioVenta { get; set; }

        [Required]
        public int Stock { get; set; }

        public bool Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

        public virtual ICollection<DetalleVenta> DetallesVenta { get; set; }
        
    }
}
