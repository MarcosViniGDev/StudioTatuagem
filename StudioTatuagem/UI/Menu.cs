using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using StudioTatuagem.Models;
using StudioTatuagem.Repositorios;
using StudioTatuagem.Services;

namespace StudioTatuagem.UI
{
    public class Menu
    {
        private readonly IRepositorio<Cliente> _repositorioCliente = new RepositorioJson<Cliente>("clientes.json");
        private readonly Agenda _agenda = new Agenda(new RepositorioJson<Agendamento>("agendamentos.json"));
        private readonly CultureInfo _pt = new CultureInfo("pt-BR");

        public void Executar()
        {
            while (true)
            {
                Console.WriteLine("=== Menu ===");
                Console.WriteLine("1. Cadastrar Cliente");
                Console.WriteLine("2. Listar Clientes");
                Console.WriteLine("3. Agendar Sessão");
                Console.WriteLine("4. Listar Agendamentos");
                Console.WriteLine("5. Cancelar Agendamento");
                Console.WriteLine("6. Concluir Agendamento");
                Console.WriteLine("7. Faturamento do Mês");
                Console.WriteLine("0. Sair");
                Console.Write("Escolha uma opção: ");
                string opcao = Console.ReadLine() ?? "";
                switch (opcao)
                {
                    case "1":
                        CadastrarCliente();
                        break;
                    case "2":
                        ListarClientes();
                        break;
                    case "3":
                        AgendarSessao();
                        break;
                    case "4":
                        ListarAgendamentos();
                        break;
                    case "5":
                        CancelarAgendamento();
                        break;
                    case "6":
                        ConcluirAgendamento();
                        break;
                    case "7":
                        FaturamentoMensal();
                        break;  
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        break;
                }

            }
        }
        private void CadastrarCliente()
        {
            Console.Write("Digite o nome do cliente: ");
            string nome = Console.ReadLine() ?? "";
            Console.Write("Digite o telefone do cliente: ");
            string telefone = Console.ReadLine() ?? "";
            Console.Write("Digite uma observação sobre o cliente: ");
            string observacao = Console.ReadLine() ?? "";
            Cliente cliente = new Cliente(nome, telefone, observacao);
            _repositorioCliente.Adicionar(cliente);
            Console.WriteLine("Cliente cadastrado com sucesso!");
        }
        private void ListarClientes()
        {
            var clientes = _repositorioCliente.ListarTodos();
            if (clientes.Length == 0)
            {
                Console.WriteLine("Nenhum cliente cadastrado.");
                return;
            }
            Console.WriteLine("=== Lista de Clientes ===");
            foreach (var cliente in clientes)
            {
                Console.WriteLine(cliente.Resumo());
            }
        }
        private void AgendarSessao()
        {
            // Solicitando o ID do cliente para o agendamento
            Console.Write("Digite o ID do cliente: ");
            if (!int.TryParse(Console.ReadLine(), out int idCliente))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            var cliente = _repositorioCliente.BuscarPorId(idCliente);
            if (cliente is null)
            {
                Console.WriteLine("Cliente não encontrado.");
                return;
            }
            // Solicitando a data e hora de início do agendamento
            Console.Write("Digite a data e hora de início (dd/MM/yyyy HH:mm): ");

            if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy HH:mm", _pt, DateTimeStyles.None, out DateTime inicio))
            {
                Console.WriteLine("Data inválida.");
                return;
            }

            // Solicitando a duração do agendamento
            Console.Write("Digite a duração da sessão (em minutos): ");

            if (!int.TryParse(Console.ReadLine(), out int duracao) || duracao <= 0)
            {
                Console.WriteLine("Duração inválida.");
                return;
            }
            // Solicitando a descrição do agendamento
            Console.Write("Digite a descrição da sessão: ");
            string descricao = Console.ReadLine() ?? "";

            // Solicitando o valor estimado da sessão
            Console.Write("Digite o valor estimado da sessão: ");
            if (!decimal.TryParse(Console.ReadLine(), NumberStyles.Number, _pt, out decimal valor) || valor <= 0)
            {
                Console.WriteLine("Valor inválido.");
                return;
            }
            // Tentando agendar a sessão
            var agendamento = _agenda.Agendar(cliente, inicio, duracao, descricao, valor);

            if (agendamento is null)
            {
                Console.WriteLine("Conflito de horário: já existe uma sessão nesse período.");
                return;
            }

            Console.Write($"\n Sessão agendada! ID {agendamento.Id}.");
        }

        private void ListarAgendamentos()
        {
            var agendamentos = _agenda.ListarOrdenado();
            if (agendamentos.Length == 0)
            {
                Console.WriteLine("Nenhum agendamento cadastrado.");
                return;
            }
            Console.WriteLine("=== Lista de Agendamentos ===");
            foreach (var agendamento in agendamentos)
            {
                Console.WriteLine(agendamento.Resumo());
            }
        }

        private void CancelarAgendamento()
        {
            Console.Write("Digite o ID do agendamento a ser cancelado: ");
            if (!int.TryParse(Console.ReadLine(), out int idAgendamento))
            {
                Console.WriteLine("ID inválido.");
                return;
            }

            if (_agenda.Cancelar(idAgendamento))
            {  
                Console.WriteLine("Agendamento cancelado com sucesso.");
            }
            else
            {
                Console.WriteLine("Agendamento não encontrado ou já cancelado.");
                return;
            }
        }

        private void ConcluirAgendamento()
        {
            Console.Write("Digite o ID do agendamento a ser concluído: ");
            if (!int.TryParse(Console.ReadLine(), out int idAgendamento))
            {
                Console.WriteLine("ID inválido.");
                return;
            }
            if (_agenda.Concluir(idAgendamento))
            {
                Console.WriteLine("Agendamento concluído com sucesso.");
            }
            else
            {
                Console.WriteLine("Agendamento não encontrado ou já concluído/cancelado.");
            }
        }

        private void FaturamentoMensal()
        {
            Console.Write("Digite o ano: ");
            if (!int.TryParse(Console.ReadLine(), out int ano))
            {
                Console.WriteLine("Ano inválido.");
                return;
            }

            Console.Write("Digite o mês: ");
            if (!int.TryParse(Console.ReadLine(), out int mes) || mes < 1 || mes > 12)
            {
                Console.WriteLine("Mês inválido.");
                return;
            }

            decimal faturamento = _agenda.FaturamentoMes(ano, mes);
            Console.WriteLine($"Faturamento do mês {mes}/{ano}: R$ {faturamento.ToString("C", _pt)}");
        }
    }
}
