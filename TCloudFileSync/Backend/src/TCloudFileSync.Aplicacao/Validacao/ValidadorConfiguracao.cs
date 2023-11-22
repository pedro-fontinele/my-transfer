using TCloudFileSync.Aplicacao.Dto;

namespace TCloudFileSync.Aplicacao.Validacao
{
    public static class ValidadorConfiguracao
    {
        public static void Valida(ConfiguracaoClienteSftp param, Notificacao notificacao)
        {
            if (string.IsNullOrWhiteSpace(param.Host))
            {
                notificacao.Adiciona($"Host para conexão SFTP não informado: {param.Host}");
            }
            if (param.Port == default)
            {
                notificacao.Adiciona($"Porta do Host para conexão SFTP inválida: {param.Port}");
            }
            if (string.IsNullOrWhiteSpace(param.Username))
            {
                notificacao.Adiciona($"Username para conexão SFTP não informado: {param.Username}");
            }
            if (string.IsNullOrWhiteSpace(param.Password))
            {
                notificacao.Adiciona($"Password para conexão SFTP não informado: {param.Password}");
            }
        }

        public static void Valida(ConfiguracaoEnvioArquivo param, Notificacao notificacao)
        {
            if (string.IsNullOrWhiteSpace(param.CaminhoArquivoLocal))
            {
                notificacao.Adiciona($"Caminho do arquivo local inválido: {param.CaminhoArquivoLocal}");
            }
            if (string.IsNullOrWhiteSpace(param.CaminhoArquivoNuvem))
            {
                notificacao.Adiciona($"Caminho do arquivo na nuvem inválido: {param.CaminhoArquivoNuvem}");
            }
        }
    }
}
