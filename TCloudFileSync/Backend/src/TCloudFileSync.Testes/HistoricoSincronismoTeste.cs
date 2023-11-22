using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Enum;
using TCloudFileSync.Aplicacao.Repositorio;
using TCloudFileSync.Aplicacao.Servico;
using Xunit;

namespace TCloudFileSync.Testes
{
    public class HistoricoSincronismoTeste
    {
        private readonly ServiceProvider serviceProvider;

        public HistoricoSincronismoTeste()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<Notificacao>();

            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task Deve_Retornar_Todos_Os_Resigstro()
        {
            // Arrange
            var hiistoricoSincronismoRepositorioMock = new Mock<IHistoricoSincronismoRepositorio>();
            var logMock = new Mock<ILogServico>();
            var notificacaoMock = new Mock<Notificacao>();

            var historico = new List<HistoricoSincronismoDto>
            {
                new HistoricoSincronismoDto
                {
                    Arquivo = "Arquivo1",
                    FluxoLocalParaNuvem = "Fluxo1",
                    CaminhoArquivoLocal = "Caminho1",
                    CaminhoArquivoNuvem = "Nuvem1",
                    Tamanho = 1024,
                    DtaMovimento = DateTime.Now.ToShortDateString(),
                    HorMovimento = DateTime.Now.ToShortTimeString(),
                    Situacao = "Situacao1"
                },
                new HistoricoSincronismoDto
                {
                    Arquivo = "Arquivo2",
                    FluxoLocalParaNuvem = "Fluxo2",
                    CaminhoArquivoLocal = "Caminho2",
                    CaminhoArquivoNuvem = "Nuvem2",
                    Tamanho = 2048,
                    DtaMovimento = DateTime.Now.ToShortDateString(),
                    HorMovimento = DateTime.Now.ToShortTimeString(),
                    Situacao = "Situacao2"
                },
                new HistoricoSincronismoDto
                {
                    Arquivo = "Arquivo3",
                    FluxoLocalParaNuvem = "Fluxo3",
                    CaminhoArquivoLocal = "Caminho3",
                    CaminhoArquivoNuvem = "Nuvem3",
                    Tamanho = 3072,
                    DtaMovimento = DateTime.Now.ToShortDateString(),
                    HorMovimento = DateTime.Now.ToShortTimeString(),
                    Situacao = "Situacao3"
                }
            };

            hiistoricoSincronismoRepositorioMock.Setup(repo => repo.BuscaHistoricoSincronismo(1, 20)).ReturnsAsync(historico);
            var historicoSincronismoService = new HistoricoSincronismoService(notificacaoMock.Object, hiistoricoSincronismoRepositorioMock.Object, logMock.Object);

            // Act
            var response = await historicoSincronismoService.BuscaHistoricoSincronismo();
            var notification = serviceProvider.GetRequiredService<Notificacao>();

            // Assert
            Assert.NotNull(response);
            Assert.False(notification.HouveNotificacao());
            Assert.Equal(historico, response);
        }

