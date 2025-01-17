using CRUD_MVVM.DataAccess;
using CRUD_MVVM.Modelos;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace CRUD_MVVM.ViewModels
{
    public class PedidoViewModel : INotifyPropertyChanged
    {
        private readonly PedidoRepository _pedidoRepository;
        private readonly UsuarioRepository _usuarioRepository; // Para obtener los usuarios
        private Pedido _pedido;
        private List<Usuario> _usuarios;
        private bool _isLoading;
        private Usuario _usuarioSeleccionado;

        public Pedido Pedido
        {
            get => _pedido;
            set
            {
                if (_pedido != value)
                {
                    _pedido = value;
                    OnPropertyChanged(nameof(Pedido));
                }
            }
        }

        public List<Usuario> Usuarios
        {
            get => _usuarios;
            set
            {
                if (_usuarios != value)
                {
                    _usuarios = value;
                    OnPropertyChanged(nameof(Usuarios));
                }
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                if (_isLoading != value)
                {
                    _isLoading = value;
                    OnPropertyChanged(nameof(IsLoading));
                }
            }
        }

        public Usuario UsuarioSeleccionado // Propiedad para almacenar el usuario seleccionado.
        {
            get => _usuarioSeleccionado;
            set
            {
                if (_usuarioSeleccionado != value)
                {
                    _usuarioSeleccionado = value;
                    OnPropertyChanged(nameof(UsuarioSeleccionado));

                    // Aquí actualizas el UsuarioId en Pedido cuando seleccionas un usuario.
                    Pedido.UsuarioId = _usuarioSeleccionado?.IdUsuario ?? 0;
                }
            }
        }

        public ICommand GuardarPedidoCommand { get; }
        public ICommand CargarPedidoCommand { get; }

        public PedidoViewModel(PedidoRepository pedidoRepository, UsuarioRepository usuarioRepository)
        {
            _pedidoRepository = pedidoRepository;
            _usuarioRepository = usuarioRepository;
            Pedido = new Pedido();
            GuardarPedidoCommand = new Command(async () => await GuardarPedido());
            CargarPedidoCommand = new Command<int>(async (id) => await CargarPedido(id));

            // Cargar usuarios al inicio
            CargarUsuarios();
        }

        public async Task CargarUsuarios()
        {
            IsLoading = true;
            // Obtener lista de usuarios
            Usuarios = new List<Usuario>(await _usuarioRepository.GetUsuariosAsync());
            IsLoading = false;
        }

        public async Task GuardarPedido()
        {
            IsLoading = true;

            // Validar que se haya seleccionado un usuario
            if (Pedido.UsuarioId == 0)
            {
                // Puedes agregar lógica de validación aquí
                IsLoading = false;
                return;
            }

            // Calcular el monto total (puedes agregar tu lógica de cálculo aquí)
            Pedido.MontoTotal = Pedido.MontoTotal;

            if (Pedido.Id == 0)
            {
                Console.WriteLine(Pedido.ToString());
                await _pedidoRepository.AddPedidoAsync(Pedido);
            }
            else
            {
                Console.WriteLine(Pedido.ToString());
                await _pedidoRepository.UpdatePedidoAsync(Pedido);
                
            }

            IsLoading = false;
            await Shell.Current.GoToAsync("VerPedidosPage");
        }

        public async Task CargarPedido(int id)
        {
            if (id <= 0) return;

            IsLoading = true;

            var pedido = await _pedidoRepository.GetPedidoByIdAsync(id);
            if (pedido != null)
            {
                Pedido = pedido;
            }

            IsLoading = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
