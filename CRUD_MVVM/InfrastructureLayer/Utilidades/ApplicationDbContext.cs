using CRUD_MVVM.DomainLayer.Modelos;
using Microsoft.EntityFrameworkCore;

namespace CRUD_MVVM.InfrastructureLayer.Utilidades
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // Constructor con DbContextOptions
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) // Pasa la configuración al constructor base
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // El método OnConfiguring solo se utiliza si no se pasa configuración en el constructor
            // Pero si ya se pasa configuración en el constructor, no es necesario usar este método.
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\NievesLocal;Database=CRUD_MVVM;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Si deseas personalizar alguna tabla o relación, lo puedes hacer aquí
            modelBuilder.Entity<Pedido>().ToTable("Pedidos");
            modelBuilder.Entity<Usuario>().ToTable("Usuario");
        }
    }
}
