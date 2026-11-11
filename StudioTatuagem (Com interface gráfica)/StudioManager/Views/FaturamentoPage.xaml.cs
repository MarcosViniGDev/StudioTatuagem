using StudioManager.ViewModels;

namespace StudioManager.Views;

public partial class FaturamentoPage : ContentPage
{
    private readonly FaturamentoViewModel _vm;
    public FaturamentoPage(FaturamentoViewModel vm)
	{
		InitializeComponent();
        _vm = vm;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _vm.CalcularCommand.Execute(null);
    }
}