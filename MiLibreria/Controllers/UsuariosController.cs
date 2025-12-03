using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MiLibreria.Models;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;

namespace MiLibreria.Controllers
{
    [Authorize] // Solo usuarios autenticados pueden acceder
    public class UsuariosController : Controller
    {
        private readonly BDLibreriaContext _context;

        public UsuariosController(BDLibreriaContext context)
        {
            _context = context;
        }

        // GET: Usuarios - Solo administradores
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Index()
        {
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();
            return View(usuarios);
        }

        // GET: Usuarios/Create - Solo administradores
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create()
        {
            await CargarRoles();
            return View();
        }

        // POST: Usuarios/Create - Solo administradores
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                // Verificar si el correo ya existe
                var correoExistente = await _context.Usuarios
                    .AnyAsync(u => u.Correo == usuario.Correo);

                if (correoExistente)
                {
                    ModelState.AddModelError("Correo", "Ya existe un usuario con este correo electrónico.");
                    await CargarRoles();
                    return View(usuario);
                }

                // Verificar si el nombre de usuario ya existe
                var usuarioExistente = await _context.Usuarios
                    .AnyAsync(u => u.NombreUsuario == usuario.NombreUsuario);

                if (usuarioExistente)
                {
                    ModelState.AddModelError("NombreUsuario", "Ya existe un usuario con este nombre de usuario.");
                    await CargarRoles();
                    return View(usuario);
                }

                // Asignar fecha de registro
                usuario.FechaRegistro = DateTime.Now;
                usuario.Estado = true; // Usuario activo por defecto

                _context.Add(usuario);
                await _context.SaveChangesAsync();

                TempData["MensajeExito"] = "Usuario creado exitosamente.";
                return RedirectToAction(nameof(Index));
            }

            await CargarRoles();
            return View(usuario);
        }

        // GET: Usuarios/Edit/5 - Solo administradores
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            await CargarRoles();
            return View(usuario);
        }

        // POST: Usuarios/Edit/5 - Solo administradores
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Edit(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Verificar si el correo ya existe (excluyendo al usuario actual)
                    var correoExistente = await _context.Usuarios
                        .AnyAsync(u => u.Correo == usuario.Correo && u.UsuarioId != id);

                    if (correoExistente)
                    {
                        ModelState.AddModelError("Correo", "Ya existe un usuario con este correo electrónico.");
                        await CargarRoles();
                        return View(usuario);
                    }

                    // Verificar si el nombre de usuario ya existe (excluyendo al usuario actual)
                    var usuarioExistente = await _context.Usuarios
                        .AnyAsync(u => u.NombreUsuario == usuario.NombreUsuario && u.UsuarioId != id);

                    if (usuarioExistente)
                    {
                        ModelState.AddModelError("NombreUsuario", "Ya existe un usuario con este nombre de usuario.");
                        await CargarRoles();
                        return View(usuario);
                    }

                    // Obtener usuario actual para mantener algunos datos
                    var usuarioActual = await _context.Usuarios.AsNoTracking()
                        .FirstOrDefaultAsync(u => u.UsuarioId == id);

                    if (usuarioActual != null)
                    {
                        // Mantener la fecha de registro original
                        usuario.FechaRegistro = usuarioActual.FechaRegistro;
                        // Mantener la contraseña si no se cambió (campo vacío)
                        if (string.IsNullOrEmpty(usuario.Clave))
                        {
                            usuario.Clave = usuarioActual.Clave;
                        }
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    TempData["MensajeExito"] = "Usuario actualizado exitosamente.";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.UsuarioId))
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

            await CargarRoles();
            return View(usuario);
        }

        // GET: Usuarios/Details/5 - Solo administradores
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // GET: Usuarios/Delete/5 - Solo administradores
        [Authorize(Roles = "1")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(m => m.UsuarioId == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        // POST: Usuarios/Delete/5 - Solo administradores
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "1")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                // No eliminar el usuario admin por seguridad
                if (usuario.RolId == 1 && usuario.Correo == "admin@libreria.com")
                {
                    TempData["Error"] = "No se puede eliminar el usuario administrador principal.";
                    return RedirectToAction(nameof(Index));
                }

                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();

                TempData["MensajeExito"] = "Usuario eliminado exitosamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Usuarios/ActivarDesactivar/5 - Solo administradores
        [Authorize(Roles = "1")]
        public async Task<IActionResult> ActivarDesactivar(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                // No desactivar el usuario admin por seguridad
                if (usuario.RolId == 1 && usuario.Correo == "admin@libreria.com")
                {
                    TempData["Error"] = "No se puede desactivar el usuario administrador principal.";
                    return RedirectToAction(nameof(Index));
                }

                usuario.Estado = !usuario.Estado;
                await _context.SaveChangesAsync();

                TempData["MensajeExito"] = usuario.Estado ?
                    "Usuario activado exitosamente." :
                    "Usuario desactivado exitosamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }

        private async Task CargarRoles()
        {
            var roles = await _context.Roles.ToListAsync();
            ViewBag.Roles = new SelectList(roles, "RolId", "Nombre");
        }
    }
}