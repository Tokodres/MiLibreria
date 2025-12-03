using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiLibreria.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MiLibreria.Controllers
{
    [Authorize] // Solo usuarios autenticados
    public class PerfilController : Controller
    {
        private readonly BDLibreriaContext _context;

        public PerfilController(BDLibreriaContext context)
        {
            _context = context;
        }

        // GET: Perfil/Edit - Para que el usuario edite su propio perfil
        public async Task<IActionResult> Edit()
        {
            // Obtener el correo del usuario actual
            var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Index", "Acceso");
            }

            // Buscar usuario por correo
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == userEmail);

            if (usuario == null)
            {
                return RedirectToAction("Index", "Acceso");
            }

            return View(usuario);
        }

        // POST: Perfil/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el usuario actual de la base de datos
                    var usuarioActual = await _context.Usuarios
                        .AsNoTracking()
                        .FirstOrDefaultAsync(u => u.UsuarioId == usuario.UsuarioId);

                    if (usuarioActual == null)
                    {
                        return NotFound();
                    }

                    // Verificar que el usuario que intenta editar es el mismo que está logueado
                    var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                    if (usuarioActual.Correo != userEmail)
                    {
                        // Si no es el mismo, redirigir a acceso denegado
                        return RedirectToAction("AccessDenied", "Home");
                    }

                    // Mantener los datos que no se deben cambiar
                    usuario.RolId = usuarioActual.RolId;
                    usuario.Estado = usuarioActual.Estado;
                    usuario.FechaRegistro = usuarioActual.FechaRegistro;

                    // Si la contraseña está vacía, mantener la anterior
                    if (string.IsNullOrEmpty(usuario.Clave))
                    {
                        usuario.Clave = usuarioActual.Clave;
                    }

                    _context.Update(usuario);
                    await _context.SaveChangesAsync();

                    TempData["MensajeExito"] = "Perfil actualizado correctamente.";
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
                return RedirectToAction("Edit");
            }
            return View(usuario);
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}