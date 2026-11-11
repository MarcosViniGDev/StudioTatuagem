using StudioManager.ViewModels;

namespace StudioManager.Views;

public partial class AgendarSessaoPage : ContentPage
{
    private readonly AgendarSessaoViewModel _vm;

    public AgendarSessaoPage(AgendarSessaoViewModel vm)
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