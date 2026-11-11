using System;
using System.Collections.Generic;
using System.Text;

namespace StudioTatuagem.Models
{
    public class Cliente : Pessoa
    {
        public string Observacao { get; private set;}
        public Cliente(string nome, string telefone, string observacao) : base(nome, telefone)
        {
            Observacao = observacao;
        }


        public override string Resumo()
        {
            return $"[{Id}] | Nome: {Nome} | Telefone: {Telefone} | Observação: {Observacao}";
        }
    }
}
