using CRUD_MVVM.Modelos;
using CRUD_MVVM.Utilidades;
using Microsoft.EntityFrameworkCore;

namespace CRUD_MVVM.DataAccess
{
    public class UsuarioDBContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string conexionDB = $"Filename={ConexionDB.DevolverRuta("usuarios.db")}";
            optionsBuilder.UseSqlite(conexionDB);   
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(col => col.IdUsuario);
                entity.Property(col => col.IdUsuario).IsRequired().ValueGeneratedOnAdd();
            });
        }
    }
}
