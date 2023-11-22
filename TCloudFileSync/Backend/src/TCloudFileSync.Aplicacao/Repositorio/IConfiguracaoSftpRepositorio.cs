using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;

namespace TCloudFileSync.Aplicacao.Repositorio
{
    public interface IConfiguracaoSftpRepositorio
    {
        EnviaArquivoDto BuscaConfiguracoesSftp();
        Task InsereConfiguracaoSftp(ConfiguracaoClienteSftp configuracaoClienteSftp);
        Task AtualizaConfiguracaoSftp(ConfiguracaoClienteSftp configuracaoClienteSftp);
    }
}
