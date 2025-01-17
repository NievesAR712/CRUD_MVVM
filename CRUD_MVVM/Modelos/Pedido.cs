using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_MVVM.Modelos
{
    public class Pedido
    {
        [Key]
        public int Id { get; set; }
        public int UsuarioId { get; set; } // Clave foránea
        public DateTime FechaPedido { get; set; }
        public decimal MontoTotal { get; set; }
        public string Estado { get; set; }
        public Usuario Usuario { get; set; }
    }
}
