
using Microsoft.Extensions.DependencyInjection;
using TCloudFileSync.Aplicacao;
using TCloudFileSync.Aplicacao.Auxiliar;
using TCloudFileSync.Aplicacao.Repositorio;
using TCloudFileSync.Aplicacao.Servico;
using TCloudFileSync.Infra;
using TCloudFileSync.Infra.Extensoes;

namespace TCloudFileSync.Cross.IoC
{
    public static class InjecaoDependencia
    {
        public static void ConfiguraIntegradorSftp(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<GerenciadorIntegracaoArquivos>();
            serviceCollection.AddScoped<Notificacao>();
            serviceCollection.AddScoped<LeitorArquivo>();
            serviceCollection.AddScoped<IIntegradorArquivoSftp, IntegradorArquivoSftp>();
            serviceCollection.AddScoped<IConfiguracaoSftpRepositorio, ConfiguracaoSftpRepositorio>();
            serviceCollection.AddScoped<IConfiguracaoSftpServico, ConfiguracaoSftpServico>();
            serviceCollection.AddScoped<IRotinaRepositorio, RotinaRepositorio>();
            serviceCollection.AddScoped<IRotinaServico, RotinaServico>();
            serviceCollection.AddScoped<ILogServico, LogServico>();
            serviceCollection.AddScoped<IHistoricoSincronismoService, HistoricoSincronismoService>();
            serviceCollection.AddScoped<IHistoricoSincronismoRepositorio, HistoricoSincronismoRepositorio>();

            // Renci (SSH.NET) 
            serviceCollection.AddScoped<ISftpClientServico, RenciSSH.SshNetSftpClientServico>();

            // WinSCP 
            //serviceCollection.AddScoped<ISftpClientServico, WinSCP.WinSCPSftpClientServico>();

            serviceCollection.ConfiguraBaseDeDados();
        }
    }
}
