using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;

namespace TCloudFileSync.Aplicacao.Servico
{
    public interface IHistoricoSincronismoService
    {
        Task<List<HistoricoSincronismoDto>> BuscaHistoricoSincronismo(int pagina = 1, int itensPorPagina = 20);
        Task InsereHistoricoSincronismo(ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, string nomeArquivoComExtensao, string situacaoSincronismo, DateTime dateTime, double tamanhoEmKB);
        Task AtualizaHistoricoSincronismo(string nomeArquivoComExtensao, string situacaoSincronismo, DateTime dateTime);
    }
}
