using StudioManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace StudioManager.Repositorios
{
    public interface IRepositorio<T> where T : class, IEntidade
    {
        public T Adicionar(T item);
        public T? BuscarPorId(int id);
        public T[] ListarTodos();

        public void Salvar();

        public bool Remover(int id);

    }
}
