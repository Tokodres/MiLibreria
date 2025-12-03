using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiLibreria.Models;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;

namespace MiLibreria.Controllers
{
    public class LibrosController : Controller
    {
        private readonly BDLibreriaContext _context;


        public LibrosController(BDLibreriaContext context)
        {
            _context = context;
        }
        public IActionResult Reporte(string Editorial)
        {
            // Filtrar libros por editorial si se proporciona
            var librosQuery = _context.Libros.AsQueryable();

            if (!string.IsNullOrEmpty(Editorial))
            {
                 librosQuery = librosQuery.Where(l => EF.Functions.Like(l.Editorial, "%" + Editorial + "%"));
            }

            var libros = librosQuery.ToList();

            // Asegurarse de que siempre se pasa una lista, aunque esté vacía
            return View(libros); // Pasar la lista de libros a la vista
        }




        public IActionResult DescargarPDF(string Editorial)
        {
            try
            {
                Console.WriteLine($"Valor de Editorial: {Editorial}"); // Verificar el valor de Editorial

                var librosQuery = _context.Libros.AsQueryable();

                if (!string.IsNullOrEmpty(Editorial))
                {
                    librosQuery = librosQuery.Where(l => EF.Functions.Like(l.Editorial, "%" + Editorial + "%"));
                }
                else
                {
                    Console.WriteLine("No se aplicó filtro por Editorial");
                }

                var libros = librosQuery.ToList();
                using (var memoryStream = new MemoryStream())
                {
                    var pdfWriter = new PdfWriter(memoryStream);
                    var pdf = new PdfDocument(pdfWriter);
                    var document = new Document(pdf);

                    // Agregar contenido al PDF
                    document.Add(new Paragraph("Reporte de Libros Filtrados por Editorial")
           .SetTextAlignment(TextAlignment.CENTER)
           .SetFontSize(20));

                    document.Add(new Paragraph(" ")); // Espaciado

                    // Tabla
                    var table = new iText.Layout.Element.Table(6).UseAllAvailableWidth();
                    table.AddHeaderCell("ID");
                    table.AddHeaderCell("Título");
                    table.AddHeaderCell("Autor");
                    table.AddHeaderCell("Precio Compra");
                    table.AddHeaderCell("Precio Venta");
                    table.AddHeaderCell("Stock ");

                    foreach (var libro in libros)
                    {
                        table.AddCell(libro.LibroId.ToString());
                        table.AddCell(libro.Titulo);
                        table.AddCell(libro.Autor);
                        table.AddCell(libro.PrecioCompra.ToString("N"));
                        table.AddCell(libro.PrecioVenta.ToString("N"));
                        table.AddCell(libro.Stock.ToString());
                    }

                    document.Add(table);

                    document.Close();

                    return File(memoryStream.ToArray(), "application/pdf", "ReporteLibros.pdf");
                }
            }
            catch (Exception ex)
            {
                // Aquí puedes capturar el error y visualizarlo para saber lo que falla
                Console.WriteLine(ex.Message);
                return View("Error"); // O redirigir a una página de error
            }
        }

        // Título
       
        // GET: Libros
        public async Task<IActionResult> Index()
        {
            return View(await _context.Libros.ToListAsync());
        }

        // GET: Libros/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.LibroId == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // GET: Libros/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Libros/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("")] Libro libro)
        {
            // if (ModelState.IsValid)
            //{
            _context.Add(libro);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            // }
            return View(libro);
        }

        // GET: Libros/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }
            return View(libro);
        }

        // POST: Libros/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LibroId,Titulo,Autor,Editorial,PrecioCompra,PrecioVenta,Stock,Estado,FechaRegistro")] Libro libro)
        {
            if (id != libro.LibroId)
            {
                return NotFound();
            }

           // if (ModelState.IsValid)
           //{
                try
                {
                    _context.Update(libro);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LibroExists(libro.LibroId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
           // }
            // Aquí puedes revisar los errores con: ModelState.Values.SelectMany(v => v.Errors)
            return View(libro);
        }



        // GET: Libros/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var libro = await _context.Libros
                .FirstOrDefaultAsync(m => m.LibroId == id);
            if (libro == null)
            {
                return NotFound();
            }

            return View(libro);
        }

        // POST: Libros/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var libro = await _context.Libros.FindAsync(id);
            if (libro != null)
            {
                _context.Libros.Remove(libro);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LibroExists(int id)
        {
            return _context.Libros.Any(e => e.LibroId == id);
        }
    }
}
