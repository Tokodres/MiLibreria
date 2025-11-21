namespace MiLibreria.ViewModels
{
    public class DetalleVentaViewModel
    {
        public int LibroId { get; set; }
        public string Libro { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;
    }
}
