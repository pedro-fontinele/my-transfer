using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using TCloudFileSync.Api.Background;
using TCloudFileSync.Infra.Configuracoes;

namespace TCloudFileSync.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            host.Services
                .CreateScope()
                .ServiceProvider
                .GetService<ContextoConfiguracao>()
                .PreparaBaseDeDados();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureServices((hostContext, services) => 
                {
                    services.AddHostedService<Worker>();
                });
    }
}
