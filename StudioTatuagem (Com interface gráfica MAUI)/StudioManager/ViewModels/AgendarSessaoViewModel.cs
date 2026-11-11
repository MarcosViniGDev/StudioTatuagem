using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudioManager.Models;
using StudioManager.Repositorios;
using StudioManager.Services;

namespace StudioManager.ViewModels
{
    public partial class AgendarSessaoViewModel : ObservableObject
    {
        private readonly Agenda _agenda;
        private readonly IRepositorio<Cliente> _repositorio;
        public ObservableCollection<Cliente> Clientes { get; } = new();
        public AgendarSessaoViewModel(Agenda agenda, IRepositorio<Cliente> repositorio)
        {
            _agenda = agenda;
            _repositorio = repositorio;
        }

        [ObservableProperty]
        Cliente? clienteSelecionado;

        [ObservableProperty]
        DateTime data = DateTime.Today;

        [ObservableProperty]
        TimeSpan hora = new TimeSpan(9, 0, 0);

        [ObservableProperty]
        string duracao = "";

        [ObservableProperty]
        string descricao = "";

        [ObservableProperty]
        string valor = "";

        [ObservableProperty]
        string mensagem = "";

        public void Carregar()
        {
            Clientes.Clear();
            foreach (var cliente in _repositorio.ListarTodos())
            {
                Clientes.Add(cliente);
            }
        }

        [RelayCommand]
        private async Task Agendar()
        {
            if (clienteSelecionado == null)
            {
                mensagem = "Selecione um cliente para continuar.";
                return;
            }
            if(!int.TryParse(duracao, out int duracaoInt) || duracaoInt <= 0)
            {
                mensagem = "Informe uma duração válida.";
                return;
            }

            var pt = new CultureInfo("pt-BR");
            if (!decimal.TryParse(valor, NumberStyles.Currency, pt, out decimal valorDecimal) || valorDecimal <= 0)
            {
                mensagem = "Informe um valor válido.";
                return;
            }
            DateTime inicio = data.Date + hora;
            var agendamento =_agenda.Agendar(clienteSelecionado, inicio, duracaoInt, descricao, valorDecimal);

            if (agendamento is null)
            {
                mensagem = "Conflito de horário: já existe uma sessão nesse período.";
                return;
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}
