using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudioManager.Models;
using StudioManager.Repositorios;
using StudioManager.Services;

namespace StudioManager.ViewModels
{
    // Quando a navegação envia um item chamado "Agendamento", ele chega em AgendamentoEditado
    [QueryProperty("AgendamentoEditado", "Agendamento")]
    public partial class AgendarSessaoViewModel : ObservableObject
    {
        private readonly Agenda _agenda;
        private readonly IRepositorio<Cliente> _repositorioClientes;

        public ObservableCollection<Cliente> Clientes { get; } = new();

        // Fica nulo em uma sessão nova e preenchido quando a tela abre para editar
        [ObservableProperty]
        private Agendamento? agendamentoEditado;

        [ObservableProperty]
        private string titulo = "Nova sessão";

        [ObservableProperty]
        private string textoBotao = "Agendar";

        [ObservableProperty]
        private Cliente? clienteSelecionado;

        [ObservableProperty]
        private DateTime data = DateTime.Today;

        [ObservableProperty]
        private TimeSpan hora = new TimeSpan(9, 0, 0);

        [ObservableProperty]
        private string duracao = "";

        [ObservableProperty]
        private string descricao = "";

        [ObservableProperty]
        private string valor = "";

        [ObservableProperty]
        private string mensagem = "";

        public AgendarSessaoViewModel(Agenda agenda, IRepositorio<Cliente> repositorioClientes)
        {
            _agenda = agenda;
            _repositorioClientes = repositorioClientes;
        }

        // Método gerado pelo pacote: roda sempre que AgendamentoEditado recebe um valor
        partial void OnAgendamentoEditadoChanged(Agendamento? value)
        {
            if (value is null)
            {
                return;
            }

            Titulo = "Editar sessão";
            TextoBotao = "Salvar alterações";
            Data = value.Inicio.Date;
            Hora = value.Inicio.TimeOfDay;
            Duracao = value.DuracaoMinutos.ToString();
            Descricao = value.Descricao;
            Valor = value.Valor.ToString("F2", new CultureInfo("pt-BR"));

            SelecionarClienteDaSessao();
        }

        // A lista suspensa precisa receber o MESMO objeto que está na coleção Clientes
        private void SelecionarClienteDaSessao()
        {
            if (AgendamentoEditado is null)
            {
                return;
            }

            ClienteSelecionado = Clientes.FirstOrDefault(c => c.Id == AgendamentoEditado.ClienteId);
        }

        public void Carregar()
        {
            Clientes.Clear();
            foreach (var cliente in _repositorioClientes.ListarTodos())
            {
                Clientes.Add(cliente);
            }

            // Limpar a coleção desfaz a seleção, então ela é refeita aqui
            SelecionarClienteDaSessao();
        }

        [RelayCommand]
        private async Task Salvar()
        {
            if (ClienteSelecionado is null)
            {
                Mensagem = "Selecione um cliente para continuar.";
                return;
            }

            if (!int.TryParse(Duracao, out int duracaoInt) || duracaoInt <= 0)
            {
                Mensagem = "Informe uma duração válida.";
                return;
            }

            var pt = new CultureInfo("pt-BR");
            if (!decimal.TryParse(Valor, NumberStyles.Currency, pt, out decimal valorDecimal) || valorDecimal <= 0)
            {
                Mensagem = "Informe um valor válido.";
                return;
            }

            DateTime inicio = Data.Date + Hora;

            if (AgendamentoEditado is null)
            {
                // Sessão nova
                var agendamento = _agenda.Agendar(ClienteSelecionado, inicio, duracaoInt, Descricao, valorDecimal);

                if (agendamento is null)
                {
                    Mensagem = "Conflito de horário: já existe uma sessão nesse período.";
                    return;
                }
            }
            else
            {
                // Edição de uma sessão existente
                var resultado = _agenda.Editar(AgendamentoEditado.Id, ClienteSelecionado, inicio, duracaoInt, Descricao, valorDecimal);

                switch (resultado)
                {
                    case ResultadoEdicao.Conflito:
                        Mensagem = "Conflito de horário: já existe uma sessão nesse período.";
                        return;
                    case ResultadoEdicao.StatusInvalido:
                        Mensagem = "Só sessões agendadas podem ser editadas.";
                        return;
                    case ResultadoEdicao.NaoEncontrado:
                        Mensagem = "Sessão não encontrada.";
                        return;
                }
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}
