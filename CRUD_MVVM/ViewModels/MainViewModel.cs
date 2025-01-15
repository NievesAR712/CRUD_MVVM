using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using CRUD_MVVM.DataAccess;
using CRUD_MVVM.DTOs;
using CRUD_MVVM.Utilidades;
using CRUD_MVVM.Modelos;
using System.Collections.ObjectModel;
using CRUD_MVVM.Views;
using static CRUD_MVVM.DataAccess.UsuarioRepository;

namespace CRUD_MVVM.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly UsuarioRepository _usuarioRepository;

        [ObservableProperty]
        private ObservableCollection<UsuarioDTO> listaUsuario = new ObservableCollection<UsuarioDTO>();

        public MainViewModel(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;

            MainThread.BeginInvokeOnMainThread(new Action(async () => await Obtener()));

            WeakReferenceMessenger.Default.Register<UsuarioMensajeria>(this, (r, m) =>
            {
                UsuarioMensajeRecibido(m.Value);
            });
        }

        public async Task Obtener()
        {
            var lista = await _usuarioRepository.GetUsuariosAsync();
            if (lista.Any())
            {
                foreach (var item in lista)
                {
                    ListaUsuario.Add(new UsuarioDTO
                    {
                        IdUsuario = item.IdUsuario,
                        NombreCompleto = item.NombreCompleto,
                        Correo = item.Correo,
                        Password = item.Password,
                        FechaRegistro = item.FechaRegistro,
                    });
                }
            }
        }

        private void UsuarioMensajeRecibido(UsuarioMensaje usuarioMensaje)
        {
            var usuarioDto = usuarioMensaje.UsuarioDto;

            if (usuarioMensaje.EsCrear)
            {
                ListaUsuario.Add(usuarioDto);
            }
            else
            {
                var encontrado = ListaUsuario
                    .First(e => e.IdUsuario == usuarioDto.IdUsuario);
                encontrado.NombreCompleto = usuarioDto.NombreCompleto;
                encontrado.Correo = usuarioDto.Correo;
                encontrado.Password = usuarioDto.Password;
                encontrado.FechaRegistro = usuarioDto.FechaRegistro;
            }
        }

        [RelayCommand]
        private async Task Crear()
        {
            var uri = $"{nameof(UsuarioPage)}?id=0";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Editar(UsuarioDTO usuarioDTO)
        {
            var uri = $"{nameof(UsuarioPage)}?id={usuarioDTO.idUsuario}";
            await Shell.Current.GoToAsync(uri);
        }

        [RelayCommand]
        private async Task Eliminar(UsuarioDTO usuarioDto)
        {
            bool answer = await Shell.Current.DisplayAlert("Mensaje", "Desea eliminar el Usuario?", "Si", "NO");

            if (answer)
            {
                var usuario = await _usuarioRepository.GetUsuarioByIdAsync(usuarioDto.idUsuario);
                if (usuario != null)
                {
                    await _usuarioRepository.DeleteUsuarioAsync(usuarioDto.idUsuario);
                    ListaUsuario.Remove(usuarioDto);
                }
            }
        }
    }
}
