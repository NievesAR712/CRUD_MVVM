using CRUD_MVVM.Modelos;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;


namespace CRUD_MVVM.DataAccess
{
    public class UsuarioRepository
    {
        private readonly IRepository<Usuario> _usuarioRepository;

        public UsuarioRepository(IRepository<Usuario> usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }
        public async Task<Usuario> AddUsuarioAsync(Usuario usuario)
        {
            
            return await _usuarioRepository.AddAsync(usuario);
        }
        public async Task<IEnumerable<Usuario>> GetUsuariosAsync()
        {
            return await _usuarioRepository.GetAllAsync();
        }
        public async Task<Usuario> GetUsuarioByIdAsync(int id)
        {
            return await _usuarioRepository.GetByIdAsync(id);
        }
        public async Task UpdateUsuarioAsync(Usuario usuario)
        {
            await _usuarioRepository.UpdateAsync(usuario);
        }
        public async Task DeleteUsuarioAsync(int id)
        {
            await _usuarioRepository.DeleteAsync(id);
        }
    }
}