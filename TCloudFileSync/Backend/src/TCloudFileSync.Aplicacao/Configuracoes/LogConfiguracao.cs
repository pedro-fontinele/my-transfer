using Serilog;
using System.IO;
using System;
using TCloudFileSync.Aplicacao.Enum;

namespace TCloudFileSync.Aplicacao.Configuracoes
{
    public static class LogConfiguracao
    {
        public static ILogger CriaLogger(int id)
        {
            var diretorioAtual = Path.GetDirectoryName(AppContext.BaseDirectory);
            var diretorioLog = Path.Join(diretorioAtual, "_logs");

            var logger = new LoggerConfiguration()
                .Enrich.With(new AdicionaThreadIdEmLog())
                .WriteTo.Console()
                .WriteTo.File($"{diretorioLog}/log-rotina-id-{id}_.txt", 
                    rollingInterval: RollingInterval.Month, 
                    outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fffff} (Processo {ThreadID}) {Message:lj}{NewLine}{Exception}"
                    ) 
                .CreateLogger();

            return logger;
        }

        public static ILogger CriaLogger(TipoLogger tipoLogger)
        {
            var diretorioAtual = Path.GetDirectoryName(AppContext.BaseDirectory);
            var diretorioLog = Path.Join(diretorioAtual, "_logs");

            var loggerConfig = new LoggerConfiguration()
                .Enrich.With(new AdicionaThreadIdEmLog())
                .WriteTo.Console();

            var logger = tipoLogger switch
            {
                TipoLogger.CONTEXTO => loggerConfig.WriteTo.File($"{diretorioLog}/_log-banco-de-dados_.txt",
                    rollingInterval: RollingInterval.Month,
                    outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fffff} (Processo {ThreadID}) {Message:lj}{NewLine}{Exception}"
                    ),
                TipoLogger.GERENCIAMENTO => loggerConfig.WriteTo.File($"{diretorioLog}/_log-gerenciamento_.txt",
                    rollingInterval: RollingInterval.Month,
                    outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fffff} (Processo {ThreadID}) {Message:lj}{NewLine}{Exception}"
                    ),
                _ => loggerConfig.WriteTo.File($"{diretorioLog}/_log-0_.txt",
                    rollingInterval: RollingInterval.Month,
                    outputTemplate: "{Timestamp:dd-MM-yyyy HH:mm:ss.fffff} (Processo {ThreadID}) {Message:lj}{NewLine}{Exception}"
                    ),
            };

            return logger.CreateLogger();
        }
    }
}
