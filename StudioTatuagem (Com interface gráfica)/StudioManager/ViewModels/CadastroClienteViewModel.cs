using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudioManager.Models;
using StudioManager.Repositorios;

namespace StudioManager.ViewModels
{
    public partial class CadastroClienteViewModel : ObservableObject    
    {
        private readonly IRepositorio<Cliente> _repositorio;

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

        [RelayCommand]
        private async Task Salvar()
        {
            if (string.IsNullOrWhiteSpace(Nome))
            {
                Mensagem = "O nome do cliente é obrigatório.";
                return;
            }

            _repositorio.Adicionar(new Cliente(Nome, Telefone, Observacao));

            await Shell.Current.GoToAsync("..");
        }
    }
}
