using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace CRUD_MVVM.DataAccess
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IDbConnection _dbConnection; 

        
        public Repository(IConfiguration configuration)
        {
            _dbConnection = new SqlConnection(DatabaseConfig.ConnectionString); 
        }

        
        public void EnsureConnectionAsync()
        {
            if (_dbConnection.State != ConnectionState.Open)
            {
                _dbConnection.Open();  
            }
        }

        
        private string GetPrimaryKeyColumn()
        {
            var properties = typeof(T).GetProperties();
            var idProperty = properties.FirstOrDefault(p => p.Name == "IdUsuario");
            if (idProperty != null)
            {
                return "IdUsuario";
            }

            throw new InvalidOperationException($"No se encontró la propiedad 'Id' en la clase {typeof(T).Name}.");
        }

        
        public async Task<T> AddAsync(T entity)
        {
            try
            {
                
                EnsureConnectionAsync();
         
                var tableName = typeof(T).Name;
                var properties = typeof(T).GetProperties()
                    .Where(p => p.Name != "IdUsuario")  
                    .Select(p => p.Name);

                
                var sql = $"INSERT INTO {tableName} ({string.Join(", ", properties)}) " +
                          $"VALUES (@{string.Join(", @", properties)}); " +
                          $"SELECT CAST(SCOPE_IDENTITY() AS INT);";

                
                var id = await _dbConnection.QuerySingleAsync<int>(sql, entity);

                var idProperty = typeof(T).GetProperty("IdUsuario");
                if (idProperty != null)
                {
                    idProperty.SetValue(entity, id);
                }

                return entity;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al insertar: {ex.Message}");
                throw;
            }
        }


        public async Task DeleteAsync(int id)
        {
            try
            {
                EnsureConnectionAsync();
                var tableName = typeof(T).Name;
                var primaryKey = GetPrimaryKeyColumn();
                var sql = $"DELETE FROM {tableName} WHERE {primaryKey} = @Id";
                await _dbConnection.ExecuteAsync(sql, new { Id = id });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar: {ex.Message}");
                throw;
            }
        }

        public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                EnsureConnectionAsync();
                var tableName = typeof(T).Name;
                var sql = $"SELECT * FROM {tableName}";
                return await _dbConnection.QueryAsync<T>(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener todas las entidades: {ex.Message}");
                throw;
            }
        }

        
        public async Task<T> GetByIdAsync(int id)
        {
            try
            {
                EnsureConnectionAsync();
                var tableName = typeof(T).Name;
                var primaryKey = GetPrimaryKeyColumn();
                var sql = $"SELECT * FROM {tableName} WHERE {primaryKey} = @IdUsuario";
                return (await _dbConnection.QueryAsync<T>(sql, new { IdUsuario = id })).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener por Id: {ex.Message}");
                throw;
            }
        }

        
        public async Task UpdateAsync(T entity)
        {
            try
            {
               
                EnsureConnectionAsync();

                var tableName = typeof(T).Name;

               
                var properties = typeof(T).GetProperties()
                    .Where(p => p.Name != "IdUsuario")  
                    .Select(p => $"{p.Name} = @{p.Name}");  
                
                var sql = $"UPDATE {tableName} SET {string.Join(", ", properties)} WHERE IdUsuario = @IdUsuario";
               
                await _dbConnection.ExecuteAsync(sql, entity);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar: {ex.Message}");
                throw;
            }
        }
    }

   
    
}
