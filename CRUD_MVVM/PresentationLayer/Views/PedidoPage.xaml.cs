using CRUD_MVVM.PresentationLayer.ViewModels;

namespace CRUD_MVVM.Views;

public partial class PedidoPage : ContentPage
{
	public PedidoPage(PedidoViewModel pedidoViewModel)
	{
		InitializeComponent();
		BindingContext = pedidoViewModel;
	}
}