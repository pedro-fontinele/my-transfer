using Serilog;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Auxiliar;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Repositorio;
using TCloudFileSync.Aplicacao.Servico;

namespace TCloudFileSync.Aplicacao
{
    public partial class IntegradorArquivoSftp : IntegradorArquivoSftpBase, IIntegradorArquivoSftp
    {
        #region Configurações 
        private ConfiguracaoEnvioArquivo configuracaoEnvioArquivo;
        private ConfiguracaoClienteSftp configuracaoClienteSftp;
        public (ConfiguracaoEnvioArquivo, ConfiguracaoClienteSftp) BuscaConfiguracoes() => (configuracaoEnvioArquivo, configuracaoClienteSftp);

        public void AplicaConfiguracoes(ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, ConfiguracaoClienteSftp configuracaoClienteSftp)
        {
            this.configuracaoEnvioArquivo = configuracaoEnvioArquivo;
            this.configuracaoClienteSftp = configuracaoClienteSftp;
        }

        public int BuscaId() => configuracaoEnvioArquivo.Id;
        public void GeraNotificacaoParaInterromperProcesso() => _log.Information("*** Processo sendo interrompido por alteração encontrada nas configurações ***");
        #endregion

        #region Dependências e construtor 
        private readonly Notificacao _notificacao;
        private readonly ILogServico _log;

        public IntegradorArquivoSftp(ISftpClientServico sftpClientServico, ILogServico log, Notificacao notificacao, LeitorArquivo leitorArquivo, IHistoricoSincronismoService historicoSincronismoService)
            : base(log, sftpClientServico, notificacao, leitorArquivo, historicoSincronismoService)
        {
            _notificacao = notificacao;
            _log = log;
        }
        #endregion

        /// <summary>
        /// Inicia integração com base nas configurações aplicadas 
        /// </summary>
        public async Task IniciaFluxoDeIntegracao()
        {
            _log.SetaPrefixo($"ID {configuracaoEnvioArquivo.Id}");

            while (true)
            {
                if (configuracaoEnvioArquivo.FluxoLocalParaNuvem)
                {
                    _log.Information("Iniciando integração de arquivos em ambiente local para nuvem");
                    this.RealizaIntegracaoArquivoLocalParaNuvem(configuracaoEnvioArquivo, configuracaoClienteSftp);
                }
                else
                {
                    _log.Information("Iniciando integração de arquivos em ambiente nuvem para local");
                    await this.RealizaIntegracaoArquivoNuvemParaLocal(configuracaoEnvioArquivo, configuracaoClienteSftp);
                }

                if (_notificacao.HouveNotificacao())
                {
                    _log.EncerraRotina($"Erros no processo de envio de arquivos: {_notificacao.ExibeNotificacoes()}", true);
                }
                else
                {
                    _log.EncerraRotina("Fluxo de integração concluído sem erros");
                }

                _notificacao.LimpaNotificacoes();

                _log.Information($"Aguardando periodo de {configuracaoEnvioArquivo.TempoParaIniciarProximoEnvio} milissegundos para realizar próxima integração");
                Thread.Sleep(configuracaoEnvioArquivo.TempoParaIniciarProximoEnvio);
            }
        }

