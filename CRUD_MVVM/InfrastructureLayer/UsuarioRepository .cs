using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CRUD_MVVM.InfrastructureLayer.Utilidades;
using CRUD_MVVM.DomainLayer.Modelos;

namespace CRUD_MVVM.InfrastructureLayer
{
    public class UsuarioRepository
    {
        private readonly ApplicationDbContext _context;

        public UsuarioRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Método para obtener todos los usuarios
        public async Task<List<Usuario>> GetUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // Método para obtener un usuario por su Id
        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
        }

        // Método para agregar un nuevo usuario
        public async Task AddUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            try
            {
                // Código que guarda los cambios en la base de datos
                await _context.SaveChangesAsync();
                Console.WriteLine(_context);
            }
            catch (DbUpdateException ex)
            {
                // Registra la excepción interna para obtener más detalles
                Console.WriteLine("Error al guardar los cambios: " + ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                }
            }
        }

        // Método para actualizar un usuario
        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }

        // Método para eliminar un usuario
        public async Task DeleteUsuarioAsync(int id)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.IdUsuario == id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
        }
    }
}
