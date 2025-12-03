using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiLibreria.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MiLibreria.Controllers
{
    public class GraficasController : Controller
    {
        private readonly BDLibreriaContext _context;

        public GraficasController(BDLibreriaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DataPastel()
        {
            var data = await _context.DetalleVentas
                .GroupBy(dv => dv.Libro.Titulo)
                .Select(g => new
                {
                    name = g.Key, // Nombre del libro
                    y = g.Sum(dv => dv.Cantidad) // Total vendido
                })
                .OrderByDescending(g => g.y)
                .Take(10) // Opcional: Solo los 10 más vendidos
                .ToListAsync();

            return Json(data);
        }
    }
}

