using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Storage;
using StudioManager.Models;
using StudioManager.Services;

namespace StudioManager.ViewModels
{
    public partial class AgendaViewModel : ObservableObject
    {
        private readonly Agenda _agenda;
        private readonly ArmazenamentoImagens _imagens;

        public ObservableCollection<Agendamento> Agendamentos { get; } = new();

        // Linha clicada na lista
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TemSelecao))]
        [NotifyPropertyChangedFor(nameof(SemSelecao))]
        private Agendamento? agendamentoSelecionado;

        [ObservableProperty]
        private string mensagem = "";

        // Imagem da sessão selecionada (nula quando não há)
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TemImagem))]
        [NotifyPropertyChangedFor(nameof(SemImagem))]
        private ImageSource? imagem;

        // Propriedades calculadas que controlam o que aparece no painel de detalhes
        public bool TemSelecao => AgendamentoSelecionado is not null;
        public bool SemSelecao => AgendamentoSelecionado is null;
        public bool TemImagem => Imagem is not null;
        public bool SemImagem => Imagem is null;

        public AgendaViewModel(Agenda agenda, ArmazenamentoImagens imagens)
        {
            _agenda = agenda;
            _imagens = imagens;
        }

        public void Carregar()
        {
            Agendamentos.Clear();
            foreach (var agendamento in _agenda.ListarOrdenado())
            {
                Agendamentos.Add(agendamento);
            }
        }

        // Método gerado pelo pacote: roda sempre que a seleção muda
        partial void OnAgendamentoSelecionadoChanged(Agendamento? value)
        {
            AtualizarImagem();
        }

        private void AtualizarImagem()
        {
            Imagem = null;

            string? arquivo = AgendamentoSelecionado?.ImagemArquivo;
            if (arquivo is null || !_imagens.Existe(arquivo))
            {
                return;
            }

            // Lê o arquivo para a memória: assim nenhum arquivo fica travado no disco
            byte[] bytes = File.ReadAllBytes(_imagens.CaminhoCompleto(arquivo));
            Imagem = ImageSource.FromStream(() => new MemoryStream(bytes));
        }

        [RelayCommand]
        private async Task NovaSessao()
        {
            await Shell.Current.GoToAsync("AgendarSessao");
        }

        [RelayCommand]
        private async Task Editar()
        {
            if (AgendamentoSelecionado is null)
            {
                Mensagem = "Selecione uma sessão na lista.";
                return;
            }

            if (AgendamentoSelecionado.Status != StatusAgendamento.Agendado)
            {
                Mensagem = "Só sessões agendadas podem ser editadas.";
                return;
            }

            Mensagem = "";

            await Shell.Current.GoToAsync("AgendarSessao", new Dictionary<string, object>
            {
                ["Agendamento"] = AgendamentoSelecionado
            });
        }

        [RelayCommand]
        private void Concluir()
        {
            if (AgendamentoSelecionado is null)
            {
                Mensagem = "Selecione uma sessão na lista.";
                return;
            }

            if (_agenda.Concluir(AgendamentoSelecionado.Id))
            {
                Mensagem = "Sessão concluída.";
            }
            else
            {
                Mensagem = "Não foi possível concluir (já concluída ou cancelada).";
            }

            Carregar();
        }

        [RelayCommand]
        private async Task Cancelar()
        {
            if (AgendamentoSelecionado is null)
            {
                Mensagem = "Selecione uma sessão na lista.";
                return;
            }

            bool confirmou = await Shell.Current.DisplayAlert(
                "Cancelar sessão",
                $"Cancelar a sessão de {AgendamentoSelecionado.Cliente?.Nome}?",
                "Sim", "Não");

            if (!confirmou)
            {
                return;
            }

            if (_agenda.Cancelar(AgendamentoSelecionado.Id))
            {
                Mensagem = "Sessão cancelada.";
            }
            else
            {
                Mensagem = "Não foi possível cancelar (já concluída ou cancelada).";
            }

            Carregar();
        }

        [RelayCommand]
        private async Task AnexarImagem()
        {
            if (AgendamentoSelecionado is null)
            {
                Mensagem = "Selecione uma sessão na lista.";
                return;
            }

            var escolhido = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Escolha a imagem da tatuagem",
                FileTypes = FilePickerFileType.Images
            });

            if (escolhido is null)
            {
                return; // a pessoa cancelou a escolha
            }

            string? arquivoAntigo = AgendamentoSelecionado.ImagemArquivo;

            // Copia a imagem para a pasta do app
            string novoNome;
            using (var origem = await escolhido.OpenReadAsync())
            {
                novoNome = await _imagens.SalvarAsync(origem, escolhido.FileName);
            }

            // Registra o nome na sessão e grava no arquivo de dados
            if (!_agenda.DefinirImagem(AgendamentoSelecionado.Id, novoNome))
            {
                _imagens.Excluir(novoNome);
                Mensagem = "Não foi possível anexar a imagem.";
                return;
            }

            // Se havia uma imagem anterior, ela não é mais necessária
            if (arquivoAntigo is not null)
            {
                _imagens.Excluir(arquivoAntigo);
            }

            AtualizarImagem();
            Mensagem = "Imagem anexada.";
        }

        [RelayCommand]
        private async Task RemoverImagem()
        {
            if (AgendamentoSelecionado is null)
            {
                Mensagem = "Selecione uma sessão na lista.";
                return;
            }

            string? arquivo = AgendamentoSelecionado.ImagemArquivo;
            if (arquivo is null)
            {
                Mensagem = "Esta sessão não tem imagem.";
                return;
            }

            bool confirmou = await Shell.Current.DisplayAlert(
                "Remover imagem",
                "Remover a imagem desta sessão?",
                "Remover", "Cancelar");

            if (!confirmou)
            {
                return;
            }

            _agenda.DefinirImagem(AgendamentoSelecionado.Id, null);
            Imagem = null;
            _imagens.Excluir(arquivo);
            Mensagem = "Imagem removida.";
        }
    }
}
