using CRUD_MVVM.ViewModels;

namespace CRUD_MVVM.Views;

public partial class PedidoPage : ContentPage
{
	public PedidoPage(PedidoViewModel pedidoViewModel)
	{
		InitializeComponent();
		BindingContext = pedidoViewModel;
	}
}