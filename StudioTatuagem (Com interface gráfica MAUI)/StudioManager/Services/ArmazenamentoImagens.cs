namespace StudioManager.Services
{
    // Guarda as imagens das sessões em uma pasta própria do app.
    // O agendamento só registra o NOME do arquivo, nunca o caminho completo.
    public class ArmazenamentoImagens
    {
        private readonly string _pasta;

        public ArmazenamentoImagens(string pasta)
        {
            _pasta = pasta;
            Directory.CreateDirectory(_pasta);
        }

        // Copia a imagem escolhida para a pasta do app e devolve o nome do arquivo criado
        public async Task<string> SalvarAsync(Stream origem, string nomeOriginal)
        {
            string extensao = Path.GetExtension(nomeOriginal);
            string nomeArquivo = $"{Guid.NewGuid()}{extensao}";
            string destino = Path.Combine(_pasta, nomeArquivo);

            using var saida = File.Create(destino);
            await origem.CopyToAsync(saida);

            return nomeArquivo;
        }

        public string CaminhoCompleto(string nomeArquivo) =>
            Path.Combine(_pasta, nomeArquivo);

        public bool Existe(string nomeArquivo) =>
            File.Exists(CaminhoCompleto(nomeArquivo));

        public void Excluir(string nomeArquivo)
        {
            try
            {
                File.Delete(CaminhoCompleto(nomeArquivo));
            }
            catch (IOException)
            {
                // Se o arquivo estiver em uso, ele só fica na pasta; não derruba o programa
            }
        }
    }
}
