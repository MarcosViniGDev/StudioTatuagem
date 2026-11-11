using StudioTatuagem.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace StudioTatuagem.Repositorios
{
    public class RepositorioEmMemoria<T> : IRepositorio<T> where T : class, IEntidade
    {
        private T[] _itens = new T[10];
        private int _quantidadeItens = 0;
        private int _proximoId = 1;

        public T Adicionar(T item)
        {
            if(_quantidadeItens >= _itens.Length)
            {
                Array.Resize(ref _itens, _itens.Length * 2);
            }
            
            item.Id = _proximoId++;
            _itens[_quantidadeItens++] = item;
            return item;
        }

        public T? BuscarPorId(int id)
        {
            for (int i = 0; i < _quantidadeItens; i++) 
            {
                if (id == _itens[i].Id) 
                {
                    return _itens[i];
                }
            }

            return default;
        }

        public T[] ListarTodos()
        {
            T[] copy = new T[_quantidadeItens];
            Array.Copy(_itens, copy, _quantidadeItens);

            return copy;
        }    

        public void Salvar(){ }
    }
}