        #region Fluxo de integração: local -> nuvem 
        /// <summary>
        /// Realiza processo de envio de arquivos locais para nuvem 
        /// <br>
        /// Método não lança exceptions. Caso ocorra algum erro, o mesmo será capturado no objeto Notificacao 
        /// e o processo de envio será interrompido sem erros 
        /// </br>
        /// </summary>
        public void RealizaIntegracaoArquivoLocalParaNuvem(ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, ConfiguracaoClienteSftp configuracaoClienteSftp)
        {
            if (!configuracaoEnvioArquivo.Ativo)
            {
                _log.Information($"Integração de id {configuracaoEnvioArquivo.Id} se encontra inativa");
                return;
            }

            var caminhoPastaTemporaria = PreparaPastaTemporaria(configuracaoEnvioArquivo);

            CriaPastaLocal(caminhoPastaTemporaria);

            if (_notificacao.HouveNotificacao())
            {
                _log.Information($"Encontrado erros ao criar pasta temporaria, reinicando o processo de integração.");
                return;
            }

            LimpaPastaTemporaria(caminhoPastaTemporaria);

            if (_notificacao.HouveNotificacao())
            {
                _log.Information($"Encontrado erros ao limpar a pastar temporaria, reinicando o processo de integração.");
                return;
            }

            ValidaSePastaLocalExisteSemCriarNova(configuracaoEnvioArquivo.CaminhoArquivoLocal);

            if (_notificacao.HouveNotificacao())
            {
                _log.Information($"Encontrado erros ao validar se a pasta local ja existe, reinicando o processo de integração.");
                return;
            }

            string[] arquivosEncontrados = BuscaArquivosNoDiretorioLocal(configuracaoEnvioArquivo.CaminhoArquivoLocal);

            if (_notificacao.HouveNotificacao())
            {
                _log.Information($"Encontrado erros na leitura dos arquivos no diretorio local, reinicando o processo de integração.");
                return;
            }

            var arquivosPastaTemporaria = CopiaArquivosParaPastaTemporaria(configuracaoEnvioArquivo.CaminhoArquivoNuvem, arquivosEncontrados, caminhoPastaTemporaria);

            if (_notificacao.HouveNotificacao())
            {
                _log.Information($"Encontrado erros no copiar arquivo para pasta temporaria, deletando a mesma para reinicalização do processo.");
                ApagaArquivosPastaTemporaria(arquivosPastaTemporaria);
                return;
            }

            GeraNovoSftpClient(configuracaoClienteSftp);

            ConectaAoServidorSftp();

            ValidaSeDiretorioNaNuvemExiste(configuracaoEnvioArquivo.CaminhoArquivoNuvem);

            if (_notificacao.HouveNotificacao())
            {
                _log.Information($"Encontrado erros na leitura do diretorio nuvem o mesmo não existe, reinicando o processo de integração.");
                ApagaArquivosPastaTemporaria(arquivosPastaTemporaria);
                DesconectaDoServidorSftp();
                return;
            }

            foreach (var arquivo in arquivosPastaTemporaria)
            {
                using var streamArquivoLocal = LeArquivo(arquivo.CaminhoPastaTemporaria);

                EnviaArquivoParaNuvem(streamArquivoLocal, configuracaoEnvioArquivo, arquivo);

                if (configuracaoEnvioArquivo.DeveApagarArquivoOrigem && !_notificacao.HouveNotificacao())
                {
                    var existeArquivoNaNuvem = VerificaSeArquivoExiste(arquivo.NomeArquivoNuvem, configuracaoEnvioArquivo.CaminhoArquivoNuvem);
                    if (existeArquivoNaNuvem)
                    {
                        _log.Information($"Confirmado que o arquivo {arquivo.NomeArquivoNuvem} foi processado e esta no diretorio da nuvem {configuracaoEnvioArquivo.CaminhoArquivoNuvem} deletando o mesmo da pasta temporaria");
                        ApagaArquivoCaminhoLocal(arquivo.CaminhoLocal);
                    }
                    else
                    {
                        _log.Information($"Arquivo não encontrado no diretorio da nuvem. Arquivo esperado {arquivo.NomeArquivoNuvem}");
                    }
                }

                ApagaArquivoCaminhoLocal(arquivo.CaminhoPastaTemporaria);
            }

            DesconectaDoServidorSftp();
        }

        #endregion

