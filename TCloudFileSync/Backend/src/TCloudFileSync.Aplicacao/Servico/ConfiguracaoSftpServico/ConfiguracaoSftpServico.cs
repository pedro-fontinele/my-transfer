using Serilog;
using System;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Configuracoes;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Enum;
using TCloudFileSync.Aplicacao.Repositorio;

namespace TCloudFileSync.Aplicacao.Servico
{
    public class ConfiguracaoSftpServico : IConfiguracaoSftpServico
    {
        private readonly ILogger _log;
        private readonly Notificacao _notificacao;
        private readonly IConfiguracaoSftpRepositorio _configuracaoSftpRepositorio;

        public ConfiguracaoSftpServico(Notificacao notificacao, IConfiguracaoSftpRepositorio configuracaoSftpRepositorio)
        {
            _log = LogConfiguracao.CriaLogger(TipoLogger.GERENCIAMENTO);
            _notificacao = notificacao;
            _configuracaoSftpRepositorio = configuracaoSftpRepositorio;
        }

        public EnviaArquivoDto BuscaConfiguracoesSftp()
        {
            try
            {
                _log.Information("Buscando configurações para integração de arquivos");
                return _configuracaoSftpRepositorio.BuscaConfiguracoesSftp();
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao buscar configurações da base de dados: {ex.Message}");
                return null;
            }
        }

        public async Task<EnviaArquivoDto> InsereOuAtualizaConfiguracaoSftp(ConfiguracaoClienteSftp configuracaoClienteSftp)
        {
            try
            {
                var configuracao = _configuracaoSftpRepositorio.BuscaConfiguracoesSftp();
                if (configuracao == null)
                {
                    _log.Information("Realizando inserção do registro no banco de dados.");
                    await _configuracaoSftpRepositorio.InsereConfiguracaoSftp(configuracaoClienteSftp);
                }
                else
                {
                    _log.Information("Realizando atualização do registro no banco de dados.");
                    await _configuracaoSftpRepositorio.AtualizaConfiguracaoSftp(configuracaoClienteSftp);
                }

                return _configuracaoSftpRepositorio.BuscaConfiguracoesSftp();
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao buscar configurações da base de dados: {ex.Message}");
                return null;
            }
        }
    }
}
