export abstract class ColunasTabela{
    public static colunasTabelaConfiguracaoSftp: Array<any> = [
        { property: 'fluxoLocalParaNuvem', width: '30%', label: 'Sentido de sincronismo', type: 'label', labels: [
            { value: false, color: 'color-02', label: 'Da nuvem para local' },
            { value: true, color: 'color-12', label: 'Do local para nuvem' }
        ]},
        { property: 'caminhoArquivoLocal', width: '30%' , label: 'Caminho Local'},
        { property: 'caminhoArquivoNuvem', width: '28%', label: 'Caminho Nuvem' },
        { property: 'ativo', width: '10%', label: 'Ativo', type:'label',   labels: [
             { value: false, color: 'color-07', label: 'Nao' },
             { value: true, color: 'color-11', label: 'Sim' }
        ]}
    ]

    public static colunasTabelaHistoricoSincronismo: any[] = [
        { property: 'arquivo', width: '10%' , label: 'Arquivo'},
        { property: 'fluxoLocalParaNuvem', width: '15%', label: 'Sentido de sincronismo', type: 'label', labels: [
            { value: 'N', color: 'color-02', label: 'Da nuvem para local' },
            { value: 'S', color: 'color-11', label: 'Do local para nuvem' }
        ]},
        { property: 'caminhoArquivoLocal', width: '15%' , label: 'Caminho Local'},
        { property: 'caminhoArquivoNuvem', width: '15%', label: 'Caminho Nuvem' },
        { property: 'tamanho', width: '10%', label: 'Tamanho KB' },
        { property: 'dtaMovimento', width: '10%', label: 'Data' },
        { property: 'horMovimento', width: '10%', label: 'Hora' },
        {  property: 'situacao', width: '10%', label: 'Situação', type: 'label', labels: [
            { value: 'FALHA', color: 'color-08', label: 'FALHA' },
            { value: 'ENVIANDO', color: 'color-02', label: 'ENVIANDO' },
            { value: 'SINCRONIZADA', color: 'color-11', label: 'SINCRONIZADA' }
        ]}
    ]
}