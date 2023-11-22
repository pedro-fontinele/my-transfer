using TCloudFileSync.Aplicacao.Auxiliar;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Servico;
using TCloudFileSync.Aplicacao.Validacao;
using WinSCP;

namespace TCloudFileSync.WinSCP
{
    /// <summary>
    /// Documentação: <link>https://winscp.net/eng/docs/library#csharp</link>
    /// </summary>
    public class WinSCPSftpClientServico : ISftpClientServico
    {
        private Session client;
        private SessionOptions sessionOptions;

        private ConfiguracaoClienteSftp configuracoesAtuais;

        public void ApagaArquivo(string arquivo)
        {
            client.RemoveFile(arquivo);
        }

        public ConfiguracaoClienteSftp BuscaConfiguracoesAtuais()
        {
            return configuracoesAtuais;
        }

        public bool ClienteSftpJaConfigurado()
        {
            return client is not null && configuracoesAtuais is not null;
        }

        public void Conecta()
        {
            client.Open(sessionOptions);
        }

        public void CriaPasta(string caminho)
        {
            client.CreateDirectory(caminho);
        }

        public void Desconecta()
        {
            client.Close();
        }

        public bool Existe(string caminho)
        {
            return client.FileExists(caminho);
        }

        public void GeraNovoSftpClient(ConfiguracaoClienteSftp param)
        {
            configuracoesAtuais = param;

            sessionOptions = new SessionOptions
            {
                Protocol = Protocol.Sftp,
                HostName = param.HostIp(),
                PortNumber = param.Port,
                UserName = param.Username,
                Password = param.Password,
                SshHostKeyPolicy = SshHostKeyPolicy.GiveUpSecurityAndAcceptAny
            };

            client = new Session();
        }

        public Stream LeArquivo(string arquivo)
        {
            return client.GetFile(arquivo);
        }

        public ArquivoVindoDaNuvem[] ListaArquivosEmDiretorio(string caminho)
        {
            var files = client.ListDirectory(caminho).Files;

            var retorno = new List<ArquivoVindoDaNuvem>();

            foreach (RemoteFileInfo item in files)
            {
                if (ValidadorDiretorioNuvem.ArquivoValido(item.Name) && !item.IsDirectory)
                {
                    retorno.Add(new ArquivoVindoDaNuvem(item.Name, item.FullName));
                }
            }

            return retorno.ToArray();
        }

        public void Upload(Stream streamArquivoLocal, string caminhoArquivoNuvem)
        {
            client.PutFile(streamArquivoLocal, caminhoArquivoNuvem);
        }

        public bool VerificaSeArquivoExisteEmNuvem(string caminho, string nomeArquivo)
        {
            throw new NotImplementedException();
        }
    }
}