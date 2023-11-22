namespace TCloudFileSync.Infra.Entidade
{
    public class Rotina : Entidade
    {
        public string Ativo { get; set; }
        public string CaminhoArquivoLocal { get; set; }
        public string CaminhoArquivoNuvem { get; set; }
        public int TempoParaIniciarProximoEnvio { get; set; }
        public string DeveApagarArquivoOrigem { get; set; }
        public string FluxoLocalParaNuvem { get; set; }

        public int IdConfiguracaoSftp { get; set; }
        public ConfiguracaoSftp ConfiguracaoSftp { get; set; }
    }
}
