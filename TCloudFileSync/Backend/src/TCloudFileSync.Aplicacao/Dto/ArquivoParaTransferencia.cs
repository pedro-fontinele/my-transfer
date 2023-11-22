namespace TCloudFileSync.Aplicacao.Dto
{
    public record ArquivoParaTransferencia(string NomeComExtensao, string CaminhoPastaTemporaria, string CaminhoNuvem, string CaminhoLocal)
    {
        public string NomeArquivoNuvem { get; init; } = NomeComExtensao;
        public string CaminhoDestinoCompleto { get; init; } = CaminhoNuvem + "/"+ NomeComExtensao;
    }

    public record ArquivoVindoDaNuvem(string NomeComExtensao, string CaminhoNuvemCompleto);
}
