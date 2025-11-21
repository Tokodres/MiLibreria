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
    public class ClientesController : Controller
    {
        private readonly BDLibreriaContext _context;

        public ClientesController(BDLibreriaContext context)
        {
            _context = context;
        }

     

        // GET: Clientes
        public IActionResult Index(string DNI)
        {
            IEnumerable<Cliente> clientes;

            if (!string.IsNullOrEmpty(DNI))
            {
                // Filtrar clientes por DNI
                clientes = _context.Clientes.Where(c => c.DNI == DNI).ToList();
            }
            else
            {
                // Mostrar todos los clientes si no se especifica un DNI
                clientes = _context.Clientes.ToList();
            }

            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ClienteId,Nombre,Apellido,DNI,Direccion,Telefono,Email")] Cliente cliente)
        {
            // Verificar si ya existe un cliente con el mismo DNI
            var clienteExistente = await _context.Clientes.FirstOrDefaultAsync(c => c.DNI == cliente.DNI);

            if (clienteExistente != null)
            {
                ModelState.AddModelError("DNI", "Ya existe un cliente con este DNI.");
                return View(cliente);
            }

           // if (ModelState.IsValid)
            {
                _context.Add(cliente);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(cliente);
        }


        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClienteId,Nombre,Apellido,DNI,Direccion,Telefono,Email")] Cliente cliente)
        {
            if (id != cliente.ClienteId)
            {
                return NotFound();
            }

          //  if (ModelState.IsValid)
           // {
               // try
                {
                    _context.Update(cliente);
                    await _context.SaveChangesAsync();
               // }
               // catch (DbUpdateConcurrencyException)
                /*{
                    if (!ClienteExists(cliente.ClienteId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
               */ }
                return RedirectToAction(nameof(Index));
          //  }
            return View(cliente);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Clientes
                .FirstOrDefaultAsync(m => m.ClienteId == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(int id)
        {
            return _context.Clientes.Any(e => e.ClienteId == id);
        }
    }
}
