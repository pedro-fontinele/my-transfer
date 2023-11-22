using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;

namespace TCloudFileSync.Aplicacao.Servico
{
    public interface IConfiguracaoSftpServico
    {
        EnviaArquivoDto BuscaConfiguracoesSftp();
        Task<EnviaArquivoDto> InsereOuAtualizaConfiguracaoSftp(ConfiguracaoClienteSftp  configuracaoClienteSftp);
    }
}
