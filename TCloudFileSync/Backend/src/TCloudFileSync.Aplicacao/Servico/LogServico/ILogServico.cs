using Serilog;

namespace TCloudFileSync.Aplicacao.Servico
{
    /// <summary>
    /// Serviço utilizado para abstrair funcionalidades de incluir logs 
    /// <br>Utilizado tanto para logs em texto quanto para logs em banco de dados </br>
    /// </summary>
    public interface ILogServico
    {
        /// <summary>
        /// Informa mensagem de erro 
        /// </summary>
        void Error(string mensagem); 

        /// <summary>
        /// Informa mensagem de log padrão 
        /// </summary>
        void Information(string mensagem);

        /// <summary>
        /// Informa ensagem de log padrão e salva em banco de dados
        /// </summary>
        void EncerraRotina(string mensagem, bool houveErro = false);

        /// <summary>
        /// Configura prefixo para mensagens de log 
        /// </summary>
        void SetaPrefixo(string prefixo);

        /// <summary>
        /// Parametriza objeto do tipo ILogger para ser utilizado por serviço de log 
        /// </summary>
        void SetaLog(ILogger log);
    }
}
