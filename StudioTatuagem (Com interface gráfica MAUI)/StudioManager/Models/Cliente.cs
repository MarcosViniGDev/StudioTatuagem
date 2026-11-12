using System;
using System.Collections.Generic;
using System.Text;

namespace StudioManager.Models
{
    public class Cliente : Pessoa
    {
        public string Observacao { get; private set;}
        public Cliente(string nome, string telefone, string observacao) : base(nome, telefone)
        {
            Observacao = observacao;
        }
        public void Atualizar(string nome, string telefone, string observacao)
        {
            Nome = nome;
            Telefone = telefone;
            Observacao = observacao;
        }

        public override string Resumo()
        {
            return $"[{Id}] | Nome: {Nome} | Telefone: {Telefone} | Observação: {Observacao}";
        }
    }
}
