import { Rotina } from "../rotina/rotina";
import { ConfiguracaoSFTP } from "../configuracao-sftp/configuracao-sftp";

export class EntidadeBase{
    public configuracaoClienteSftp?: ConfiguracaoSFTP;
    public configuracaoEnvioArquivo?: Array<Rotina>[];
}