using Moq;
using Serilog;
using System;
using System.IO;
using System.Text;
using TCloudFileSync.Aplicacao.Auxiliar;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Servico;
using TCloudFileSync.RenciSSH;
using TCloudFileSync.Testes.Auxiliar;

namespace TCloudFileSync.Testes.Mock
{
    /// <summary>
    /// Classe responsável por criar mocks de objetos que fazem acesso a recursos externos 
    /// </summary>
    public static class GeradorMock
    {
        public static LeitorArquivo LeitorArquivo()
        {
            var mock = new Mock<LeitorArquivo>();

            // sucessos 
            mock.Setup(x => x.LeArquivo(It.Is<string>(y => y == Argumento.Sucesso)))
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes(Argumento.Sucesso)));

            mock.Setup(x => x.CaminhoArquivoLocalExiste(It.Is<string>(y => y == Argumento.Sucesso)))
                .Returns(true);

            mock.Setup(x => x.RecuperaNomeDoArquivo(It.Is<string>(y => y == Argumento.Sucesso)))
                .Returns(Argumento.Sucesso);

            mock.Setup(x => x.PreparaCaminhoArquivoLocal(It.Is<string>(y => y == Argumento.Sucesso)
                    , It.Is<string>(y => y == Argumento.Sucesso)))
                .Returns(Argumento.Sucesso);

            mock.Setup(x => x.BuscaArquivosEmDiretorioLocal(It.Is<string>(y => y == Argumento.Sucesso)))
                .Returns(new string[] { Argumento.Sucesso });

            mock.Setup(x => x.BuscaNomeDiretorioAtual())
                .Returns(Argumento.Sucesso);

            mock.Setup(x => x.CopiaArquivoParaPastaTemporaria(It.Is<string>(y => y == Argumento.Sucesso), It.IsAny<string>()));
            mock.Setup(x => x.CriaPastaLocal(It.Is<string>(y => y == Argumento.Sucesso)));

            mock.Setup(x => x.RemoveArquivo(    It.Is<string>(y => y == Argumento.Sucesso)));
            mock.Setup(x => x.LimpaPasta(It.Is<string>(y => y == Argumento.Sucesso)));

            mock.Setup(x => x.CopiaConteudo(It.IsAny<Stream>(), It.IsAny<Stream>()));
            mock.Setup(x => x.FechaRecursosDeLeituraEscrita(It.IsAny<Stream>()));

            mock.Setup(x => x.CriaEAbreArquivoParaEscrita(It.Is<string>(y => y == Argumento.Sucesso)))
                .Returns(new MemoryStream(Encoding.UTF8.GetBytes(Argumento.Sucesso)));


            // erros 
            mock.Setup(x => x.LeArquivo(It.Is<string>(y => y == Argumento.Erro)))
                .Throws(new Exception(Argumento.Erro));

            mock.Setup(x => x.CaminhoArquivoLocalExiste(It.Is<string>(y => y == Argumento.Erro || string.IsNullOrEmpty(y))))
                .Returns(false);

            mock.Setup(x => x.BuscaArquivosEmDiretorioLocal(It.Is<string>(y => y == Argumento.Erro)))
                .Throws(new Exception(Argumento.Erro));

            return mock.Object;
        }

        public static SshNetSftpClientServico SftpClientServico()
        {
            var mock = new Mock<SshNetSftpClientServico>();

            // sucessos
            mock.Setup(x => x.Conecta());
            mock.Setup(x => x.Desconecta());
            mock.Setup(x => x.ClienteSftpJaConfigurado()).Returns(true);

            mock.Setup(x => x.GeraNovoSftpClient(It.IsAny<ConfiguracaoClienteSftp>()));
            mock.Setup(x => x.Upload(It.IsAny<Stream>(), It.Is<string>(y => y == Argumento.Sucesso)));
            mock.Setup(x => x.Existe(It.Is<string>(y => y == Argumento.Sucesso))).Returns(true);
            mock.Setup(x => x.CriaPasta(It.Is<string>(y => y == Argumento.Sucesso)));
            mock.Setup(x => x.LeArquivo(It.Is<string>(y => y == Argumento.Sucesso))).Returns(new MemoryStream(Encoding.UTF8.GetBytes(Argumento.Sucesso)));
            mock.Setup(x => x.ApagaArquivo(It.Is<string>(y => y == Argumento.Sucesso)));
            mock.Setup(x => x.ListaArquivosEmDiretorio(It.Is<string>(y => y == Argumento.Sucesso)))
                .Returns(new ArquivoVindoDaNuvem[1] {
                    new ArquivoVindoDaNuvem(Argumento.Sucesso, Argumento.Sucesso)
                });

            // erros 
            mock.Setup(x => x.Upload(It.IsAny<Stream>(), It.Is<string>(y => y == Argumento.Erro))).Throws(new Exception(Argumento.Erro)); ;
            mock.Setup(x => x.Existe(It.Is<string>(y => y == Argumento.Erro))).Returns(false);
            mock.Setup(x => x.CriaPasta(It.Is<string>(y => y == Argumento.Erro))).Throws(new Exception(Argumento.Erro)); ;
            mock.Setup(x => x.LeArquivo(It.Is<string>(y => y == Argumento.Erro))).Throws(new Exception(Argumento.Erro));
            mock.Setup(x => x.ApagaArquivo(It.Is<string>(y => y == Argumento.Sucesso))).Throws(new Exception(Argumento.Erro));
            mock.Setup(x => x.ListaArquivosEmDiretorio(It.Is<string>(y => y == Argumento.Erro))).Throws(new Exception(Argumento.Erro));

            return mock.Object;
        }

        public static LogServico LogServico()
        {
            var mock = new Mock<LogServico>();
            mock.Setup(x => x.EncerraRotina(It.IsAny<string>(), It.IsAny<bool>()));
            mock.Setup(x => x.Error(It.IsAny<string>()));
            mock.Setup(x => x.Information(It.IsAny<string>()));
            mock.Setup(x => x.SetaPrefixo(It.IsAny<string>()));
            mock.Setup(x => x.SetaLog(It.IsAny<ILogger>()));

            return mock.Object;
        }
    }
}
