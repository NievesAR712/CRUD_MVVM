using System.Diagnostics;
using CRUD_MVVM.PresentationLayer.ViewModels;

namespace CRUD_MVVM.Views;

[QueryProperty(nameof(UsuarioId), "UsuarioId")]
public partial class UsuarioPage : ContentPage
{
    public int UsuarioId { get; set; }

    public UsuarioPage(UsuarioViewModel usuarioViewModel)
    {
        InitializeComponent();
        BindingContext = usuarioViewModel;
        
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();

        // Ahora que la p�gina se est� mostrando, puedes usar UsuarioId
        Debug.WriteLine($"Recibiendo UsuarioId: {UsuarioId}");

        // Llama al m�todo en el ViewModel para cargar los datos
        if (BindingContext is UsuarioViewModel viewModel)
        {
            viewModel.CargarUsuario(UsuarioId);
        }
    }
}
