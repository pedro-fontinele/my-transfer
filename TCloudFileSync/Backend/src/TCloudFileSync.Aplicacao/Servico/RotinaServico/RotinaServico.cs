using Serilog;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Repositorio;

namespace TCloudFileSync.Aplicacao.Servico
{
    public class RotinaServico : IRotinaServico
    {
        private readonly Notificacao _notificacao;        
        private readonly IRotinaRepositorio _rotinaRepositorio;
        private readonly IConfiguracaoSftpRepositorio _configuracaoSftpRepositorio;

        public RotinaServico(Notificacao notificacao, IRotinaRepositorio rotinaRepositorio, IConfiguracaoSftpRepositorio configuracaoSftpRepositorio)
        {
            _notificacao = notificacao;
            _rotinaRepositorio = rotinaRepositorio;
            _configuracaoSftpRepositorio = configuracaoSftpRepositorio;
        }

        public List<ConfiguracaoEnvioArquivo> BuscaTodasAsRotinas()
        {
            try
            {
                return _rotinaRepositorio.BuscaTodasAsRotinas();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ConfiguracaoEnvioArquivo BuscaRotinaPorId(int id)
        {
            try
            {
                return _rotinaRepositorio.BuscaRotinaPorId(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<EnviaArquivoDto> InsereOuAtualizaRotina(ConfiguracaoEnvioArquivo  consultaConfiguracaoEnvioArquivo)
        {
            try
            {
                if (consultaConfiguracaoEnvioArquivo == null) return null;

                var rotina = _rotinaRepositorio.BuscaRotinaPorId(consultaConfiguracaoEnvioArquivo.Id);
                if (rotina == null)
                {
                     await _rotinaRepositorio.InsereRotina(consultaConfiguracaoEnvioArquivo);
                }
                else
                {
                    await _rotinaRepositorio.AtualizaRotina(consultaConfiguracaoEnvioArquivo);
                }

                return _configuracaoSftpRepositorio.BuscaConfiguracoesSftp();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
