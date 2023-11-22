using Microsoft.EntityFrameworkCore;
using System.IO;
using TCloudFileSync.Aplicacao.Configuracoes;
using TCloudFileSync.Aplicacao.Enum;
using TCloudFileSync.Aplicacao.Servico;
using TCloudFileSync.Infra.Contexto;

namespace TCloudFileSync.Infra.Configuracoes
{
    public class ContextoConfiguracao
    {
        private readonly SqliteContexto _contexto;
        private readonly ILogServico _log;

        public ContextoConfiguracao(SqliteContexto contexto, ILogServico log)
        {
            _contexto = contexto;
            _log = log;
            _log.SetaLog(LogConfiguracao.CriaLogger(TipoLogger.CONTEXTO));
        }

        public void PreparaBaseDeDados()
        {
            _log.Information($"Validando existência do arquivo {_contexto.CaminhoArquivoDB}");

            if (!File.Exists(_contexto.CaminhoArquivoDB))
            {
                _log.Information($"Arquivo não encontrado. Criando diretório: {_contexto.DiretorioDB}");
                Directory.CreateDirectory(_contexto.DiretorioDB);
            }

            _log.Information($"Realizando migration");
            _contexto.Database.Migrate();

            _log.Information($"Migration concluída");            
        }
    }
}
