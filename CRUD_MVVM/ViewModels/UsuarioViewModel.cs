using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Input;
using CRUD_MVVM.DataAccess;
using CRUD_MVVM.DTOs;
using CRUD_MVVM.Utilidades;
using CRUD_MVVM.Modelos;
using static CRUD_MVVM.DataAccess.UsuarioRepository;

namespace CRUD_MVVM.ViewModels
{
    public partial class UsuarioViewModel : ObservableObject, IQueryAttributable
    {
        private readonly UsuarioRepository _usuarioRepository;

        [ObservableProperty]
        private UsuarioDTO usuarioDto = new UsuarioDTO();

        [ObservableProperty]
        private string tituloPagina;

        private int idUsuario;

        [ObservableProperty]
        private bool loadingEsVisible = false;

        public UsuarioViewModel(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            UsuarioDto.FechaRegistro = DateTime.Now;
        }

        public async void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            var id = int.Parse(query["id"].ToString());
            idUsuario = id;

            if (idUsuario == 0)
            {
                TituloPagina = "Nuevo Usuario";
            }
            else
            {
                TituloPagina = "Editar Usuario";
                LoadingEsVisible = true;
                await Task.Run(async () =>
                {
                    var encontrado = await _usuarioRepository.GetUsuarioByIdAsync(idUsuario);
                    UsuarioDto.IdUsuario = encontrado.IdUsuario;
                    UsuarioDto.NombreCompleto = encontrado.NombreCompleto;
                    UsuarioDto.Correo = encontrado.Correo;
                    UsuarioDto.Password = encontrado.Password;
                    UsuarioDto.FechaRegistro = encontrado.FechaRegistro;

                    MainThread.BeginInvokeOnMainThread(() => { LoadingEsVisible = false; });
                });
            }
        }

        [RelayCommand]
        private async Task Guardar()
        {
            LoadingEsVisible = true;
            UsuarioMensaje mensaje = new UsuarioMensaje();

            await Task.Run(async () =>
            {
                if (idUsuario == 0)
                {
                    var tbUsuario = new Usuario
                    {
                        NombreCompleto = UsuarioDto.NombreCompleto,
                        Correo = UsuarioDto.Correo,
                        Password = UsuarioDto.Password,
                        FechaRegistro = UsuarioDto.FechaRegistro,
                    };

                    await _usuarioRepository.AddUsuarioAsync(tbUsuario);

                    UsuarioDto.IdUsuario = tbUsuario.IdUsuario;
                    mensaje = new UsuarioMensaje
                    {
                        EsCrear = true,
                        UsuarioDto = UsuarioDto
                    };
                }
                else
                {
                    var encontrado = await _usuarioRepository.GetUsuarioByIdAsync(idUsuario);
                    encontrado.NombreCompleto = UsuarioDto.NombreCompleto;
                    encontrado.Correo = UsuarioDto.Correo;
                    encontrado.Password = UsuarioDto.Password;
                    encontrado.FechaRegistro = UsuarioDto.FechaRegistro;

                    await _usuarioRepository.UpdateUsuarioAsync(encontrado);

                    mensaje = new UsuarioMensaje
                    {
                        EsCrear = false,
                        UsuarioDto = UsuarioDto
                    };
                }

                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    LoadingEsVisible = false;
                    WeakReferenceMessenger.Default.Send(new UsuarioMensajeria(mensaje));
                    await Shell.Current.Navigation.PopAsync();
                });
            });
        }
    }
}
