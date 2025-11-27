using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiLibreria.Models
{
    public class Proveedor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProveedorId { get; set; }



        [StringLength(100)]
        public string Direccion { get; set; }

        [Phone]
        public string Telefono { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [StringLength(50)]
        public string PersonaContacto { get; set; }


        public virtual ICollection<Compra> Compras { get; set; }
    }

}
