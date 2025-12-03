using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiLibreria.Models
{
    public class Proveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProveedorId { get; set; }

        [StringLength(20)]
        [Display(Name = "RUC")]
        public string? RUC { get; set; }

        [Required]
        [StringLength(100)]
        public string Direccion { get; set; }

        [Required]
        [Phone]
        public string Telefono { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Persona de Contacto")]
        public string PersonaContacto { get; set; }
    }
}