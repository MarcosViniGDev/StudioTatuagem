using StudioTatuagem.Models;

Console.WriteLine("Hello, World!");

Cliente cliente = new Cliente("Marco", "123456789", "Cliente VIP");
Agendamento agendamento = new Agendamento(1, cliente, new DateTime(2026, 11, 5, 9, 0, 0), 60, "Tatuagem de dragão", 200.00m);
Agendamento agendamento2 = new Agendamento(2, cliente, new DateTime(2026, 11, 5, 9, 0, 0), 30, "Tatuagem de fênix", 150.00m);

Console.WriteLine(agendamento.conflitaCom(agendamento2));
Console.WriteLine(agendamento.resumo());
Console.WriteLine(agendamento2.resumo());