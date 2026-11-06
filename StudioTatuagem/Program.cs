using StudioTatuagem.Models;
using StudioTatuagem.Repositorios;

RepositorioEmMemoria<Cliente> RepCliente = new RepositorioEmMemoria<Cliente>();

RepCliente.Adicionar(new Cliente("Marco", "123", "DSADS"));
RepCliente.Adicionar(new Cliente("Maria", "456", "DSADS"));
RepCliente.Adicionar(new Cliente("José", "789", "DSADS"));

foreach (Cliente c in RepCliente.ListarTodos()) 
{
    Console.WriteLine(c.Resumo());
}
Console.WriteLine("\n=== Teste 2: buscar por Id ===");
var achado = RepCliente.BuscarPorId(2);
Console.WriteLine(achado != null ? achado.Resumo() : "Id 2 não encontrado");

var inexistente = RepCliente.BuscarPorId(99);
Console.WriteLine(inexistente != null ? inexistente.Resumo() : "Id 99 não encontrado");



Console.WriteLine("\n=== Teste 3: crescimento do vetor ===");
var repoGrande = new RepositorioEmMemoria<Cliente>();

for (int i = 1; i <= 12; i++)
{
    repoGrande.Adicionar(new Cliente($"Cliente {i}", "000", "-"));
}

var todos = repoGrande.ListarTodos();
Console.WriteLine($"Total guardado: {todos.Length}");

for(int i = 0; i < todos.Length; i++)
{
    Console.WriteLine(todos[i].Resumo());
}


