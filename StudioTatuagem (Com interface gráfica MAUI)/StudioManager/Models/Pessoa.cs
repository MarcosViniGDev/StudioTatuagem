using System;
using System.Collections.Generic;
using System.Text;

namespace StudioManager.Models
{
    public abstract class Pessoa : IEntidade
    {
        public int Id { get; set; }
        public string Nome { get; protected set; }
        public string Telefone { get; protected set; }

        public abstract string Resumo();

        protected Pessoa(string nome, string telefone)
        {
            Nome = nome;
            Telefone = telefone;
        }
    }
}
