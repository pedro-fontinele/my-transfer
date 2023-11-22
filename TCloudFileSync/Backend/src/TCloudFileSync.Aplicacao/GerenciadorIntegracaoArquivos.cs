using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using TCloudFileSync.Aplicacao.Configuracoes;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Enum;
using TCloudFileSync.Aplicacao.Repositorio;
using TCloudFileSync.Aplicacao.Servico;
using TCloudFileSync.Aplicacao.Validacao;
 
namespace TCloudFileSync.Aplicacao
{
    public class GerenciadorIntegracaoArquivos
    {
        private readonly List<IIntegradorArquivoSftp> lista = new();

        private readonly ILogger _log;
        private readonly Notificacao _notificacao;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguracaoSftpServico _configuracaoSftpServico;

        public GerenciadorIntegracaoArquivos(Notificacao notificacao, IServiceProvider serviceProvider, IConfiguracaoSftpServico configuracaoSftpServico)
        {
            _log = LogConfiguracao.CriaLogger(TipoLogger.GERENCIAMENTO);
            _notificacao = notificacao;
            _serviceProvider = serviceProvider;
            _configuracaoSftpServico = configuracaoSftpServico;
        }

        public void RealizaIntegracaoArquivosViaSftp()
        {
            _log.Information("Buscando configurações para realizar integrações");

            // busca as configurações para execução de rotinas de integração registradas em banco de dados 
            var param = _configuracaoSftpServico.BuscaConfiguracoesSftp();

            if (!_notificacao.HouveNotificacao() && param != null)
            {
                // valida configurações de SFTP. Caso haja problemas, mantém fluxo para validar também configurações de envio de arquivo
                ValidadorConfiguracao.Valida(param.ConfiguracaoClienteSftp, _notificacao);

                foreach (var envioArquivo in param.ConfiguracaoEnvioArquivo)
                {
                    // valida configurações de envio de arquivo 
                    ValidadorConfiguracao.Valida(envioArquivo, _notificacao);

                    // caso haja problemas nas validações de configurações de SFTP ou envio de arquivo, 
                    // não conclui loop mas continua validando configurações de envio de arquivo 
                    // para exibição de todoso os erros encontrados ao final do processo 
                    if (_notificacao.HouveNotificacao())
                    {
                        continue;
                    }

                    // verifica se já há integração em execução 
                    var servicoEmExecucao = lista.FirstOrDefault(x => x.BuscaId() == envioArquivo.Id);

                    // rotina de integração já em execução. 
                    if (servicoEmExecucao is not null)
                    {
                        // busca configurações da rotina em execução. Caso haja diferença nos dados 
                        // emite uma notificação para interromper processo atual 
                        // e atualiza configurações 
                        AtualizaConfiguracoesDeRotinaEmExecucao(servicoEmExecucao, envioArquivo, param.ConfiguracaoClienteSftp);
                    }
                    else
                    {
                        // caso não haja rotina de integração em execução 
                        // gera nova integração em thread específica 
                        // e armazena referência da rotina em objeto List<IIntegradorArquivoSftp> 
                        CriaNovoFluxoDeIntegracao(envioArquivo, param.ConfiguracaoClienteSftp);
                    }
                }
            }

            if (_notificacao.HouveNotificacao())
            {
                _log.Error($"Erros no processo iniciar integração de arquivos: {_notificacao.ExibeNotificacoes()}");
                _notificacao.LimpaNotificacoes();
            }
            else
            {
                _log.Information("Configuração de rotinas de envio de arquivos realizada com sucesso");
            }
        }

        protected virtual void CriaNovoFluxoDeIntegracao(ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, ConfiguracaoClienteSftp configuracaoClienteSftp)
        {
            _log.Information($"Integração com ID {configuracaoEnvioArquivo.Id} sendo iniciada");

            var servico = GeraNovoEscopoDeIntegracao(configuracaoEnvioArquivo, configuracaoClienteSftp);

            var thread = new Thread(async () => await servico.IniciaFluxoDeIntegracao());

            lista.Add(servico);

            thread.Start();
        }

        protected virtual void AtualizaConfiguracoesDeRotinaEmExecucao(IIntegradorArquivoSftp servicoEmExecucao, ConfiguracaoEnvioArquivo envioArquivo, ConfiguracaoClienteSftp configuracaoClienteSftp)
        {
            // busca configurações atuais da rotina em execução 
            var (envioArquivoAtual, clienteSftpAtual) = servicoEmExecucao.BuscaConfiguracoes();

            // verifica se houve alguma alteração em qualquer dos campos das configurações 
            if (!envioArquivoAtual.Equals(envioArquivo) || !clienteSftpAtual.Equals(configuracaoClienteSftp))
            {
                _log.Information($"Integração com ID {envioArquivo.Id} já em execução. Foram encontradas alterações nas configurações. Aplicando alterações");

                // se houve alguma alteração válida nas configurações, interrompe processo de integração 
                // e aplica configurações na rotina em execução 
                // próxima execução da rotina ocorrerá com as novas configurações 
                servicoEmExecucao.GeraNotificacaoParaInterromperProcesso();
                servicoEmExecucao.AplicaConfiguracoes(envioArquivo, configuracaoClienteSftp);
            }
        }

        protected virtual IIntegradorArquivoSftp GeraNovoEscopoDeIntegracao(ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, ConfiguracaoClienteSftp configuracaoClienteSftp)
        {
            var scope = _serviceProvider.CreateScope();

            // gera logger com id da rotina no nome do arquivo 
            var servicoLog = scope.ServiceProvider.GetService<ILogServico>();
            servicoLog.SetaLog(LogConfiguracao.CriaLogger(configuracaoEnvioArquivo.Id));

            var servico = scope.ServiceProvider.GetService<IIntegradorArquivoSftp>();

            servico.AplicaConfiguracoes(configuracaoEnvioArquivo, configuracaoClienteSftp);

            return servico;
        }
    }
}
