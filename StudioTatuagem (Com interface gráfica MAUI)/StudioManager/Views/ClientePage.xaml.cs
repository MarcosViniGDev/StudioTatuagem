using StudioManager.ViewModels;

namespace StudioManager.Views;

public partial class ClientesPage : ContentPage
{
    private readonly ClientesViewModel _vm;

    public ClientesPage(ClientesViewModel vm)
    {
        InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.Carregar();
    }
}