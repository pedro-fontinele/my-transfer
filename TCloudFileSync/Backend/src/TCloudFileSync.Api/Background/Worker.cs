using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao;

namespace TCloudFileSync.Api.Background
{
    public class Worker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var scope = _serviceProvider.CreateScope();
            var gerenciadorIntegracaoArquivos = scope.ServiceProvider.GetRequiredService<GerenciadorIntegracaoArquivos>();

            while (!stoppingToken.IsCancellationRequested)
            {
                gerenciadorIntegracaoArquivos.RealizaIntegracaoArquivosViaSftp();
                Thread.Sleep(10000);
            }

            return Task.CompletedTask;
        }
    }
}
