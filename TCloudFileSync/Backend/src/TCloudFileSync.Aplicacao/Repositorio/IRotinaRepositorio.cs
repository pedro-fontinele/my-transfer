using System.Collections.Generic;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;

namespace TCloudFileSync.Aplicacao.Repositorio
{
    public interface IRotinaRepositorio
    {
        ConfiguracaoEnvioArquivo BuscaRotinaPorId(int id);
        List<ConfiguracaoEnvioArquivo> BuscaTodasAsRotinas();
        Task InsereRotina(ConfiguracaoEnvioArquivo consultaConfiguracaoEnvioArquivo);
        Task AtualizaRotina(ConfiguracaoEnvioArquivo consultaConfiguracaoEnvioArquivo);
    }
}
