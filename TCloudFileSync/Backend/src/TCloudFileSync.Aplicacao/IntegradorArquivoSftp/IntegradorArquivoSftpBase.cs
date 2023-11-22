using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Auxiliar;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Enum;
using TCloudFileSync.Aplicacao.Servico;

namespace TCloudFileSync.Aplicacao
{
    public abstract class IntegradorArquivoSftpBase
    {
        #region Dependências e construtor 
        private readonly ILogServico _logServico;
        private readonly ISftpClientServico _sftpClientServico;
        private readonly Notificacao _notificacao;
        private readonly LeitorArquivo _leitorArquivo;
        private readonly IHistoricoSincronismoService _historicoSincronismoService;

        protected IntegradorArquivoSftpBase(ILogServico log, ISftpClientServico sftpClientServico, Notificacao notificacao, LeitorArquivo leitorArquivo, IHistoricoSincronismoService historicoSincronismoService)
        {
            _logServico = log;
            _sftpClientServico = sftpClientServico;
            _notificacao = notificacao;
            _leitorArquivo = leitorArquivo;
            _historicoSincronismoService = historicoSincronismoService;
        }
        #endregion

        #region Manipulação cliente SFTP 
        protected virtual void GeraNovoSftpClient(ConfiguracaoClienteSftp param)
        {
            if (!_sftpClientServico.ClienteSftpJaConfigurado())
            {
                _logServico.Information($"Cliente SFTP ainda não configurado. Criando novo cliente para o host: {param.Host}");
                _sftpClientServico.GeraNovoSftpClient(param);
            }
            else
            {
                var configuracoesClienteAtual = _sftpClientServico.BuscaConfiguracoesAtuais();

                if (!param.Equals(configuracoesClienteAtual))
                {
                    _logServico.Information($"Houve alteração nas configurações do cliente SFTP. Gerando novo cliente ara o host: {param.Host}");
                    _sftpClientServico.GeraNovoSftpClient(param);
                }
            }
        }

        protected virtual void ConectaAoServidorSftp()
        {
            var limite = 5;
            var tentativas = 0;

            while (tentativas <= limite)
            {
                try
                {
                    _logServico.Information("Conectando ao servidor SFTP");
                    _sftpClientServico.Conecta();
                    _logServico.Information("Conexão estabelecida com sucesso");
                    break;
                }
                catch (Exception ex)
                {
                    if (tentativas < limite)
                    {
                        _logServico.Error($"Falha ao conectar ao servidor SFTP: {ex.Message}. Tentando novamente");
                    }
                    else
                    {
                        _notificacao.Adiciona($"Falha ao conectar ao servidor SFTP: {ex.Message}");
                    }

                    tentativas++;
                }
            }
        }

        protected virtual void DesconectaDoServidorSftp()
        {
            try
            {
                _logServico.Information($"Desconectando cliente SFTP do servidor");
                _sftpClientServico.Desconecta();
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao desconectar client do servidor SFTP: {ex.Message}");
            }
        }
        #endregion

