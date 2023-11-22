using Serilog;

namespace TCloudFileSync.Aplicacao.Servico
{
    public class LogServico : ILogServico
    {
        private ILogger _log;
        private string prefixo;

        public virtual void SetaLog(ILogger log)
        {
            _log = log;
        }

        public virtual void Information(string mensagem)
        {
            _log.Information($"{prefixo}{mensagem}");
        }
        public virtual void Error(string mensagem)
        {
            _log.Error($"{prefixo}{mensagem}");
        }

        public virtual void EncerraRotina(string mensagem, bool houveErro)
        {
            // TODO (HOOK) salvar log em banco de dados
            //_log.Warning("ATENÇÃO - implementar envio de SUCESSO ou ERRO para base de dados");

            if (!houveErro)
            {
                _log.Information($"{prefixo}{mensagem}");
            }
            else
            {
                _log.Error($"{prefixo}{mensagem}");
            }
        }

        public virtual void SetaPrefixo(string prefixo) => this.prefixo = $"{prefixo} ";
    }
}
