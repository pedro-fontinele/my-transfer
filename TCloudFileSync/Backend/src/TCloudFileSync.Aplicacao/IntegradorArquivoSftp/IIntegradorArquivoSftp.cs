using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;

namespace TCloudFileSync.Aplicacao
{
    public interface IIntegradorArquivoSftp
    {
        Task IniciaFluxoDeIntegracao();
        int BuscaId();
        void AplicaConfiguracoes(ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, ConfiguracaoClienteSftp configuracaoClienteSftp);
        void GeraNotificacaoParaInterromperProcesso();
        (ConfiguracaoEnvioArquivo, ConfiguracaoClienteSftp) BuscaConfiguracoes();
    }
}
