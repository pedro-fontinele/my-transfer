using System.IO;
using TCloudFileSync.Aplicacao.Dto;

namespace TCloudFileSync.Aplicacao.Servico
{
    public interface ISftpClientServico
    {
        /// <summary>
        /// Gera o SftpClient que será utilizado para envio do arquivo via SFTP 
        /// </summary>
        void GeraNovoSftpClient(ConfiguracaoClienteSftp param);

        /// <summary>
        /// Desconecta cliente SFTP utilizado na transação 
        /// </summary>
        void Desconecta();

        /// <summary>
        /// Estabelece conexão SFTP utilizando o client gerado 
        /// </summary>
        void Conecta();

        /// <summary>
        /// Realiza o envio do arquivo para a nuvem
        /// </summary>
        void Upload(Stream streamArquivoLocal, string caminhoArquivoNuvem);

        /// <summary>
        /// Verifica se pasta no caminho informado existe 
        /// </summary>
        bool Existe(string caminho);

        /// <summary>
        /// Cria novo diretório no caminho especificado 
        /// </summary>
        void CriaPasta(string caminho);

        /// <summary>
        /// Verifica se já existe cliente SFTP devidamente configurado 
        /// </summary>
        bool ClienteSftpJaConfigurado();

        /// <summary>
        /// Retorna as configurações atuais do cliente SFTP 
        /// </summary>
        /// <returns></returns>
        ConfiguracaoClienteSftp BuscaConfiguracoesAtuais();

        /// <summary>
        /// Busca a lista de todos os arquivos contigos no caminho especificado
        /// </summary>
        ArquivoVindoDaNuvem[] ListaArquivosEmDiretorio(string caminho);

        bool VerificaSeArquivoExisteEmNuvem(string caminho, string nomeArquivo);

        /// <summary>
        /// Realiza leitura do arquivo espcificado contido em ambiente na nuvem 
        /// </summary>
        Stream LeArquivo(string arquivo);

        /// <summary>
        /// Remove arquivo especificado em ambiente na nuvemn 
        /// </summary>
        void ApagaArquivo(string arquivo);
    }
}
