using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using MiLibreria.Models;
using MiLibreria.ViewModels;


namespace MiLibreria.Controllers
{
    public class ComprasController : Controller
    {
        private readonly BDLibreriaContext _context;

        public ComprasController(BDLibreriaContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new CompraDetalleViewModel
            {
                FechaCompra = DateTime.Now,
                Proveedores = await _context.Proveedores.ToListAsync(),
                Libros = await _context.Libros.ToListAsync(),
                DetallesCompra = new List<DetalleCompraViewModel>()
            };



            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CompraDetalleViewModel viewModel)
        {
            //  if (ModelState.IsValid)
            // {

            var compra = new Compra

            {

                ProveedorId = viewModel.ProveedorId,
                FechaCompra = viewModel.FechaCompra,
                NumeroFactura = viewModel.NumeroFactura,
                EstadoCompra = "Pendiente",

                Detallescompra = viewModel.DetallesCompra
                    .Select(dc => new DetalleCompra
                    {


                        LibroId = dc.LibroId,
                        Cantidad = dc.Cantidad,
                        PrecioUnitario = dc.PrecioUnitario,
                        Subtotal = dc.Subtotal
                    })
                    .ToList()


            };


         
            foreach (var detalle in viewModel.DetallesCompra)
            {
                var libro = await _context.Libros.FindAsync(detalle.LibroId);
                if (libro != null)
                {
                    var detalleCompra = new DetalleCompra
                    {
                        CompraId = compra.CompraId,
                        LibroId = detalle.LibroId,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario,
                        Subtotal = detalle.Cantidad * detalle.PrecioUnitario
                         
                    };

                    libro.Stock += detalle.Cantidad;
                    compra.Total += detalle.Subtotal;
                }
            }

            _context.Add(compra);

            await _context.SaveChangesAsync();


            return RedirectToAction("Create");

            // }

        }
    }
}