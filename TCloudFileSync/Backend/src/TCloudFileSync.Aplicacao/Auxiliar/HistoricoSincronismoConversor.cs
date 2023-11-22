using System;
using TCloudFileSync.Aplicacao.Dto;

public static class HistoricoSincronismoConverter
{
    public static HistoricoSincronismoDto Converter(ConfiguracaoEnvioArquivo configuracaoEnvioArquivo, string nomeArquivoComExtensao, string situacao, DateTime dateTime, double tamanhoEmKB)
    {
        var dtaMovimento = dateTime.ToString("dd/MM/yyyy");
        var horMovimento = dateTime.ToString("HH:mm:ss");

        return new HistoricoSincronismoDto
        {
            Arquivo = nomeArquivoComExtensao,
            FluxoLocalParaNuvem = configuracaoEnvioArquivo.FluxoLocalParaNuvem ? "S" : "N",
            CaminhoArquivoLocal = configuracaoEnvioArquivo.CaminhoArquivoLocal,
            CaminhoArquivoNuvem = configuracaoEnvioArquivo.CaminhoArquivoNuvem,
            Tamanho = (int)tamanhoEmKB,
            DtaMovimento = dtaMovimento,
            HorMovimento = horMovimento,
            Situacao = situacao
        };
    }
}
