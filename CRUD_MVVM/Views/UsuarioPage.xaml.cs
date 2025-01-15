using CRUD_MVVM.ViewModels;

namespace CRUD_MVVM.Views;

public partial class UsuarioPage : ContentPage
{
	public UsuarioPage(UsuarioViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}