        #region Fluxo de integração: nuvem -> local 
        /// <summary>
        /// Realiza processo de envio de arquivos em ambiente na nuvem para ambiente local 
        /// <br>
        /// Método não lança exceptions. Caso ocorra algum erro, o mesmo será capturado no objeto Notificacao 
        /// e o processo de envio será interrompido sem erros 
        /// </br>
        /// </summary>
        public async Task RealizaIntegracaoArquivoNuvemParaLocal(ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, ConfiguracaoClienteSftp configuracaoClienteSftp)
        {
            if (!configuracaoEnvioArquivo.Ativo)
            {
                _log.Information($"Integração de id {configuracaoEnvioArquivo.Id} se encontra inativa");
                return;
            }

            CriaPastaLocal(configuracaoEnvioArquivo.CaminhoArquivoLocal);

            if (_notificacao.HouveNotificacao())
            {
                _log.Information($"Encontrado erros ao criar pasta local, reinicando o processo de integração.");
                return;
            }

            GeraNovoSftpClient(configuracaoClienteSftp);

            ConectaAoServidorSftp();

            ValidaSeDiretorioNaNuvemExiste(configuracaoEnvioArquivo.CaminhoArquivoNuvem);

            if (_notificacao.HouveNotificacao())
            {
                _log.Information($"Encontrado erros na leitura do diretorio nuvem o mesmo não existe, reinicando o processo de integração.");
                DesconectaDoServidorSftp();
                return;
            }

            var arquivosEncontrados = BuscaArquivosNoDiretorioNaNuvem(configuracaoEnvioArquivo.CaminhoArquivoNuvem);

            if (_notificacao.HouveNotificacao())
            {
                _log.Information($"Encontrado erros busca dos arquivos na nuvem, reinicando o processo de integração.");
                DesconectaDoServidorSftp();
                return;
            }

            foreach (var arquivo in arquivosEncontrados)
            {
                var arquivoBaixado = LeArquivoNuvem(arquivo.CaminhoNuvemCompleto);

                if (_notificacao.HouveNotificacao())
                {
                    if (arquivoBaixado is not null)
                    {
                        FechaRecursosDeLeituraEscrita(arquivoBaixado, false);
                    }

                    _log.Information($"Encontrado erros na leitura dos arquivos na nuvem, reinicando o processo de integração.");
                    DesconectaDoServidorSftp();
                    return;
                }

                var caminhoCompletoArquivoLocal = PreparaCaminhoArquivoLocal(configuracaoEnvioArquivo.CaminhoArquivoLocal, arquivo.NomeComExtensao);

                if (_notificacao.HouveNotificacao())
                {
                    _log.Information($"Encontrado erros no retorno do caminho local dos arquivos, reinicando o processo de integração.");
                    FechaRecursosDeLeituraEscrita(arquivoBaixado, false);
                    DesconectaDoServidorSftp();
                    return;
                }

                var arquivoLocal = CriaEAbreArquivoParaEscrita(caminhoCompletoArquivoLocal);

                if (_notificacao.HouveNotificacao())
                {
                    if (arquivoLocal is not null)
                    {
                        FechaRecursosDeLeituraEscrita(arquivoLocal, true);
                    }

                    _log.Information($"Encontrado erros na criaçao do conteudo dos arquivos.");
                    FechaRecursosDeLeituraEscrita(arquivoBaixado, false);
                    DesconectaDoServidorSftp();
                    return;
                }

                await CopiaConteudo(arquivoBaixado, arquivoLocal, configuracaoEnvioArquivo, arquivo.NomeComExtensao, caminhoCompletoArquivoLocal);

                FechaRecursosDeLeituraEscrita(arquivoBaixado, false);
                FechaRecursosDeLeituraEscrita(arquivoLocal, true);

                if (configuracaoEnvioArquivo.DeveApagarArquivoOrigem && !_notificacao.HouveNotificacao())
                {
                    string[] arquivosEncontradosLocal = BuscaArquivosNoDiretorioLocal(configuracaoEnvioArquivo.CaminhoArquivoLocal);
                    if (arquivosEncontradosLocal != null)
                    {
                        for (var i = 0; i < arquivosEncontradosLocal.Length; i++)
                        {
                            var nomeArquivo = RecuperaNomeDoArquivo(arquivosEncontradosLocal[i]);
                            if (nomeArquivo == arquivo.NomeComExtensao)
                            {
                                _log.Information($"Confirmado que o arquivo {arquivo.NomeComExtensao} foi devidamente baixado do diretirio em nuvem {configuracaoEnvioArquivo.CaminhoArquivoNuvem} deletando o mesmo da nuvem mediante confirmacao da operacao.");
                                _log.Information($"Agente TCloud SFTP, marcado para deletar arquivo da origem");
                                ApagaArquivoNuvem(arquivo);
                            }
                        }
                    }
                    else
                    {
                        _log.Information($"Não foram encontrados arquivos no diretorio local apos o downloadn, reinicando o processo.");
                    }
                }
            }

            DesconectaDoServidorSftp();
        }
        #endregion
    }
}
