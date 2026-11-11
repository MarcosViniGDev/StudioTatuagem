using System.Collections.ObjectModel;
using StudioManager.Models;
using StudioManager.Repositorios;

namespace StudioManager.ViewModels;

public class ClientesViewModel
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
}