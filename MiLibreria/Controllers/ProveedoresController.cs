using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiLibreria.Models;

namespace MiLibreria.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly BDLibreriaContext _context;

        public ProveedoresController(BDLibreriaContext context)
        {
            _context = context;
        }

        // GET: Proveedores con búsqueda
        public IActionResult Index(string buscar, string criterio)
        {
            IEnumerable<Proveedor> proveedores;

            if (!string.IsNullOrEmpty(buscar))
            {
                switch (criterio?.ToLower())
                {
                    case "ruc":
                        proveedores = _context.Proveedores
                            .Where(p => p.RUC.Contains(buscar))
                            .ToList();
                        break;
                    case "contacto":
                        proveedores = _context.Proveedores
                            .Where(p => p.PersonaContacto.Contains(buscar))
                            .ToList();
                        break;
                    case "direccion":
                        proveedores = _context.Proveedores
                            .Where(p => p.Direccion.Contains(buscar))
                            .ToList();
                        break;
                    case "email":
                        proveedores = _context.Proveedores
                            .Where(p => p.Email.Contains(buscar))
                            .ToList();
                        break;
                    default:
                        // Búsqueda general en todos los campos
                        proveedores = _context.Proveedores
                            .Where(p => (p.RUC != null && p.RUC.Contains(buscar)) ||
                                        p.PersonaContacto.Contains(buscar) ||
                                        p.Direccion.Contains(buscar) ||
                                        p.Email.Contains(buscar) ||
                                        p.Telefono.Contains(buscar))
                            .ToList();
                        break;
                }
            }
            else
            {
                proveedores = _context.Proveedores.ToList();
            }

            ViewBag.Criterio = criterio ?? "general";
            ViewBag.Buscar = buscar;
            return View(proveedores);
        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.ProveedorId == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Proveedores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProveedorId,RUC,PersonaContacto,Direccion,Telefono,Email")] Proveedor proveedor)
        {
            // Validar RUC único si se proporciona
            if (!string.IsNullOrEmpty(proveedor.RUC))
            {
                var proveedorExistente = await _context.Proveedores
                    .FirstOrDefaultAsync(p => p.RUC == proveedor.RUC);

                if (proveedorExistente != null)
                {
                    ModelState.AddModelError("RUC", "Ya existe un proveedor con este RUC.");
                }
            }

            // Validar ProveedorId único
            var idExistente = await _context.Proveedores
                .FirstOrDefaultAsync(p => p.ProveedorId == proveedor.ProveedorId);

            if (idExistente != null)
            {
                ModelState.AddModelError("ProveedorId", "Ya existe un proveedor con este ID.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(proveedor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }
            return View(proveedor);
        }

        // POST: Proveedores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProveedorId,RUC,PersonaContacto,Direccion,Telefono,Email")] Proveedor proveedor)
        {
            if (id != proveedor.ProveedorId)
            {
                return NotFound();
            }

            // Validar RUC único (excluyendo el actual)
            if (!string.IsNullOrEmpty(proveedor.RUC))
            {
                var proveedorConMismoRUC = await _context.Proveedores
                    .FirstOrDefaultAsync(p => p.RUC == proveedor.RUC && p.ProveedorId != proveedor.ProveedorId);

                if (proveedorConMismoRUC != null)
                {
                    ModelState.AddModelError("RUC", "Ya existe otro proveedor con este RUC.");
                }
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(proveedor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedorExists(proveedor.ProveedorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(proveedor);
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.ProveedorId == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        // POST: Proveedores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor != null)
            {
                _context.Proveedores.Remove(proveedor);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProveedorExists(int id)
        {
            return _context.Proveedores.Any(e => e.ProveedorId == id);
        }
    }
}