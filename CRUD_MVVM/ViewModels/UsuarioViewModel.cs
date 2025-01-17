using CRUD_MVVM.DataAccess;
using CRUD_MVVM.DTOs;
using CRUD_MVVM.Modelos;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;

namespace CRUD_MVVM.ViewModels
{
    public class UsuarioViewModel : INotifyPropertyChanged
    {
        private readonly UsuarioRepository _usuarioRepository;
        private MainViewModel _mainViewModel;
        private UsuarioDTO _usuarioDto;
        private bool _isLoading;

        public UsuarioDTO UsuarioDto
        {
            get => _usuarioDto;
            set
            {
                if (_usuarioDto != value)
                {
                    _usuarioDto = value;
                    OnPropertyChanged(nameof(UsuarioDto));
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

        public ICommand GuardarCommand { get; }
        public ICommand CargarUsuarioCommand { get; }

        public UsuarioViewModel(UsuarioRepository usuarioRepository, MainViewModel mainViewModel)
        {
            _usuarioRepository = usuarioRepository;
            UsuarioDto = new UsuarioDTO();
            GuardarCommand = new Command(async () => await Guardar());
            CargarUsuarioCommand = new Command<int>(async (id) => await CargarUsuario(id));
            _mainViewModel = mainViewModel;
        }

        public async Task Guardar()
        {
            IsLoading = true;

            Usuario usuario = new Usuario
            {
                IdUsuario = UsuarioDto.IdUsuario,
                NombreCompleto = UsuarioDto.NombreCompleto,
                Correo = UsuarioDto.Correo,
                Password = UsuarioDto.Password,
                FechaRegistro = UsuarioDto.FechaRegistro
            };

            if (UsuarioDto.IdUsuario == 0)
            {
                await _usuarioRepository.AddUsuarioAsync(usuario);
                UsuarioDto.IdUsuario = usuario.IdUsuario;
                _mainViewModel.AgregarUsuarioALista(UsuarioDto, true); // Agregar el nuevo usuario
            }
            else
            {
                await _usuarioRepository.UpdateUsuarioAsync(usuario);
                _mainViewModel.AgregarUsuarioALista(UsuarioDto, false); // Actualizar el usuario existente
            }

            IsLoading = false;
            await Shell.Current.GoToAsync("//MainPage");
        }
        public async Task CargarUsuario(int id)
        {
            if (id <= 0) return;

            IsLoading = true;

            var usuario = await _usuarioRepository.GetUsuarioByIdAsync(id);
            if (usuario != null)
            {
                UsuarioDto = new UsuarioDTO
                {
                    IdUsuario = usuario.IdUsuario,
                    NombreCompleto = usuario.NombreCompleto,
                    Correo = usuario.Correo,
                    Password = usuario.Password,
                    FechaRegistro = usuario.FechaRegistro
                };
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
