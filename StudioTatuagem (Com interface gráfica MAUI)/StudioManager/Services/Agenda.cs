using StudioManager.Models;
using StudioManager.Repositorios;

namespace StudioManager.Services
{
    public class Agenda
    {
        private readonly IRepositorio<Agendamento> _repositorioAgendamento;

        public Agenda(IRepositorio<Agendamento> agendamentos, IRepositorio<Cliente> clientes)
        {
            _repositorioAgendamento = agendamentos;

            // O arquivo guarda só o ClienteId; aqui cada sessão é ligada ao cliente real
            foreach (var agendamento in _repositorioAgendamento.ListarTodos())
            {
                agendamento.Cliente = clientes.BuscarPorId(agendamento.ClienteId);
            }
        }

        public Agendamento? Agendar(Cliente cliente, DateTime inicio, int duracao, string descricao, decimal valor)
        {
            var novo = new Agendamento(cliente, inicio, duracao, descricao, valor);

            foreach (var existente in _repositorioAgendamento.ListarTodos())
            {
                if (existente.Status == StatusAgendamento.Agendado && novo.ConflitaCom(existente))
                {
                    return null;
                }
            }

            return _repositorioAgendamento.Adicionar(novo);
        }

        public ResultadoEdicao Editar(int id, Cliente cliente, DateTime inicio, int duracao, string descricao, decimal valor)
        {
            var agendamento = _repositorioAgendamento.BuscarPorId(id);

            if (agendamento is null)
            {
                return ResultadoEdicao.NaoEncontrado;
            }

            if (agendamento.Status != StatusAgendamento.Agendado)
            {
                return ResultadoEdicao.StatusInvalido;
            }

            var candidato = new Agendamento(cliente, inicio, duracao, descricao, valor);

            foreach (var existente in _repositorioAgendamento.ListarTodos())
            {
                if (existente.Id != id
                    && existente.Status == StatusAgendamento.Agendado
                    && existente.ConflitaCom(candidato))
                {
                    return ResultadoEdicao.Conflito;
                }
            }

            agendamento.Atualizar(cliente, inicio, duracao, descricao, valor);
            _repositorioAgendamento.Salvar();
            return ResultadoEdicao.Sucesso;
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

        public bool ClienteTemAgendamentos(int clienteId)
        {
            foreach (var agendamento in _repositorioAgendamento.ListarTodos())
            {
                if (agendamento.ClienteId == clienteId)
                {
                    return true;
                }
            }
            return false;
        }

        public Agendamento[] ListarOrdenado()
        {
            var copia = _repositorioAgendamento.ListarTodos();
            Array.Sort(copia, (a, b) => a.Inicio.CompareTo(b.Inicio));
            return copia;
        }

        public decimal FaturamentoMes(int ano, int mes)
        {
            decimal total = 0m;
            foreach (var agendamento in _repositorioAgendamento.ListarTodos())
            {
                if (agendamento.Status == StatusAgendamento.Concluido
                    && agendamento.Inicio.Year == ano
                    && agendamento.Inicio.Month == mes)
                {
                    total += agendamento.Valor;
                }
            }
            return total;
        }
    }
}