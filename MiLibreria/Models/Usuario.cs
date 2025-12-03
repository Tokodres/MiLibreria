using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MiLibreria.Models
{
    public class Usuario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 6)]
        public string Clave { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Rol")]
        public int RolId { get; set; }

        public bool Estado { get; set; } = true;

        [DataType(DataType.DateTime)]
        [Display(Name = "Fecha de Registro")]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        [Display(Name = "Último Acceso")]
        public DateTime? UltimoAcceso { get; set; }

        // Nuevas propiedades para restablecimiento de contraseña
        [StringLength(10)]
        public string? CodigoVerificacion { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? FechaExpiracionCodigo { get; set; }

        // Navegación
        public virtual Rol Rol { get; set; } = null!;
    }
}