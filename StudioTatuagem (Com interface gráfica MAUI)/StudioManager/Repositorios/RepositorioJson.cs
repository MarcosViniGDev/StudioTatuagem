using System.Text.Json;
using StudioManager.Models;

namespace StudioManager.Repositorios
{
    public class RepositorioJson<T> : IRepositorio<T> where T : class, IEntidade
    {
        private readonly RepositorioEmMemoria<T> _repositorio = new RepositorioEmMemoria<T>();
        private readonly string _caminho;

        public RepositorioJson(string caminho)
        {
            _caminho = caminho;
            Carregar();
        }

        private void Carregar()
        {
            if (!File.Exists(_caminho))
            {
                return;
            }

            string json = File.ReadAllText(_caminho);
            T[]? itens = JsonSerializer.Deserialize<T[]>(json);

            if (itens is null)
            {
                return;
            }

            foreach (var item in itens)
            {
                _repositorio.Restaurar(item);
            }
        }

        public void Salvar()
        {
            T[] itens = _repositorio.ListarTodos();
            string json = JsonSerializer.Serialize(itens, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_caminho, json);
        }

        public T Adicionar(T item)
        {
            T adicionado = _repositorio.Adicionar(item);
            Salvar();
            return adicionado;
        }

        public bool Remover(int id)
        {
            bool removido = _repositorio.Remover(id);

            if (removido)
            {
                Salvar();
            }

            return removido;
        }

        public T? BuscarPorId(int id)
        {
            return _repositorio.BuscarPorId(id);
        }

        public T[] ListarTodos()
        {
            return _repositorio.ListarTodos();
        }
    }
}