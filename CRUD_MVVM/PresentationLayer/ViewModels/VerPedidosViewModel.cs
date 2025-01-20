using CRUD_MVVM.DomainLayer.Modelos;
using CRUD_MVVM.InfrastructureLayer;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CRUD_MVVM.PresentationLayer.ViewModels
{
    public class VerPedidosViewModel
    {
        private readonly PedidoRepository _pedidoRepository;
        private readonly UsuarioRepository _usuarioRepository;

        public ObservableCollection<Pedido> Pedidos { get; set; }
        public ObservableCollection<Usuario> Usuarios { get; set; }
        public ICommand EliminarPedidoCommand { get; }

        public VerPedidosViewModel(PedidoRepository pedidoRepository, UsuarioRepository usuarioRepository)
        {
            _pedidoRepository = pedidoRepository;
            _usuarioRepository = usuarioRepository;
            Pedidos = new ObservableCollection<Pedido>();
            Usuarios = new ObservableCollection<Usuario>();
            EliminarPedidoCommand = new Command<int>(async (pedidoId) => await EliminarPedido(pedidoId));
            Pedidos.Clear();
            CargarPedidos();
        }

        public async Task CargarPedidos()
        {
            // Cargar pedidos
            var pedidos = await _pedidoRepository.GetAllPedidosAsync();
            Pedidos.Clear();
            foreach (var pedido in pedidos)
            {
                // Obtener el usuario correspondiente
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(pedido.UsuarioId);
                if (usuario != null)
                {
                    // Asignar el nombre completo del usuario al pedido
                    pedido.Usuario = usuario;
                }
                Pedidos.Add(pedido);
            }
        }
        public async Task EliminarPedido(int pedidoId)
        {
            var pedido = Pedidos.FirstOrDefault(p => p.IdPedidos == pedidoId);
            if (pedido != null)
            {
                // Eliminar de la base de datos
                await _pedidoRepository.DeletePedidoAsync(pedidoId);

                // Eliminar de la colección ObservableCollection
                Pedidos.Remove(pedido);
            }
        }

    }
}
