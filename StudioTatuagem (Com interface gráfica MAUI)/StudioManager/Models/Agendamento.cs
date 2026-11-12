using System.Text.Json.Serialization;

namespace StudioManager.Models
{
    public class Agendamento : IEntidade
    {
        public int Id { get; set; }

        public int ClienteId { get; private set; }

        [JsonIgnore]
        public Cliente? Cliente { get; set; }

        public DateTime Inicio { get; private set; }
        public int DuracaoMinutos { get; private set; }
        public string Descricao { get; private set; }
        public decimal Valor { get; private set; }
        public StatusAgendamento Status { get; set; } = StatusAgendamento.Agendado;

        [JsonIgnore]
        public DateTime Fim => Inicio.AddMinutes(DuracaoMinutos);

        public Agendamento(Cliente cliente, DateTime inicio, int duracaoMinutos, string descricao, decimal valor)
            : this(cliente.Id, inicio, duracaoMinutos, descricao, valor)
        {
            Cliente = cliente;
        }
        [JsonConstructor]
        public Agendamento(int clienteId, DateTime inicio, int duracaoMinutos, string descricao, decimal valor)
        {
            ClienteId = clienteId;
            Inicio = inicio;
            DuracaoMinutos = duracaoMinutos;
            Descricao = descricao;
            Valor = valor;
        }

        public void Atualizar(Cliente cliente, DateTime inicio, int duracaoMinutos, string descricao, decimal valor)
        {
            Cliente = cliente;
            ClienteId = cliente.Id;
            Inicio = inicio;
            DuracaoMinutos = duracaoMinutos;
            Descricao = descricao;
            Valor = valor;
        }

        public bool ConflitaCom(Agendamento outro) =>
            Inicio < outro.Fim && outro.Inicio < Fim;

        public string Resumo() =>
            $"[{Id}] {Inicio:dd/MM/yyyy HH:mm} - {Fim:HH:mm} | {Cliente?.Nome} | {Descricao} | R$ {Valor:F2} | {Status}";
    }
}