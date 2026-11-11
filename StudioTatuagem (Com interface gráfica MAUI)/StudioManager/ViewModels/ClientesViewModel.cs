using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using StudioManager.Models;
using StudioManager.Repositorios;

namespace StudioManager.ViewModels;

public partial class ClientesViewModel : ObservableObject
{
    private readonly IRepositorio<Cliente> _repositorio;

    public ObservableCollection<Cliente> Clientes { get; } = new();

    public ClientesViewModel(IRepositorio<Cliente> repositorio)
    {
        _repositorio = repositorio;
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
        await Shell.Current.GoToAsync("CadastroCliente");
    }   
}