using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD_MVVM.DomainLayer.Modelos
{
    public class Pedido
    {
        [Key]
        public int IdPedidos { get; set; }
        public int UsuarioId { get; set; } // Clave foránea
        public DateTime FechaPedido { get; set; }
        public float MontoTotal { get; set; }
        public string Estado { get; set; }
        public Usuario Usuario { get; set; }
    }
}
