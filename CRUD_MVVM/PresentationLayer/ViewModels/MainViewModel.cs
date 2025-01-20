using CRUD_MVVM.DTOs;
using CRUD_MVVM.DomainLayer.Modelos;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using CRUD_MVVM.Views;
using System.Diagnostics;
using CRUD_MVVM.InfrastructureLayer;

namespace CRUD_MVVM.PresentationLayer.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly UsuarioRepository _usuarioRepository;
        private bool _isLoading;

        private ObservableCollection<UsuarioDTO> _usuarios { get; set; }

        public ObservableCollection<UsuarioDTO> Usuarios
        {
            get { return _usuarios; }
            set
            {
                if (_usuarios != value)
                {
                    _usuarios = value;
                    OnPropertyChanged(nameof(Usuarios)); // Notifica el cambio
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

        // Comandos
        public ICommand CargarUsuariosCommand { get; }
        public ICommand EliminarCommand { get; }
        public ICommand NavegarCrearUsuarioCommand { get; }
        public ICommand EditarUsuarioCommand { get; }

        // Constructor con inyección de dependencias
        public MainViewModel(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            Usuarios = new ObservableCollection<UsuarioDTO>();

            // Inicializar comandos
            CargarUsuariosCommand = new Command(async () => await CargarUsuarios());
            EliminarCommand = new Command<UsuarioDTO>(async (usuario) => await Eliminar(usuario));
            NavegarCrearUsuarioCommand = new Command(async () => await NavegarCrearUsuario());
            EditarUsuarioCommand = new Command<UsuarioDTO>(async (usuario) => await EditarUsuario(usuario));

            // Cargar usuarios al iniciar
            _ = CargarUsuarios();
        }

        // Cargar todos los usuarios
        private async Task CargarUsuarios()
        {
            IsLoading = true;

            Usuarios.Clear();
            var usuarios = await _usuarioRepository.GetUsuariosAsync();

            foreach (var usuario in usuarios)
            {
                Usuarios.Add(new UsuarioDTO
                {
                    IdUsuario = usuario.IdUsuario,
                    NombreCompleto = usuario.NombreCompleto,
                    Correo = usuario.Correo,
                    Password = usuario.Password,
                    FechaRegistro = usuario.FechaRegistro
                });
            }

            IsLoading = false;
        }

        public void AgregarUsuarioALista(UsuarioDTO usuarioDto, bool esCrear)
        {
            if (esCrear)
            {
                Usuarios.Add(usuarioDto);
            }
            else
            {
                var usuario = Usuarios.FirstOrDefault(u => u.IdUsuario == usuarioDto.IdUsuario);
                if (usuario != null)
                {
                    usuario.NombreCompleto = usuarioDto.NombreCompleto;
                    usuario.Correo = usuarioDto.Correo;
                    usuario.Password = usuarioDto.Password;
                    usuario.FechaRegistro = usuarioDto.FechaRegistro;
                    // Notificar cambios al objeto en la lista
                    OnPropertyChanged(nameof(Usuarios));
                }
            }
        }


        private async Task Eliminar(UsuarioDTO usuarioDto)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "Desea eliminar el Usuario?", "Si", "NO");

            if (answer)
            {
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(usuarioDto.idUsuario);
                if (usuario != null)
                {
                    await _usuarioRepository.DeleteUsuarioAsync(usuarioDto.idUsuario);
                    Usuarios.Remove(usuarioDto);
                }
            }
        }

        // Navegar a la página de creación de usuario
        private async Task NavegarCrearUsuario()
        {
            await Shell.Current.GoToAsync(nameof(UsuarioPage));
        }

        // Navegar a la página de edición de usuario
        private async Task EditarUsuario(UsuarioDTO usuarioDTO)
        {
            Debug.WriteLine($"UsuariosPage?UsuarioId={usuarioDTO.IdUsuario}");
            await Shell.Current.GoToAsync($"UsuariosPage?UsuarioId={usuarioDTO.IdUsuario}");
        }

        // Notificar cambios a la vista
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}