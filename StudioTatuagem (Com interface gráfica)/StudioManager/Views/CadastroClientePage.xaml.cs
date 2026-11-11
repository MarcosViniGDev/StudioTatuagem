using StudioManager.ViewModels;

namespace StudioManager.Views;

public partial class CadastroClientePage : ContentPage
{
    public CadastroClientePage(CadastroClienteViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}