using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudioManager.Models;
using StudioManager.Repositorios;
using StudioManager.Services;

namespace StudioManager.ViewModels;

public partial class ClientesViewModel : ObservableObject
{
    private readonly IRepositorio<Cliente> _repositorio;
    private readonly Agenda _agenda;

    public ObservableCollection<Cliente> Clientes { get; } = new();

    // Linha clicada na lista
    [ObservableProperty]
    private Cliente? clienteSelecionado;

    [ObservableProperty]
    private string mensagem = "";

    public ClientesViewModel(IRepositorio<Cliente> repositorio, Agenda agenda)
    {
        _repositorio = repositorio;
        _agenda = agenda;
    }

    public void Carregar()
    {
        Clientes.Clear();
        foreach (var cliente in _repositorio.ListarTodos())
        {
            Clientes.Add(cliente);
        }
    }

    [RelayCommand]
    private async Task NovoCliente()
    {
        Mensagem = "";
        await Shell.Current.GoToAsync("CadastroCliente");
    }

    [RelayCommand]
    private async Task Editar()
    {
        if (ClienteSelecionado is null)
        {
            Mensagem = "Selecione um cliente na lista.";
            return;
        }

        Mensagem = "";

        // Envia o cliente inteiro para a tela de cadastro, que abre em modo de edição
        await Shell.Current.GoToAsync("CadastroCliente", new Dictionary<string, object>
        {
            ["Cliente"] = ClienteSelecionado
        });
    }

    [RelayCommand]
    private async Task Excluir()
    {
        if (ClienteSelecionado is null)
        {
            Mensagem = "Selecione um cliente na lista.";
            return;
        }

        // Regra: cliente com histórico de sessões não pode ser excluído
        if (_agenda.ClienteTemAgendamentos(ClienteSelecionado.Id))
        {
            Mensagem = "Não é possível excluir: o cliente possui agendamentos.";
            return;
        }

        bool confirmou = await Shell.Current.DisplayAlert(
            "Excluir cliente",
            $"Excluir {ClienteSelecionado.Nome}? Esta ação não pode ser desfeita.",
            "Excluir", "Cancelar");

        if (!confirmou)
        {
            return;
        }

        _repositorio.Remover(ClienteSelecionado.Id);
        Mensagem = "Cliente excluído.";
        Carregar();
    }
}
