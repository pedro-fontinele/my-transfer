using Renci.SshNet;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Servico;
using TCloudFileSync.Aplicacao.Validacao;

namespace TCloudFileSync.RenciSSH
{
    public class SshNetSftpClientServico : ISftpClientServico
    {
        private SftpClient sftpClient;
        private ConfiguracaoClienteSftp configuracoesAtuais;

        public virtual void Upload(Stream streamArquivoLocal, string caminhoArquivoNuvem)
        {
            sftpClient.UploadFile(streamArquivoLocal, caminhoArquivoNuvem);
        }

        public virtual void Conecta()
        {
            var timeSpan = new System.TimeSpan(0, 0, 60);
            sftpClient.OperationTimeout = timeSpan;
            sftpClient.Connect();
        }

        public virtual void GeraNovoSftpClient(ConfiguracaoClienteSftp param)
        {
            configuracoesAtuais = param;
            sftpClient = new SftpClient(param.HostIp(), param.Port, param.Username, param.Password);
        }

        public virtual void Desconecta()
        {
            if (sftpClient != null && sftpClient.IsConnected)
            {
                sftpClient.Disconnect();
            }
        }

        public virtual bool Existe(string caminho)
        {
            return sftpClient.Exists(caminho);
        }

        public virtual void CriaPasta(string caminho)
        {
            sftpClient.CreateDirectory(caminho);
        }

        public virtual bool ClienteSftpJaConfigurado()
        {
            return sftpClient is not null && configuracoesAtuais is not null;
        }

        public virtual ConfiguracaoClienteSftp BuscaConfiguracoesAtuais()
        {
            return configuracoesAtuais;
        }

        public virtual ArquivoVindoDaNuvem[] ListaArquivosEmDiretorio(string caminho)
        {
            var files = sftpClient.ListDirectory(caminho).ToImmutableArray();

            var retorno = new List<ArquivoVindoDaNuvem>();

            for (var i = 0; i < files.Length; i++)
            {
                if (ValidadorDiretorioNuvem.ArquivoValido(files[i].Name) && !files[i].IsDirectory)
                {
                    retorno.Add(new ArquivoVindoDaNuvem(files[i].Name, files[i].FullName));
                }
            }

            return retorno.ToArray();
        }

        public virtual bool VerificaSeArquivoExisteEmNuvem(string caminho, string nomeArquivo)
        {
            var files = sftpClient.ListDirectory(caminho).FirstOrDefault(e => e.Name == nomeArquivo);

            if (files != null)
            {
                return files.Name == nomeArquivo;
            }

            return false;
        }

        public virtual Stream LeArquivo(string arquivo)
        {
            Stream stream = new MemoryStream();
            sftpClient.DownloadFile(arquivo, stream);

            return stream;
        }

        public virtual void ApagaArquivo(string arquivo)
        {
            sftpClient.DeleteFile(arquivo);
        }
    }
}
