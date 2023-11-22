using Serilog.Core;
using Serilog.Events;
using System.Threading;

namespace TCloudFileSync.Aplicacao.Configuracoes
{
    public class AdicionaThreadIdEmLog : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(
              "ThreadID", Thread.CurrentThread.ManagedThreadId.ToString("D4")));
        }
    }
}
