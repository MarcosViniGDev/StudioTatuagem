using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudioManager.Services;

namespace StudioManager.ViewModels
{
    public partial class FaturamentoViewModel : ObservableObject
    {
        private readonly Agenda _agenda;

        [ObservableProperty]
        private string ano = DateTime.Today.Year.ToString();

        [ObservableProperty]
        private string mes = DateTime.Today.Month.ToString();

        [ObservableProperty]
        private string total = "";

        [ObservableProperty]
        private string mensagem = "";

        public FaturamentoViewModel(Agenda agenda)
        {
            _agenda = agenda;
        }

        [RelayCommand]
        private void Calcular()
        {
            if (!int.TryParse(Ano, out int anoInt) || anoInt <= 0)
            {
                Mensagem = "Ano inválido.";
                return;
            }

            if (!int.TryParse(Mes, out int mesInt) || mesInt < 1 || mesInt > 12)
            {
                Mensagem = "Mês inválido (use de 1 a 12).";
                return;
            }

            Mensagem = "";

            var pt = new CultureInfo("pt-BR");
            Total = _agenda.FaturamentoMes(anoInt, mesInt).ToString("C", pt);
        }
    }
}