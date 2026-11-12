using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudioManager.Models;
using StudioManager.Repositorios;

namespace StudioManager.ViewModels
{
    // Quando a navegação envia um item chamado "Cliente", ele chega em ClienteEditado
    [QueryProperty("ClienteEditado", "Cliente")]
    public partial class CadastroClienteViewModel : ObservableObject
    {
        private readonly IRepositorio<Cliente> _repositorio;

        // Fica nulo no cadastro novo e preenchido quando a tela abre para editar
        [ObservableProperty]
        private Cliente? clienteEditado;

        [ObservableProperty]
        private string titulo = "Novo cliente";

        [ObservableProperty]
        private string nome = "";

        [ObservableProperty]
        private string telefone = "";

        [ObservableProperty]
        private string observacao = "";

        [ObservableProperty]
        private string mensagem = "";

        public CadastroClienteViewModel(IRepositorio<Cliente> repositorio)
        {
            _repositorio = repositorio;
        }

        // Método gerado pelo pacote: roda sempre que ClienteEditado recebe um valor
        partial void OnClienteEditadoChanged(Cliente? value)
        {
            if (value is null)
            {
                return;
            }

            Titulo = "Editar cliente";
            Nome = value.Nome;
            Telefone = value.Telefone;
            Observacao = value.Observacao;
        }

        [RelayCommand]
        private async Task Salvar()
        {
            if (string.IsNullOrWhiteSpace(Nome))
            {
                Mensagem = "O nome do cliente é obrigatório.";
                return;
            }

            if (ClienteEditado is null)
            {
                _repositorio.Adicionar(new Cliente(Nome, Telefone, Observacao));
            }
            else
            {
                ClienteEditado.Atualizar(Nome, Telefone, Observacao);
                _repositorio.Salvar();
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}