using Microsoft.EntityFrameworkCore;
using MiLibreria.Models;
using System.Threading.Tasks;

namespace MiLibreria.Logica
{
    public class LogicaUsuarios
    {
        private readonly BDLibreriaContext _context;

        public LogicaUsuarios(BDLibreriaContext context)
        {
            _context = context;
        }

        public async Task<Usuario> EncontrarUsuarioAsync(string correo, string clave)
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u =>
                    u.Correo == correo &&
                    u.Clave == clave &&
                    u.Estado == true);
        }

        public async Task ActualizarUltimoAccesoAsync(int usuarioId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario != null)
            {
                usuario.UltimoAcceso = DateTime.Now;
                await _context.SaveChangesAsync();
            }
        }
    }
}