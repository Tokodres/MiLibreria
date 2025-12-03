using System.ComponentModel.DataAnnotations;

namespace MiLibreria.Models
{
    public class SolicitarCodigoViewModel
    {
        [Required(ErrorMessage = "El correo electrónico es requerido")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        [Display(Name = "Correo Electrónico")]
        public string Correo { get; set; } = string.Empty;
    }
}