        [Fact]
        public async Task Deve_Inserir_Novo_Historico()
        {
            // Arrange
            var historicoSincronismoRepositorioMock = new Mock<IHistoricoSincronismoRepositorio>();
            var logMock = new Mock<ILogServico>();
            var notificacaoMock = new Mock<Notificacao>();

            var historico = new HistoricoSincronismoDto
            {
                Arquivo = "Arquivo1",
                FluxoLocalParaNuvem = "Fluxo1",
                CaminhoArquivoLocal = "Caminho1",
                CaminhoArquivoNuvem = "Nuvem1",
                Tamanho = 1024,
                DtaMovimento = DateTime.Now.ToShortDateString(),
                HorMovimento = DateTime.Now.ToShortTimeString(),
                Situacao = "Situacao1"
            };

            var historicoList = new List<HistoricoSincronismoDto>
            {
                new HistoricoSincronismoDto
                {
                    Arquivo = "Arquivo1",
                    FluxoLocalParaNuvem = "Fluxo1",
                    CaminhoArquivoLocal = "Caminho1",
                    CaminhoArquivoNuvem = "Nuvem1",
                    Tamanho = 1024,
                    DtaMovimento = DateTime.Now.ToShortDateString(),
                    HorMovimento = DateTime.Now.ToShortTimeString(),
                    Situacao = "Situacao1"
                }
            };

            var configuracao = new ConfiguracaoEnvioArquivo
            {
                Ativo = true,
                CaminhoArquivoLocal = "Caminho/Local/Arquivo",
                CaminhoArquivoNuvem = "Caminho/Nuvem/Arquivo",
                Id = 1,
                TempoParaIniciarProximoEnvio = 1000,
                DeveApagarArquivoOrigem = true,
                FluxoLocalParaNuvem = true,
                IdConfiguracaoSftp = 123
            };

            historicoSincronismoRepositorioMock.Setup(repo => repo.InsereHistoricoSincronismo(historico));
            historicoSincronismoRepositorioMock.Setup(repo => repo.BuscaHistoricoSincronismo(1, 20)).ReturnsAsync(historicoList);            
            var historicoSincronismoService = new HistoricoSincronismoService(notificacaoMock.Object, historicoSincronismoRepositorioMock.Object, logMock.Object);

            // Act
            await historicoSincronismoService.InsereHistoricoSincronismo(configuracao, historico.Arquivo, SituacaoSincronismo.Enviando, DateTime.Now, historico.Tamanho);
            var response = historicoSincronismoService.BuscaHistoricoSincronismo();

            var notification = serviceProvider.GetRequiredService<Notificacao>();

            // Assert
            Assert.NotNull(response);
            Assert.False(notification.HouveNotificacao());
            Assert.Equal(historicoList, response.Result);
        }

        [Fact]
        public async Task Deve_Atualizar_Historico()
        {
            // Arrange
            var historicoSincronismoRepositorioMock = new Mock<IHistoricoSincronismoRepositorio>();
            var logMock = new Mock<ILogServico>();
            var notificacaoMock = new Mock<Notificacao>();

            var historico = new HistoricoSincronismoDto
            {
                Arquivo = "Arquivo1",
                FluxoLocalParaNuvem = "Fluxo1",
                CaminhoArquivoLocal = "Caminho1",
                CaminhoArquivoNuvem = "Nuvem1",
                Tamanho = 1024,
                DtaMovimento = DateTime.Now.ToShortDateString(),
                HorMovimento = DateTime.Now.ToShortTimeString(),
                Situacao = "Situacao1"
            };


            var historicoList = new List<HistoricoSincronismoDto>
            {
                new HistoricoSincronismoDto
                {
                    Arquivo = "Arquivo1",
                    FluxoLocalParaNuvem = "Fluxo1",
                    CaminhoArquivoLocal = "Caminho1",
                    CaminhoArquivoNuvem = "Nuvem1",
                    Tamanho = 1024,
                    DtaMovimento = DateTime.Now.ToShortDateString(),
                    HorMovimento = DateTime.Now.ToShortTimeString(),
                    Situacao = "Situacao1"
                }
            };

            historicoSincronismoRepositorioMock.Setup(repo => repo.AtualizaHistoricoSincronismo(historico));
            historicoSincronismoRepositorioMock.Setup(repo => repo.BuscaHistoricoSincronismo(1, 20)).ReturnsAsync(historicoList);
            historicoSincronismoRepositorioMock.Setup(repo => repo.BuscaHistoricoPorNomeArquivo(historico.Arquivo, DateTime.Now)).Returns(historico);
            var historicoSincronismoService = new HistoricoSincronismoService(notificacaoMock.Object, historicoSincronismoRepositorioMock.Object, logMock.Object);

            // Act
            await historicoSincronismoService.AtualizaHistoricoSincronismo(historico.Arquivo, SituacaoSincronismo.Enviando, DateTime.Now);
            var response = historicoSincronismoService.BuscaHistoricoSincronismo();

            var notification = serviceProvider.GetRequiredService<Notificacao>();

            // Assert
            Assert.NotNull(response);
            Assert.False(notification.HouveNotificacao());
            Assert.Equal(historicoList, response.Result);
        }
    }
}
