using System.Collections.Generic;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;

namespace TCloudFileSync.Aplicacao.Servico
{
    public interface IRotinaServico
    {
        ConfiguracaoEnvioArquivo BuscaRotinaPorId(int id);
        List<ConfiguracaoEnvioArquivo> BuscaTodasAsRotinas();
        Task<EnviaArquivoDto> InsereOuAtualizaRotina(ConfiguracaoEnvioArquivo consultaConfiguracaoEnvioArquivo);
    }
}