        #region Manipulação de arquivos na Nuvem 
        protected virtual async void EnviaArquivoParaNuvem(Stream streamArquivoLocal, ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, ArquivoParaTransferencia arquivo)
        {
            var dateTime = DateTime.Now;

            if (streamArquivoLocal is null)
            {
                _notificacao.Adiciona($"Não foi feito envio do arquivo devido a falha na leitura: {arquivo.NomeComExtensao}, rotina de envio para nuvem stopada");
                return;
            }

            try
            {
                // Insere o historico desse sincronismo
                _logServico.Information($"Inserindo registro do arquivo {arquivo.NomeComExtensao} na aplicação de historico de sincronissmo com status de {SituacaoSincronismo.Enviando} | FLUXO EXECUTADO: LOCAL {arquivo.CaminhoLocal} para NUVEM {arquivo.CaminhoNuvem}");
                double tamanhoEmKB = ValidadorTamanhoArquivo.ValidadorTamanho(arquivo.CaminhoLocal);
                await _historicoSincronismoService.InsereHistoricoSincronismo(configuracaoEnvioArquivo, arquivo.NomeComExtensao, SituacaoSincronismo.Enviando, dateTime, tamanhoEmKB);

                // Informa a ação no log
                _logServico.Information($"Realizando upload assincrono do arquivo {arquivo.NomeComExtensao} para o diretorio {arquivo.CaminhoNuvem} em nuvem");
                _sftpClientServico.Upload(streamArquivoLocal, arquivo.CaminhoDestinoCompleto);
                streamArquivoLocal.Close();

                // Atualiza o registro apos o inserção
                _logServico.Information($"Arquivo local {arquivo.NomeComExtensao} enviado com sucesso para nuvem {arquivo.CaminhoNuvem}");
                _logServico.Information($"Atualizando registro do arquivo {arquivo.NomeComExtensao} na aplicação de historico de sincronissmo para o status {SituacaoSincronismo.Sincronizada}");
                await _historicoSincronismoService.AtualizaHistoricoSincronismo(arquivo.NomeComExtensao, SituacaoSincronismo.Sincronizada, dateTime);
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Falha ao enviar arquivo {arquivo.NomeComExtensao} para a nuvem | FLUXO EXECUTADO: LOCAL {arquivo.CaminhoLocal} para NUVEM {arquivo.CaminhoNuvem}: ERRO | {ex.Message}");
                _notificacao.Adiciona($"Devido a falha no envio do arquivo {arquivo.NomeComExtensao} para nuvem estamos atualizando seu status como {SituacaoSincronismo.Falha} na aplicação de historico de sincronissmo");

                //Em caso de erro o mesmo é apontado como falha
                await _historicoSincronismoService.AtualizaHistoricoSincronismo(arquivo.NomeComExtensao, SituacaoSincronismo.Falha, dateTime);
            }
        }

        protected virtual void ValidaSeDiretorioNaNuvemExiste(string caminhoArquivoNuvem)
        {
            try
            {
                if (!_sftpClientServico.Existe(caminhoArquivoNuvem))
                {
                    _logServico.Information($"Diretório no ambiente na nuvem não encontrada. Criando nova: {caminhoArquivoNuvem}");
                    _sftpClientServico.CriaPasta(caminhoArquivoNuvem);
                }

                _logServico.Information($"Diretório no ambiente na nuvem encontrada: {caminhoArquivoNuvem}");
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao criar pasta no ambiente na nuvem: {caminhoArquivoNuvem}: {ex.Message}");
            }
        }

        protected virtual ArquivoVindoDaNuvem[] BuscaArquivosNoDiretorioNaNuvem(string caminhoArquivoNuvem)
        {
            try
            {
                _logServico.Information($"Buscando arquivos no diretório na nuvem: {caminhoArquivoNuvem}");
                var arquivos = _sftpClientServico.ListaArquivosEmDiretorio(caminhoArquivoNuvem);

                if (arquivos is null || arquivos?.Length == 0)
                {
                    _logServico.Information($"Nenhum arquivo encontrado no diretório na nuvem: {caminhoArquivoNuvem}");
                }

                return arquivos;
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao buscar arquivos no ambiente na nuvem: {caminhoArquivoNuvem}: {ex.Message}");
                return null;
            }
        }

        protected virtual Stream LeArquivoNuvem(string caminhoArquivoNuvem)
        {
            try
            {
                _logServico.Information($"Lendo arquivo no diretório na nuvem: {caminhoArquivoNuvem}");
                return _sftpClientServico.LeArquivo(caminhoArquivoNuvem);
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao ler arquivo no ambiente na nuvem: {caminhoArquivoNuvem}: {ex.Message}");
                return null;
            }
        }

        protected virtual void ApagaArquivoNuvem(ArquivoVindoDaNuvem arquivo)
        {
            try
            {
                _logServico.Information($"Apagando arquivo na nuvem {arquivo.CaminhoNuvemCompleto}");
                _sftpClientServico.ApagaArquivo(arquivo.CaminhoNuvemCompleto);
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao apagar arquivo na nuvem {arquivo.CaminhoNuvemCompleto}: {ex.Message}");
            }
        }
        #endregion

