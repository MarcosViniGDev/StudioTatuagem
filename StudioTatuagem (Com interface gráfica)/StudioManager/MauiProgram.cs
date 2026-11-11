using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;
using StudioManager.Models;
using StudioManager.Repositorios;
using StudioManager.Services;
using StudioManager.ViewModels;
using StudioManager.Views;

namespace StudioManager
{
    public static class MauiProgram
    {

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<IRepositorio<Cliente>>(_ =>
                new RepositorioJson<Cliente>(Path.Combine(FileSystem.AppDataDirectory, "clientes.json")));

            builder.Services.AddSingleton<IRepositorio<Agendamento>>(_ =>
                new RepositorioJson<Agendamento>(Path.Combine(FileSystem.AppDataDirectory, "agendamentos.json")));

            builder.Services.AddSingleton<Agenda>();

            builder.Services.AddSingleton<ClientesViewModel>();
            builder.Services.AddTransient<ClientesPage>();
            builder.Services.AddTransient<CadastroClienteViewModel>();
            builder.Services.AddTransient<CadastroClientePage>();

            return builder.Build();
        }
    }
}