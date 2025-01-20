using CRUD_MVVM.PresentationLayer.ViewModels;

namespace CRUD_MVVM.Views;

public partial class VerPedidosPage : ContentPage
{
	public VerPedidosPage(VerPedidosViewModel pedidosViewModel)
	{
		InitializeComponent();
		BindingContext = pedidosViewModel;
	}
}