        #region Pasta temporária 
        protected virtual string PreparaPastaTemporaria(ConfiguracaoEnvioArquivo param)
        {
            string caminhoPastaTemporaria = null;
            var nomePastaTemporaria = $"rotina-{param.Id}-temp";

            try
            {
                caminhoPastaTemporaria = _leitorArquivo.PreparaCaminhoArquivoLocal(_leitorArquivo.BuscaNomeDiretorioAtual(), nomePastaTemporaria);
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao preparar pasta temporária: {ex.Message}");
            }

            return caminhoPastaTemporaria;
        }

        protected virtual void ApagaArquivosPastaTemporaria(ArquivoParaTransferencia[] arquivosPastaTemporaria)
        {
            for (var i = 0; i < arquivosPastaTemporaria.Length; i++)
            {
                try
                {
                    _logServico.Information($"Apagando arquivo local {arquivosPastaTemporaria[i].CaminhoLocal}");
                    _leitorArquivo.RemoveArquivo(arquivosPastaTemporaria[i].CaminhoLocal);
                }
                catch (Exception ex)
                {
                    _notificacao.Adiciona($"Erro ao apagar arquivo em diretório local. Arquivo: {arquivosPastaTemporaria[i].CaminhoLocal} | Erro: {ex.Message}");
                }
            }
        }

        protected virtual void LimpaPastaTemporaria(string caminhoPastaTemporaria)
        {
            try
            {
                _logServico.Information($"Deletando arquivos na pasta temporária: {caminhoPastaTemporaria}");
                _leitorArquivo.LimpaPasta(caminhoPastaTemporaria);
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao deletar arquivos de pasta temporária: {caminhoPastaTemporaria}: {ex.Message}");
            }
        }

        protected virtual ArquivoParaTransferencia[] CopiaArquivosParaPastaTemporaria(string caminhoArquivoNuvem, string[] arquivosLocais, string caminhoPastaTemporaria)
        {
            var arquivosPastaTemporaria = new ArquivoParaTransferencia[arquivosLocais.Length];
            var houveAlgumSucesso = false;

            for (var i = 0; i < arquivosLocais.Length; i++)
            {
                try
                {
                    var nomeArquivo = _leitorArquivo.RecuperaNomeDoArquivo(arquivosLocais[i]);

                    if (string.IsNullOrEmpty(nomeArquivo))
                    {
                        _notificacao.Adiciona($"Erro ao recuperar nome do arquivo {arquivosLocais[i]}");
                        continue;
                    }

                    _logServico.Information($"Preparando caminho do arquivo na pasta temporária: Caminho: {caminhoPastaTemporaria} | Arquivo: {nomeArquivo}");
                    var caminhoArquivoPastaTemporaria = _leitorArquivo.PreparaCaminhoArquivoLocal(caminhoPastaTemporaria, nomeArquivo);

                    _logServico.Information($"Copiando arquivo para pasta temporária: {caminhoArquivoPastaTemporaria}");
                    _leitorArquivo.CopiaArquivoParaPastaTemporaria(arquivosLocais[i], caminhoArquivoPastaTemporaria);

                    arquivosPastaTemporaria[i] = new(nomeArquivo, caminhoArquivoPastaTemporaria, caminhoArquivoNuvem, arquivosLocais[i]);

                    houveAlgumSucesso = true;
                }
                catch (Exception ex)
                {
                    _notificacao.Adiciona($"Erro ao copiar arquivo para pasta temporária: Arquivo: {arquivosLocais[i]} | Erro: {ex.Message}");
                }
            }

            if (!houveAlgumSucesso)
            {
                _logServico.Information("Nenhum arquivo copiado para pasta temporária");
            }

            return arquivosPastaTemporaria;
        }
        #endregion

