using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;

namespace TCloudFileSync.Aplicacao.Repositorio
{
    public interface IHistoricoSincronismoRepositorio
    {
        Task<List<HistoricoSincronismoDto>> BuscaHistoricoSincronismo(int pagina = 1, int itensPorPagina = 20);
        HistoricoSincronismoDto BuscaHistoricoPorNomeArquivo(string nomeComExtensao, DateTime dateTime);
        void InsereHistoricoSincronismo(HistoricoSincronismoDto historicoSincronismoDto);
        void AtualizaHistoricoSincronismo(HistoricoSincronismoDto historicoSincronismoDto);
    }
}
