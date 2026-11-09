using System;
using System.Collections.Generic;
using System.Text;
using StudioTatuagem.Models;
using StudioTatuagem.Repositorios;

namespace StudioTatuagem.Services
{
    public class Agenda
    {
        private readonly RepositorioEmMemoria<Agendamento> _repositorioAgendamento = new RepositorioEmMemoria<Agendamento>();
        
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
            return true;
        }

        public Agendamento[] ListarOrdenado()
        {
            var copia = _repositorioAgendamento.ListarTodos();
            Array.Sort(copia, (a, b) => a.Inicio.CompareTo(b.Inicio));
            return copia;
        }
    }
}