        #region Manipulação arquivo local 
        protected virtual Stream LeArquivo(string arquivoLocal)
        {
            Stream stream = null;

            try
            {
                _logServico.Information($"Lendo arquivo local {arquivoLocal}");
                stream = _leitorArquivo.LeArquivo(arquivoLocal);
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao ler arquivo {arquivoLocal}: {ex.Message}");
            }
            return stream;
        }

        protected virtual void ValidaSePastaLocalExisteSemCriarNova(string caminho)
        {
            try
            {
                _logServico.Information($"Verificando existência de pasta local: {caminho}");

                if (!_leitorArquivo.CaminhoArquivoLocalExiste(caminho))
                {
                    _notificacao.Adiciona($"Pasta local para busca de arquivos não encontrada: {caminho}");
                }
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro validar se pasta local existe: Pasta: {caminho} | Erro: {ex.Message}");
            }
        }

        protected virtual void CriaPastaLocal(string caminho)
        {
            try
            {
                _logServico.Information($"Verificando existência de pasta local: {caminho}");

                if (!_leitorArquivo.CaminhoArquivoLocalExiste(caminho))
                {
                    _logServico.Information($"Pasta local não encontrada. Criando nova");
                    _leitorArquivo.CriaPastaLocal(caminho);
                }
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao preparar pasta local: {ex.Message}");
            }

            if (caminho is null)
            {
                _notificacao.Adiciona($"Pasta local não foi criada");
            }
        }

        protected virtual string[] BuscaArquivosNoDiretorioLocal(string caminhoArquivoLocal)
        {
            string[] retorno = null;

            try
            {
                _logServico.Information($"Buscando arquivos no diretório local {caminhoArquivoLocal}");
                retorno = _leitorArquivo.BuscaArquivosEmDiretorioLocal(caminhoArquivoLocal);

                if (retorno is null || retorno?.Length == 0)
                {
                    _logServico.Information($"Nenhum arquivo encontrado na pasta: {caminhoArquivoLocal}");
                }
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao buscar arquivos em diretório local. Pasta: {caminhoArquivoLocal} | Erro: {ex.Message}");
            }

            return retorno;
        }

        protected virtual void ApagaArquivoCaminhoLocal(string caminhoLocal)
        {
            try
            {
                _logServico.Information($"Deletando arquivo local: {caminhoLocal}");
                _leitorArquivo.RemoveArquivo(caminhoLocal);
            }
            catch (IOException ex)
            {
                _notificacao.Adiciona($"Erro ao deletar arquivo local: {caminhoLocal}: {ex.Message}");
            }
        }

        protected virtual string PreparaCaminhoArquivoLocal(string caminhoArquivoLocal, string nomeComExtensao)
        {
            try
            {
                _logServico.Information($"Preparando caminho do arquivo local: {nomeComExtensao} na pasta {caminhoArquivoLocal}");
                return _leitorArquivo.PreparaCaminhoArquivoLocal(caminhoArquivoLocal, nomeComExtensao);
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao preparar caminho do arquivo local: {nomeComExtensao} na pasta {caminhoArquivoLocal} | Erro: {ex.Message}");
                return null;
            }
        }
        #endregion

        #region Manipulação conteúdo de arquivos (Stream) 
        protected virtual Stream CriaEAbreArquivoParaEscrita(string caminhoCompletoArquivoLocal)
        {
            try
            {
                _logServico.Information($"Criando arquivo local: {caminhoCompletoArquivoLocal}");
                return _leitorArquivo.CriaEAbreArquivoParaEscrita(caminhoCompletoArquivoLocal);
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao criar arquivo local: {caminhoCompletoArquivoLocal} | Erro: {ex.Message}");
                return null;
            }
        }

