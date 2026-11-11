using System;
using System.Collections.Generic;
using System.Text;
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
            Console.WriteLine("Caracteres lidos: " + json.Length);

            T[]? itens = JsonSerializer.Deserialize<T[]>(json);
            Console.WriteLine("Itens desserializados: " + (itens?.Length ?? -1));

            if (itens is null)
            {
                return;
            }

            foreach (var item in itens)
            {
                _repositorio.Adicionar(item);
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
