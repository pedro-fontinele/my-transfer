export class Rotina{
    public id?: number;
    public ativo?: any;
    public caminhoArquivoLocal?: string;
    public caminhoArquivoNuvem?: string;
    public tempoParaIniciarProximoEnvio?: number;
    public deveApagarArquivoOrigem?: any;
    public fluxoLocalParaNuvem?: any;
    public idConfiguracaoSftp?: number;
}