using System;
using System.Collections.Generic;
using System.Text;
using StudioTatuagem.Models;
using StudioTatuagem.Repositorios;

namespace StudioTatuagem.Services
{
    public class Agenda
    {
        private readonly IRepositorio<Agendamento> _repositorioAgendamento;
        
        public Agenda(IRepositorio<Agendamento> repositorioAgendamento)
        {
            _repositorioAgendamento = repositorioAgendamento;
        }

        public Agendamento? Agendar(Cliente cliente, DateTime inicio, int duracao, string descricao, decimal valor)
        {
            Agendamento agend = new Agendamento(cliente, inicio, duracao, descricao, valor);
            foreach (var existente in _repositorioAgendamento.ListarTodos())
            {
                if (existente.Status == StatusAgendamento.Agendado && agend.ConflitaCom(existente))
                {
                    return null;
                }
            }
            return _repositorioAgendamento.Adicionar(agend);
        }

        public bool Cancelar(int id)
        {
            var agendamento = _repositorioAgendamento.BuscarPorId(id);
            if (agendamento == null || agendamento.Status != StatusAgendamento.Agendado)
            {
                return false;
            }
            agendamento.Status = StatusAgendamento.Cancelado;
            _repositorioAgendamento.Salvar();
            return true;
        }

        public Agendamento[] ListarOrdenado()
        {
            var copia = _repositorioAgendamento.ListarTodos();
            Array.Sort(copia, (a, b) => a.Inicio.CompareTo(b.Inicio));
            return copia;
        }

        public bool Concluir(int id)
        {
            var agendamento = _repositorioAgendamento.BuscarPorId(id);
            if (agendamento == null || agendamento.Status != StatusAgendamento.Agendado)
            {
                return false;
            }
            agendamento.Status = StatusAgendamento.Concluido;
            _repositorioAgendamento.Salvar();
            return true;
        }

        public decimal FaturamentoMes(int ano, int mes)
        {
            decimal total = 0m;
            foreach (var agendamento in _repositorioAgendamento.ListarTodos())
            {
                if (agendamento.Status == StatusAgendamento.Concluido && agendamento.Inicio.Year == ano && agendamento.Inicio.Month == mes)
                {
                    total += agendamento.Valor;
                }
            }
            return total;
        }


    }
}
