using StudioManager.ViewModels;

namespace StudioManager.Views;

public partial class AgendaPage : ContentPage
{
    private readonly AgendaViewModel _vm;

    public AgendaPage(AgendaViewModel vm)
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