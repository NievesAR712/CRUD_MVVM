using System.ComponentModel.DataAnnotations;

namespace CRUD_MVVM.DomainLayer.Modelos
{
    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }
        public string NombreCompleto { get; set; }
        public string Correo { get; set; }
        public required string Password { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}
