using Microsoft.Extensions.DependencyInjection;
using TCloudFileSync.Aplicacao;
using TCloudFileSync.Aplicacao.Auxiliar;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Servico;
using TCloudFileSync.Testes.Auxiliar;
using TCloudFileSync.Testes.Mock;
using Xunit;

namespace TCloudFileSync.Testes
{
    public class IntegradorArquivoSftpTeste
    {
        readonly ServiceProvider serviceProvider;

        public IntegradorArquivoSftpTeste()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<LeitorArquivo>(x => GeradorMock.LeitorArquivo());
            serviceCollection.AddScoped<ILogServico>(x => GeradorMock.LogServico());
            serviceCollection.AddScoped<Notificacao>();
            serviceCollection.AddScoped<ISftpClientServico>(x => GeradorMock.SftpClientServico());
            serviceCollection.AddScoped<IntegradorArquivoSftp>();

            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        // TODO (AUTOMAÇÃO) teste de sucesso aplicável para meta de automação 
        [Theory]
        [InlineData("host", 1, "username", "password", Argumento.Sucesso, Argumento.Sucesso, true, 1, 1, false, true)]
        public void Testar_se_envia_arquivo_nuvem_para_local_com_sucesso(string host, int port, string username, string password, string caminhoArquivoLocal, string caminhoArquivoNuvem, bool ativo, int id, int tempoParaIniciarProximoEnvio, bool deveApagarArquivoOrigem, bool fluxoLocalParaNuvem)
        {
            var configClienteSftp = new ConfiguracaoClienteSftp(host, port, username, password);
            var configEnvioArquivo = new ConfiguracaoEnvioArquivo
            {
                Ativo = ativo,
                CaminhoArquivoLocal = caminhoArquivoLocal,
                CaminhoArquivoNuvem = caminhoArquivoNuvem,
                Id = id,
                TempoParaIniciarProximoEnvio = tempoParaIniciarProximoEnvio,
                DeveApagarArquivoOrigem = deveApagarArquivoOrigem,
                FluxoLocalParaNuvem = fluxoLocalParaNuvem
            };

            var integradorArquivoSftp = serviceProvider.GetRequiredService<IntegradorArquivoSftp>();

            integradorArquivoSftp.RealizaIntegracaoArquivoNuvemParaLocal(configEnvioArquivo, configClienteSftp);

            var notification = serviceProvider.GetRequiredService<Notificacao>();

            var result = notification.ExibeNotificacoes();

            Assert.False(notification.HouveNotificacao());
            Assert.Equal(string.Empty, result);
        }

        // TODO (AUTOMAÇÃO) teste de sucesso aplicável para meta de automação 
        [Theory]
        [InlineData("host", 2, "username", "password", Argumento.Sucesso, Argumento.Sucesso, true, 1, 1, false, false)]
        public void Testar_se_envia_arquivo_local_para_nuvem_com_sucesso(string host, int port, string username, string password, string caminhoArquivoLocal, string caminhoArquivoNuvem, bool ativo, int id, int tempoParaIniciarProximoEnvio, bool deveApagarArquivoOrigem, bool fluxoLocalParaNuvem)
        {
            var configClienteSftp = new ConfiguracaoClienteSftp(host, port, username, password);
            var configEnvioArquivo = new ConfiguracaoEnvioArquivo
            {
                Ativo = ativo,
                CaminhoArquivoLocal = caminhoArquivoLocal,
                CaminhoArquivoNuvem = caminhoArquivoNuvem,
                Id = id,
                TempoParaIniciarProximoEnvio = tempoParaIniciarProximoEnvio,
                DeveApagarArquivoOrigem = deveApagarArquivoOrigem,
                FluxoLocalParaNuvem = fluxoLocalParaNuvem
            };

            var integradorArquivoSftp = serviceProvider.GetRequiredService<IntegradorArquivoSftp>();

            integradorArquivoSftp.RealizaIntegracaoArquivoNuvemParaLocal(configEnvioArquivo, configClienteSftp);

            var notification = serviceProvider.GetRequiredService<Notificacao>();

            var result = notification.ExibeNotificacoes();

            Assert.False(notification.HouveNotificacao());
            Assert.Equal(string.Empty, result);
        }
    }
}
