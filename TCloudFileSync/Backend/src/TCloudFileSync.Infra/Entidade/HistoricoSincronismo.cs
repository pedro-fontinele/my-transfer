namespace TCloudFileSync.Infra.Entidade
{
    public class HistoricoSincronismo : Entidade
    {
        public string Arquivo { get; set; }
        public string FluxoLocalParaNuvem { get; set; }
        public string CaminhoArquivoLocal { get; set; }
        public string CaminhoArquivoNuvem { get; set; }
        public int Tamanho { get; set; }
        public string DtaMovimento { get; set; }
        public string HorMovimento { get; set; }
        public string Situacao { get; set; }
    }

}
