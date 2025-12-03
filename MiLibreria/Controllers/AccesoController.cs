using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using MiLibreria.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

namespace MiLibreria.Controllers
{
    public class AccesoController : Controller
    {
        private readonly BDLibreriaContext _context;

        public AccesoController(BDLibreriaContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public async Task<IActionResult> Index(Usuario usuario)
        {
            if (usuario == null || string.IsNullOrEmpty(usuario.Correo) || string.IsNullOrEmpty(usuario.Clave))
            {
                ViewBag.Error = "Por favor ingresa correo y contraseña";
                return View();
            }

            var usuarioEncontrado = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u =>
                    u.Correo == usuario.Correo &&
                    u.Clave == usuario.Clave &&
                    u.Estado == true);

            if (usuarioEncontrado != null)
            {
                // Actualizar último acceso
                usuarioEncontrado.UltimoAcceso = DateTime.Now;
                await _context.SaveChangesAsync();

                // Crear claims
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, usuarioEncontrado.NombreUsuario),
                    new Claim(ClaimTypes.Email, usuarioEncontrado.Correo),
                    new Claim(ClaimTypes.Role, usuarioEncontrado.RolId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    new AuthenticationProperties
                    {
                        IsPersistent = false,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        // GET: Solicitar restablecimiento de contraseña
        public IActionResult SolicitarRestablecimiento()
        {
            return View();
        }

        // POST: Solicitar restablecimiento de contraseña
        [HttpPost]
        public async Task<IActionResult> SolicitarRestablecimiento(SolicitarCodigoViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == model.Correo && u.Estado == true);

                if (usuario != null)
                {
                    var codigo = new Random().Next(100000, 999999).ToString();

                    usuario.CodigoVerificacion = codigo;
                    usuario.FechaExpiracionCodigo = DateTime.Now.AddMinutes(15);

                    await _context.SaveChangesAsync();

                    TempData["CodigoGenerado"] = codigo;
                    TempData["CorreoUsuario"] = model.Correo;

                    return RedirectToAction("RestablecerPassword");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No existe un usuario registrado con ese correo electrónico.");
                }
            }

            return View(model);
        }

        // GET: Restablecer contraseña
        public IActionResult RestablecerPassword()
        {
            if (TempData.ContainsKey("CodigoGenerado"))
            {
                ViewBag.CodigoGenerado = TempData["CodigoGenerado"];
                ViewBag.CorreoUsuario = TempData["CorreoUsuario"];
            }

            return View();
        }

        // POST: Restablecer contraseña
        [HttpPost]
        public async Task<IActionResult> RestablecerPassword(RestablecerPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _context.Usuarios
                    .FirstOrDefaultAsync(u => u.Correo == model.Correo && u.Estado == true);

                if (usuario != null)
                {
                    if (usuario.CodigoVerificacion == model.CodigoVerificacion &&
                        usuario.FechaExpiracionCodigo > DateTime.Now)
                    {
                        usuario.Clave = model.NuevaClave;
                        usuario.CodigoVerificacion = null;
                        usuario.FechaExpiracionCodigo = null;

                        await _context.SaveChangesAsync();

                        TempData["MensajeExito"] = "Contraseña restablecida exitosamente. Ahora puedes iniciar sesión con tu nueva contraseña.";
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Código de verificación incorrecto o expirado.");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No existe un usuario registrado con ese correo electrónico.");
                }
            }

            return View(model);
        }

        // AJAX: Verificar código de verificación
        [HttpPost]
        public async Task<IActionResult> VerificarCodigo(string correo, string codigo)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Estado == true);

            if (usuario != null && usuario.CodigoVerificacion == codigo &&
                usuario.FechaExpiracionCodigo > DateTime.Now)
            {
                return Json(new { valido = true });
            }

            return Json(new { valido = false, mensaje = "Código incorrecto o expirado" });
        }

        // Logout
        public async Task<IActionResult> Salir()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Acceso");
        }

        // GET: Página de acceso denegado
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}