using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data.Common;
using System;
using Microsoft.EntityFrameworkCore;
using CRUD_MVVM.InfrastructureLayer.Utilidades;
using CRUD_MVVM.DomainLayer.Modelos;

namespace CRUD_MVVM.InfrastructureLayer
{

    public class PedidoRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public PedidoRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Pedido>> GetAllPedidosAsync()
        {
            return await _dbContext.Pedidos.ToListAsync();
        }

        public async Task<Pedido> GetPedidoByIdAsync(int id)
        {
            return await _dbContext.Pedidos.FirstOrDefaultAsync(p => p.IdPedidos == id);
        }

        public async Task AddPedidoAsync(Pedido pedido)
        {
            _dbContext.Pedidos.Add(pedido);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdatePedidoAsync(Pedido pedido)
        {
            _dbContext.Pedidos.Update(pedido);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeletePedidoAsync(int id)
        {
            var pedido = await _dbContext.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _dbContext.Pedidos.Remove(pedido);
                await _dbContext.SaveChangesAsync();
            }
        }
    }

}

