using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiLibreria.Models;
using MiLibreria.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace MiLibreria.Controllers
{
    public class VentasController : Controller
    {
        private readonly BDLibreriaContext _context;

        public VentasController(BDLibreriaContext context)
        {
            _context = context;
        }
        public IActionResult Reporte(string NumeroFactura)
        {
            // Filtrar libros por nro factura si se proporciona
            var ventaQuery = _context.Ventas.AsQueryable();

            if (!string.IsNullOrEmpty(NumeroFactura))
            {
                ventaQuery = ventaQuery.Where(f => EF.Functions.Like(f.NumeroFactura, "%" + NumeroFactura + "%"));
              
            }

            var venta = ventaQuery.ToList();

            // Asegurarse de que siempre se pasa una lista, aunque esté vacía
            return View(venta); 
        }

        public async Task<IActionResult> Create()
        {
           

            var viewModel = new VentaDetalleViewModel
            {
                FechaVenta = DateTime.Now,
                Clientes = await _context.Clientes.ToListAsync(),
                Libros = await _context.Libros.Where(l => l.Stock > 0).ToListAsync()
         
            };

            // Inicializar la lista de detalles de la venta en el ViewModel
            viewModel.DetallesVenta = new List<DetalleVentaViewModel>();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VentaDetalleViewModel viewModel)
        {
            //  if (ModelState.IsValid)
            // {
            

            var venta = new Venta

            {

                ClienteId = viewModel.ClienteId,
                FechaVenta = viewModel.FechaVenta,
                NumeroFactura = viewModel.NumeroFactura,
                MetodoPago = viewModel.MetodoPago,
                DetallesVenta = viewModel.DetallesVenta
                    .Select(dv => new DetalleVenta
                    {
                        LibroId = dv.LibroId,
                        Cantidad = dv.Cantidad,
                        PrecioUnitario = dv.PrecioUnitario,
                        Subtotal = dv.Subtotal,
                       
                    })
                    .ToList()
            };

            // Actualizar el stock de los libros
            foreach (var detalle in venta.DetallesVenta)
            {
                var libro = await _context.Libros.FindAsync(detalle.LibroId);
                libro.Stock -= detalle.Cantidad;
                venta.Total += detalle.Subtotal;
             
            }
            venta.EstadoVenta = "Pendiente";
          
        

            _context.Add(venta);
            await _context.SaveChangesAsync();

            return RedirectToAction("Create"); // Redirigir a la acción Index o a otra vista apropiada
                                              // }

            // Si hay errores de validación, volver a mostrar la vista con los datos ingresados
            return View(viewModel);
        }

        public async Task<IActionResult> Consultar(string nroFactura)
        {
            if (string.IsNullOrEmpty(nroFactura))
            {
                return View(null); // O podrías retornar un mensaje de error
            }

            var venta = await _context.Ventas
                .Where(v => v.NumeroFactura == nroFactura)
                .Include(v => v.Cliente) // Incluye el cliente
                .Include(v => v.DetallesVenta)
                    .ThenInclude(dv => dv.Libro) // Incluye los libros de los detalles
                .FirstOrDefaultAsync();

            if (venta == null)
            {
                ViewBag.Mensaje = "Venta no encontrada.";
                return View(null);
            }

            var viewModel = new VentaDetalleViewModel
            {
                NumeroFactura = venta.NumeroFactura,
                FechaVenta = venta.FechaVenta,
               
                MetodoPago = venta.MetodoPago,
                Clientes = new List<Cliente> { venta.Cliente },
                DetallesVenta = venta.DetallesVenta.Select(dv => new DetalleVentaViewModel
                {
                    Libro = dv.Libro.Titulo,
                    Cantidad = dv.Cantidad,
                    PrecioUnitario = dv.PrecioUnitario,
                   
                }).ToList()
            };


            return View(viewModel);
        }
    }
}