        protected virtual async Task CopiaConteudo(Stream arquivoOrigem, Stream arquivoDestino, ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, string nomeArquivoComExtensao, string caminhoCompletoArquivoLocal)
        {
            var dateTime = DateTime.Now;

            try
            {
                // Insere o historico desse sincronismo
                _logServico.Information($"Inserindo registro do arquivo {nomeArquivoComExtensao} na aplicação de historico de sincronissmo com status de {SituacaoSincronismo.Enviando} | FLUXO EXECUTADO: NUVEM {configuracaoEnvioArquivo.CaminhoArquivoNuvem} para LOCAL {caminhoCompletoArquivoLocal}");
                double tamanhoEmKB = ValidadorTamanhoArquivo.ValidadorTamanho(caminhoCompletoArquivoLocal);
                await _historicoSincronismoService.InsereHistoricoSincronismo(configuracaoEnvioArquivo, nomeArquivoComExtensao, SituacaoSincronismo.Enviando, dateTime, tamanhoEmKB);

                // Ação de copiar o conteudo ao arquivo
                _logServico.Information($"Realizando download assincrono do arquivo {nomeArquivoComExtensao} da nuvem para arquivo local | FLUXO EXECUTADO: NUVEM {configuracaoEnvioArquivo.CaminhoArquivoNuvem} para LOCAL {caminhoCompletoArquivoLocal} ");
                await _leitorArquivo.CopiaConteudo(arquivoOrigem, arquivoDestino);

                // Atualiza o registro apos o inserção
                _logServico.Information($"Arquivo {nomeArquivoComExtensao} baixado com sucesso da nuvem {configuracaoEnvioArquivo.CaminhoArquivoNuvem}");
                _logServico.Information($"Atualizando registro do arquivo {nomeArquivoComExtensao} na aplicação de historico de sincronissmo para o status {SituacaoSincronismo.Sincronizada}");
                await _historicoSincronismoService.AtualizaHistoricoSincronismo(nomeArquivoComExtensao, SituacaoSincronismo.Sincronizada, dateTime);
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Falha ao baixar arquivo da nuvem {nomeArquivoComExtensao} para a local | FLUXO EXECUTADO: NUVEM {configuracaoEnvioArquivo.CaminhoArquivoNuvem} para LOCAL {caminhoCompletoArquivoLocal}: ERRO | {ex.Message}");
                _notificacao.Adiciona($"Devido a falha no download do arquivo {nomeArquivoComExtensao} para o local estamos atualizando seu status como {SituacaoSincronismo.Falha} na aplicação de historico de sincronissmo");

                //Em caso de erro o mesmo é apontado como falha
                await _historicoSincronismoService.AtualizaHistoricoSincronismo(nomeArquivoComExtensao, SituacaoSincronismo.Falha, dateTime);
            }
        }

        protected virtual void FechaRecursosDeLeituraEscrita(Stream arquivo, bool local)
        {
            var localizacaoArquivo = local ? "local" : "na nuvem";

            try
            {
                _logServico.Information($"Fechando recursos para leitura e escrita {localizacaoArquivo}");
                _leitorArquivo.FechaRecursosDeLeituraEscrita(arquivo);
            }
            catch (Exception ex)
            {
                _notificacao.Adiciona($"Erro ao fechar recursos para leitura e escrita de arquivo {localizacaoArquivo}: {ex.Message}");
            }
        }
        #endregion

        public bool VerificaSeArquivoExiste(string nomeArquivoNuvem, string caminhoArquivoNuvem)
        {
            try
            {
                _logServico.Information($"Verificando se existe arquivo na nuvem Arquivo processado {nomeArquivoNuvem} caminho na nuvem {caminhoArquivoNuvem}");
                return _sftpClientServico.VerificaSeArquivoExisteEmNuvem(caminhoArquivoNuvem, nomeArquivoNuvem);
            }
            catch (Exception ex)
            {
                _logServico.Information($"Ocorreu um erro na execução da validação em nuvem Arquivo processado {nomeArquivoNuvem} caminho na nuvem {caminhoArquivoNuvem} | ERRO: {ex.Message}");
                return false;
            }
        }

        public string RecuperaNomeDoArquivo(string caminhoArquivo)
        {
            return Path.GetFileName(caminhoArquivo);
        }
    }
}
