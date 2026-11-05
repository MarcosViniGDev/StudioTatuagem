using System;
using System.Collections.Generic;
using System.Text;

namespace StudioTatuagem.Models
{
    public class Agendamento
    {
        public int Id { get; set; }
        public Cliente Cliente { get; }
        public DateTime Inicio { get; }
        public int Duracao { get; }
        public string Descricao { get; }
        public decimal Valor { get; }
        public StatusAgendamento Status { get; set;} = StatusAgendamento.Agendado;

        public Agendamento(int id, Cliente cliente, DateTime inicio, int duracao, string descricao, decimal valor)
        {
            Id = id;
            Cliente = cliente;
            Inicio = inicio;
            Duracao = duracao;
            Descricao = descricao;
            Valor = valor;
        }

        public DateTime Fim()
        {
            return Inicio.AddMinutes(Duracao);
        }

        public bool conflitaCom(Agendamento outro)
        {
            return Inicio < outro.Fim() && outro.Inicio < Fim();
        }

        public string resumo()
        {
            return $"[{Id}] | Cliente: {Cliente.Nome} | Início: {Inicio:dd/MM/yyyy HH:mm} | Fim: {Fim():dd/MM/yyyy HH:mm} | Duração: {Duracao} minutos | Descrição: {Descricao} | Valor: R${Valor} | Status: {Status}";
        }


    }
}
