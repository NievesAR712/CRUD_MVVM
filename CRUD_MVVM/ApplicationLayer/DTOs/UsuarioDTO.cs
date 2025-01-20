using CommunityToolkit.Mvvm.ComponentModel;

namespace CRUD_MVVM.DTOs
{
    public partial class UsuarioDTO : ObservableObject
    {
        [ObservableProperty]
        public int idUsuario;
        [ObservableProperty]
        public string nombreCompleto;
        [ObservableProperty]
        public string correo;
        [ObservableProperty]
        public string password;
        [ObservableProperty]
        public DateTime fechaRegistro;
    }
}
