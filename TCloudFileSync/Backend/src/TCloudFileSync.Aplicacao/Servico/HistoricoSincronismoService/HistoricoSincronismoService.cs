using Microsoft.VisualBasic;
using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Configuracoes;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Enum;
using TCloudFileSync.Aplicacao.Repositorio;

namespace TCloudFileSync.Aplicacao.Servico
{
    public class HistoricoSincronismoService : IHistoricoSincronismoService
    {
        private ILogServico _logServico;
        private readonly Notificacao _notificacao;
        private readonly IHistoricoSincronismoRepositorio _historicoSincronismoRepositorio;

        public HistoricoSincronismoService(Notificacao notificacao, IHistoricoSincronismoRepositorio historicoSincronismoRepositorio, ILogServico logServico)
        {
            _notificacao = notificacao;
            _historicoSincronismoRepositorio = historicoSincronismoRepositorio;
            _logServico = logServico;
        }

        public async Task<List<HistoricoSincronismoDto>> BuscaHistoricoSincronismo(int pagina = 1, int itensPorPagina = 20)
        {
            try
            {
                return await _historicoSincronismoRepositorio.BuscaHistoricoSincronismo(pagina, itensPorPagina);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task InsereHistoricoSincronismo(ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, string nomeArquivoComExtensao, string situacaoSincronismo, DateTime dateTime, double tamanhoEmKB)
        {
            try
            {
                var historicoSincronismoDto = HistoricoSincronismoConverter.Converter(configuracaoEnvioArquivo, nomeArquivoComExtensao, situacaoSincronismo, dateTime, tamanhoEmKB);
                if (historicoSincronismoDto is null)
                {
                    _logServico.Information("Erro na conversao dos valores para historicoSincronismoDto, rotina stopada");
                    return;
                }

                _logServico.Information("Realizando insert de um novo registro na aplicaçao de historico de integração.");
                _logServico.Information($"Arquivo {nomeArquivoComExtensao} tamanho {historicoSincronismoDto.Tamanho} em KB origem: {(configuracaoEnvioArquivo.FluxoLocalParaNuvem ? configuracaoEnvioArquivo.CaminhoArquivoLocal : configuracaoEnvioArquivo.CaminhoArquivoNuvem)} destino {(configuracaoEnvioArquivo.FluxoLocalParaNuvem ? configuracaoEnvioArquivo.CaminhoArquivoNuvem : configuracaoEnvioArquivo.CaminhoArquivoLocal)} | FLUXO EXECUTADO: {(configuracaoEnvioArquivo.FluxoLocalParaNuvem ? "Do local para nuvem" : "Da nuvem para local")}");
                _historicoSincronismoRepositorio.InsereHistoricoSincronismo(historicoSincronismoDto);
                _logServico.Information($"Log do aquivo {nomeArquivoComExtensao} inserido com sucesso na aplicação de sincronissmo, realizando consulta do historio para continuidade da integração.");
                await BuscaHistoricoSincronismo();
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao tentar inserir novo o historico de integração do arquivos {nomeArquivoComExtensao} na base de dados | FLUXO EXECUTADO: {(configuracaoEnvioArquivo.FluxoLocalParaNuvem ? "Do local para nuvem" : "Da nuvem para local")}  Erro : {ex.Message}");
            }
        }

        public async Task AtualizaHistoricoSincronismo(string nomeArquivoComExtensao, string situacaoSincronismo, DateTime dateTime)
        {
            var historico = new HistoricoSincronismoDto();
            try
            {
                historico = _historicoSincronismoRepositorio.BuscaHistoricoPorNomeArquivo(nomeArquivoComExtensao, dateTime);
                if (historico is null)
                {
                    _logServico.Information($"Erro na consulta do registo do arquivo {nomeArquivoComExtensao} no banco de dados, rotina stopada");
                    return;
                }

                _logServico.Information($"Realizando uppdate do registro do arquivo {nomeArquivoComExtensao} na aplicaçao de historico de integração.");
                _logServico.Information($"Atualizando status do arquio {nomeArquivoComExtensao} tamanho {historico.Tamanho} em KB origem: {(historico.FluxoLocalParaNuvem == "S" ? historico.CaminhoArquivoLocal : historico.CaminhoArquivoNuvem)} destino {(historico.FluxoLocalParaNuvem == "S" ? historico.CaminhoArquivoNuvem : historico.CaminhoArquivoLocal)} | FLUXO EXECUTADO: {(historico.FluxoLocalParaNuvem == "S" ? "Do local para nuvem" : "Da nuvem para local")}");
                historico.Situacao = situacaoSincronismo;
                _historicoSincronismoRepositorio.AtualizaHistoricoSincronismo(historico);
                _logServico.Information($"Log do arquivo {nomeArquivoComExtensao} atualizado com sucesso na aplicação de sincronissmo, realizando consulta do historio para continuidade da integração.");
                await BuscaHistoricoSincronismo();
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao tentar atualizar o status do arquivio '{nomeArquivoComExtensao}' na aplicaçao de historico de integração | | FLUXO EXECUTADO: {(historico.FluxoLocalParaNuvem == "S" ? "Do local para nuvem" : "Da nuvem para local")}  ERRO : {ex.Message}");
            }
        }
    }
}
