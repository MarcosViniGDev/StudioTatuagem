using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudioManager.Models;
using StudioManager.Services;
using System.Collections.ObjectModel;

namespace StudioManager.ViewModels
{
    public partial class AgendaViewModel : ObservableObject
    {
        private readonly Agenda _agenda;

        [ObservableProperty]
        private Agendamento? agendamentoSelecionado;
        string mensagem = "";

        public ObservableCollection<Agendamento> Agendamentos { get; } = new();

        public AgendaViewModel(Agenda agenda)
        {
            _agenda = agenda;
        }

        public void Carregar()
        {
            Agendamentos.Clear();
            foreach (var agendamento in _agenda.ListarOrdenado())
            {
                Agendamentos.Add(agendamento);
            }
        }

        [RelayCommand]
        private async Task NovaSessao()
        {
            try
            {
                await Shell.Current.GoToAsync("AgendarSessao");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Erro", ex.ToString(), "OK");
            }
        }

        [RelayCommand]
        private void Concluir()
        {
            if (AgendamentoSelecionado is null)
            {
                mensagem = "Selecione uma sessão na lista.";
                return;
            }

            if (_agenda.Concluir(AgendamentoSelecionado.Id))
            {
                mensagem = "Sessão concluída.";
            }
            else
            {
                mensagem = "Não foi possível concluir (já concluída ou cancelada).";
            }

            Carregar();
        }

        [RelayCommand]
        private async Task Cancelar() 
        {
            if (agendamentoSelecionado == null)
            {
                mensagem = "Selecione uma sessão para cancelar.";
                return;
            }
            bool confirmou = await Shell.Current.DisplayAlert(
                "Cancelar sessão",
                $"Cancelar a sessão de {agendamentoSelecionado.Cliente.Nome}?",
                "Sim", "Não");

            if (!confirmou) return;
            else 
            {
                _agenda.Cancelar(agendamentoSelecionado.Id);
                mensagem = $"Sessão do cliente {agendamentoSelecionado.Cliente.Nome} cancelada com sucesso.";
                
            }
            Carregar();

        }

    }
}