using System.IO;
using System.Threading.Tasks;

namespace TCloudFileSync.Aplicacao.Auxiliar
{
    /// <summary>
    /// Classe que abstrai funcionalidades de manipulação de arquivos e diretórios contidos no namespace System.IO 
    /// </summary>
    public class LeitorArquivo
    {
        /// <summary>
        /// Abre e lê arquivo 
        /// </summary>
        public virtual Stream LeArquivo(string caminhoArquivo) => File.OpenRead(caminhoArquivo);

        /// <summary>
        /// Verifica se existe o arquivo informado 
        /// </summary>
        public virtual bool CaminhoArquivoLocalExiste(string caminhoArquivo) => Directory.Exists(caminhoArquivo);

        /// <summary>
        /// Retorna o nome do arquivo informado no caminho absoluto 
        /// </summary>
        public virtual string RecuperaNomeDoArquivo(string caminhoArquivo) => Path.GetFileName(caminhoArquivo);

        /// <summary>
        /// Faz a cópia local de um arquivo para um novo diretório 
        /// </summary>
        public virtual void CopiaArquivoParaPastaTemporaria(string caminhoArquivoOrigem, string caminhoArquivoDestino) => File.Copy(caminhoArquivoOrigem, caminhoArquivoDestino);

        /// <summary>
        /// Cria pasta para armazenar arquivos locas 
        /// </summary>
        public virtual void CriaPastaLocal(string nomeDiretorio) => Directory.CreateDirectory(nomeDiretorio);

        /// <summary>
        /// Retorna um caminho absoluto de uma nova pasta 
        /// </summary>
        public virtual string PreparaCaminhoArquivoLocal(string caminho, string arquivoOuPasta) => Path.Combine(caminho, arquivoOuPasta);

        /// <summary>
        /// Retorna lista de arquivos encontrados no diretório informado 
        /// </summary>
        public virtual string[] BuscaArquivosEmDiretorioLocal(string nomeDiretorio) => Directory.GetFiles(nomeDiretorio);

        /// <summary>
        /// Busca nome do diretório onde será criada a pasta temporária 
        /// </summary>
        public virtual string BuscaNomeDiretorioAtual() => Directory.GetCurrentDirectory();

        /// <summary>
        /// Deleta arquivo de ambiente local 
        /// </summary>
        public virtual void RemoveArquivo(string arquivo) => File.Delete(arquivo);

        /// <summary>
        /// Deleta todos os arquivos contidos na pasta informada 
        /// </summary>
        public virtual void LimpaPasta(string caminhoPasta)
        {
            var dir = new DirectoryInfo(caminhoPasta);
            
            foreach (var fileInfo in dir.GetFiles())
            {
                fileInfo.Delete();
            }
        }

        /// <summary>
        /// Cria arquivo e retorna Stream para manipulação do conteúdo do arquivo criado 
        /// </summary>
        public virtual Stream CriaEAbreArquivoParaEscrita(string caminhoArquivo) => File.Create(caminhoArquivo);

        /// <summary>
        /// Copia conteúdo do Stream de um arquivo para o outro 
        /// </summary>
        public virtual async Task CopiaConteudo(Stream conteudoOrigem, Stream conteudoDestino)
        {
            conteudoOrigem.Position = 0;
            conteudoDestino.Position = 0;
            await conteudoOrigem.CopyToAsync(conteudoDestino);
        }

        /// <summary>
        /// Libera recursos utilizados para manipulação do conteúdo de um arquivo 
        /// </summary>
        public virtual void FechaRecursosDeLeituraEscrita(Stream stream) => stream.Dispose();
    }
}
