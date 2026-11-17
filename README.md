# StudioTatuagem
Aluno: Marcos Vinicius da Silva Santos 

Nome do Projeto: SM Tattoo

O projeto foi feito utilizando C# e o .NET MAUI, além disse foi utilizado a inteligência artificial Claude, sendo utilizada como um guia e um copiloto onde me ajudava com as ideias e alguns códigos que tive dificuldade por nunca ter visto

Objetivo: O principal objetivo do SM é gerar um controle maior dos agendamentos e do faturamento de uma tatuadora

Principais funcionalidades: As principais funcionalidades até o momento são :
 1. Cadastro de Cliente: Podendo cadastrar diversos clientes e salvar em um arquivo
 2. Cadastro de Agendamento: Agenda uma nova sessão de tatuagem utilizando os clientes ja cadastrados, podendo atualizar o status desse agendamento para cancelado e concluido
 3. Sistema de faturamento Mensal: A cada sessão de tatuagem concluida automaticamente o valor ganho por esta tatuagem é atualizado no mes em que ela foi feita, podendo selecionar na aba de faturamento para ver o total do mes
 4. Exclusão/Edição de clientes e Agendamentos
 5. Sistema de Backup de dados


 Futuras Atualizações: 
 
 1. Instalador do programa para nao precisar do visual studio
 3. Sistema de Estoque de materiais para melhor controle
 4. Banco de Dados
 5. Porte para Mobile

Como executar

Clone o repositório
Abra a solução no Visual Studio (arquivo .sln dentro da pasta do projeto MAUI).
Restaure os pacotes. O Visual Studio faz isso automaticamente ao abrir a solução. Se preferir, use Ferramentas → Gerenciador de Pacotes NuGet → Restaurar Pacotes.
Escolha o destino de execução. Na barra de ferramentas, ao lado do botão verde, selecione Windows Machine.
Execute com F5 (com depuração) ou Ctrl+F5 (sem depuração).

Na primeira execução, o app abre com as listas vazias. Os arquivos de dados são criados automaticamente quando o primeiro cliente é cadastrado.


Como usar

Em Clientes, clique em Novo cliente e preencha os dados.
Em Agenda, clique em Nova sessão, escolha o cliente, a data, o horário, a duração, a descrição e o valor.
Na lista da agenda, clique em uma sessão para ver os detalhes no painel à direita. Ali é possível anexar uma imagem de referência.
Use Editar, Concluir ou Cancelar sobre a sessão selecionada.
Em Faturamento, informe o ano e o mês para ver o total das sessões concluídas.
Em Backup, clique em Fazer backup agora para gerar uma cópia de segurança.

Problemas comuns

Problema --->	O que fazer

O Visual Studio não encontra o destino Windows Machine ---> 	Instale a carga de trabalho do .NET MAUI pelo Visual Studio Installer

Erro pedindo o modo de desenvolvedor --->	Ative em Configurações do Windows → Sistema → Para desenvolvedores

Erros de InitializeComponent duplicado ou arquivos .xaml gerados em duplicidade --->	Compilar → Limpar Solução, feche o Visual Studio, apague as pastas bin e obj e recompile

Os dados não aparecem depois de reabrir o app --->	Confira na tela Backup se o app está lendo a pasta de dados esperada
