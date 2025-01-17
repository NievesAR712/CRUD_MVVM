using CRUD_MVVM.Modelos;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using System.Data.Common;

namespace CRUD_MVVM.DataAccess
{
    public class PedidoRepository
    {
        protected readonly IDbConnection _dbConnection;
        private readonly string _connectionString = DatabaseConfig.ConnectionString;

        public PedidoRepository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(DatabaseConfig.ConnectionString);
        }

       

        // Agregar un nuevo Pedido
        public async Task<Pedido> AddPedidoAsync(Pedido pedido)
        {
            var query = "INSERT INTO Pedidos (UsuarioId, FechaPedido, MontoTotal, Estado) " +
                        "VALUES (@UsuarioId, @FechaPedido, @MontoTotal, @Estado);" +
                        "SELECT CAST(SCOPE_IDENTITY() AS INT);"; // Obtener el ID del pedido recién insertado

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var id = await connection.QuerySingleAsync<int>(query, pedido);
                pedido.Id = id; // Asignar el ID generado al objeto pedido
                return pedido;
            }
        }

        // Obtener todos los Pedidos
        public async Task<IEnumerable<Pedido>> GetPedidosAsync()
        {
            var query = "SELECT * FROM Pedidos";
            using (var connection =  new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<Pedido>(query);
            }
        }

        // Obtener un Pedido por su ID
        public async Task<Pedido> GetPedidoByIdAsync(int id)
        {
            var query = "SELECT * FROM Pedidos WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QuerySingleOrDefaultAsync<Pedido>(query, new { Id = id });
            }
        }

        // Obtener Pedidos por UsuarioId
        public async Task<IEnumerable<Pedido>> GetPedidosByUsuarioIdAsync(int usuarioId)
        {
            var query = "SELECT * FROM Pedidos WHERE UsuarioId = @UsuarioId";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                return await connection.QueryAsync<Pedido>(query, new { UsuarioId = usuarioId });
            }
        }

        // Actualizar un Pedido
        public async Task UpdatePedidoAsync(Pedido pedido)
        {
            var query = "UPDATE Pedidos SET UsuarioId = @UsuarioId, FechaPedido = @FechaPedido, " +
                        "MontoTotal = @MontoTotal, Estado = @Estado WHERE Id = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(query, pedido);
            }
        }

        // Eliminar un Pedido
        public async Task DeletePedidoAsync(int id)
        {
            var query = "DELETE FROM Pedidos WHERE IdPedidos = @Id";
            using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    connection.Open();
                    var result = await connection.ExecuteAsync(query, new { Id = id });

                    if (result == 0)
                    {
                        // No rows were deleted
                        Console.WriteLine($"No se eliminó ningún pedido con el Id {id}");
                    }
                    else
                    {
                        // Successfully deleted
                        Console.WriteLine($"Pedido con Id {id} eliminado exitosamente.");
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al eliminar el pedido: {ex.Message}");
                }
            }
        }


    }
}

