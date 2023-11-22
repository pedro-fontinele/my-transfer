using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Infra.Entidade;

namespace TCloudFileSync.Infra.Auxiliar
{
    public static class EntidadeConversor
    {
        public static ConfiguracaoEnvioArquivo Converte(Rotina entidade)
        {
            return new ConfiguracaoEnvioArquivo
            {
                Ativo = entidade.Ativo == "S",
                CaminhoArquivoLocal = entidade.CaminhoArquivoLocal,
                CaminhoArquivoNuvem = entidade.CaminhoArquivoNuvem,
                Id = entidade.Id,
                TempoParaIniciarProximoEnvio = entidade.TempoParaIniciarProximoEnvio,
                DeveApagarArquivoOrigem = entidade.DeveApagarArquivoOrigem == "S",
                FluxoLocalParaNuvem = entidade.FluxoLocalParaNuvem == "S",
                IdConfiguracaoSftp = entidade.IdConfiguracaoSftp
            };
        }

        public static void ConverteConsulta(ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, Rotina rotina)
        {
            rotina.Ativo = configuracaoEnvioArquivo.Ativo ? "S" : "N";
            rotina.Id = configuracaoEnvioArquivo.Id;
            rotina.CaminhoArquivoLocal = configuracaoEnvioArquivo.CaminhoArquivoLocal;
            rotina.CaminhoArquivoNuvem = configuracaoEnvioArquivo.CaminhoArquivoNuvem;
            rotina.TempoParaIniciarProximoEnvio = configuracaoEnvioArquivo.TempoParaIniciarProximoEnvio;
            rotina.DeveApagarArquivoOrigem = configuracaoEnvioArquivo.DeveApagarArquivoOrigem ? "S" : "N";
            rotina.FluxoLocalParaNuvem = configuracaoEnvioArquivo.FluxoLocalParaNuvem ? "S" : "N";
            rotina.IdConfiguracaoSftp = configuracaoEnvioArquivo.IdConfiguracaoSftp;
        }
    }
}
