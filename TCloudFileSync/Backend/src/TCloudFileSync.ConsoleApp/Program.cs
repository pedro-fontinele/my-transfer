using Microsoft.Extensions.DependencyInjection;
using Renci.SshNet;
using System;
using System.Threading;
using TCloudFileSync.Aplicacao;
using TCloudFileSync.Cross.IoC;
using TCloudFileSync.Infra.Configuracoes;

namespace TCloudFileSync.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TestaBaseDeDados();

            //TesteClient(param.Host, param.Port, param.Username, param.Password);

            //var x = new TesteEscoloDI();

            //x.Testa();

            Console.ReadKey();
        }

        public static void TesteAplicacao()
        {
            IServiceCollection servicos = new ServiceCollection();
            servicos.ConfiguraIntegradorSftp();

            var _serviceProvider = servicos.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            var contextoConfiguracao = scope.ServiceProvider.GetService<ContextoConfiguracao>();
            contextoConfiguracao.PreparaBaseDeDados();

            var _integradorArquivoSftp = scope.ServiceProvider.GetService<GerenciadorIntegracaoArquivos>();

            while (true) { 
                _integradorArquivoSftp.RealizaIntegracaoArquivosViaSftp();

                Thread.Sleep(15000);
            }
        }

        public static void TesteClient(string host, int port, string username, string password)
        {
            var client = new SftpClient(host, port, username, password);

            client.Connect();

            client.CreateDirectory("/testeConsole");

            client.Disconnect();
        }

        public static void TestaBaseDeDados()
        {
            IServiceCollection servicos = new ServiceCollection();
            servicos.ConfiguraIntegradorSftp();

            var _serviceProvider = servicos.BuildServiceProvider();
            var scope = _serviceProvider.CreateScope();

            var contextoConfiguracao = scope.ServiceProvider.GetService<ContextoConfiguracao>();
            contextoConfiguracao.PreparaBaseDeDados();
        }
    }
}
