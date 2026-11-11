using StudioManager.Views;

namespace StudioManager
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("CadastroCliente", typeof(CadastroClientePage));
            Routing.RegisterRoute("AgendarSessao", typeof(AgendarSessaoPage));

        }
    }
}
