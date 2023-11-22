using System;
using TCloudFileSync.Aplicacao.Auxiliar;

namespace TCloudFileSync.Aplicacao.Dto
{
    /// <summary>
    /// Armazena dados para envio de arquivo 
    /// <br></br>
    /// <br></br>
    /// Parâmetros de entrada do método IntegradorArquivoSftp.EnviaArquivo
    /// </summary>
    public class EnviaArquivoDto
    {
        public ConfiguracaoClienteSftp ConfiguracaoClienteSftp { get; set; }
        public ConfiguracaoEnvioArquivo[] ConfiguracaoEnvioArquivo { get; set; }
    }

    /// <summary>
    /// Dados de acesso SFTP 
    /// </summary>
    public class ConfiguracaoClienteSftp
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // TODO ver possível criptografia 

        public string HostIp() => ManipuladorDns.BuscaEnderecoDoHost(this.Host);

        public override bool Equals(object obj)
        {
            return obj is ConfiguracaoClienteSftp sftp &&
                   Host == sftp.Host &&
                   Port == sftp.Port &&
                   Username == sftp.Username &&
                   Password == sftp.Password;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Host, Port, Username, Password);
        }

        public ConfiguracaoClienteSftp()
        {
                
        }

        public ConfiguracaoClienteSftp(string host, int port, string username, string password)
        {
            Host = host;
            Port = port;
            Username = username;
            Password = password;
        }
    }

    public class ConfiguracaoEnvioArquivo
    {
        public ConfiguracaoEnvioArquivo()
        {
                
        }

        public bool Ativo { get; set; }
        public string CaminhoArquivoLocal { get; set; }
        public string CaminhoArquivoNuvem { get; set; }
        public int Id { get; set; }
        public int TempoParaIniciarProximoEnvio { get; set; } = 1000;
        public bool DeveApagarArquivoOrigem { get; set; } = true;
        public bool FluxoLocalParaNuvem { get; set; } = true;
        public int IdConfiguracaoSftp { get; set; }

        public override bool Equals(object obj)
        {
            return obj is ConfiguracaoEnvioArquivo arquivo &&
                   Ativo == arquivo.Ativo &&
                   CaminhoArquivoLocal == arquivo.CaminhoArquivoLocal &&
                   CaminhoArquivoNuvem == arquivo.CaminhoArquivoNuvem &&
                   Id == arquivo.Id &&
                   TempoParaIniciarProximoEnvio == arquivo.TempoParaIniciarProximoEnvio &&
                   DeveApagarArquivoOrigem == arquivo.DeveApagarArquivoOrigem &&
                   FluxoLocalParaNuvem == arquivo.FluxoLocalParaNuvem;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Ativo, CaminhoArquivoLocal, CaminhoArquivoNuvem, Id, TempoParaIniciarProximoEnvio, DeveApagarArquivoOrigem, FluxoLocalParaNuvem);
        }
    }
}
