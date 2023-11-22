using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Repositorio;
using TCloudFileSync.Infra.Auxiliar;
using TCloudFileSync.Infra.Contexto;
using TCloudFileSync.Infra.Entidade;

namespace TCloudFileSync.Infra
{
    public class ConfiguracaoSftpRepositorio : IConfiguracaoSftpRepositorio
    {
        public readonly IServiceProvider _serviceProvider;
        public readonly IMapper _mapper;
        public readonly SqliteContexto _contexto;

        public ConfiguracaoSftpRepositorio(IServiceProvider serviceProvider, IMapper mapper, SqliteContexto sqliteContexto)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _contexto = sqliteContexto;
        }

        public EnviaArquivoDto BuscaConfiguracoesSftp()
        {
            using var _contexto = _serviceProvider.CreateScope().ServiceProvider.GetService<SqliteContexto>();

            var entidade = _contexto.ConfiguracaoSftp
                .Include(e => e.Rotinas)
                .OrderBy(e => e.Id)
                .FirstOrDefault();

            if (entidade == null)
            {
                return null;
            }

            var retorno = new EnviaArquivoDto
            {
                ConfiguracaoClienteSftp = _mapper.Map<ConfiguracaoClienteSftp>(entidade),
                ConfiguracaoEnvioArquivo = entidade.Rotinas.Select(EntidadeConversor.Converte).ToArray()
            };

            return retorno;
        }

        public async Task InsereConfiguracaoSftp(ConfiguracaoClienteSftp configuracaoClienteSftp)
        {
            _contexto.ConfiguracaoSftp.Add(_mapper.Map<ConfiguracaoSftp>(configuracaoClienteSftp));
            await _contexto.SaveChangesAsync();
        }

        public async Task AtualizaConfiguracaoSftp(ConfiguracaoClienteSftp configuracaoClienteSftp)
        {
            var entidade = _contexto.ConfiguracaoSftp
                                     .Include(e => e.Rotinas)
                                     .FirstOrDefault();

            if (entidade != null)
            {
                entidade.Host = configuracaoClienteSftp.Host;
                entidade.Port = configuracaoClienteSftp.Port;
                entidade.Username = configuracaoClienteSftp.Username;
                entidade.Password = configuracaoClienteSftp.Password;

                _contexto.ConfiguracaoSftp.Update(_mapper.Map<ConfiguracaoSftp>(entidade));
                await _contexto.SaveChangesAsync();
            }
        }

        /// <summary>
        /// DESCONTINUADO - Busca configuração de cliente SFTP e rotinas de integração em arquivo json  
        /// </summary>
        private EnviaArquivoDto BuscaConfiguracoesJson()
        {
            var configuracao = new ConfigurationBuilder()
                .AddJsonFile("configuracoes.json")
                .SetBasePath(Path.GetDirectoryName(AppContext.BaseDirectory))
                .Build();

            var configuracaoClienteSftp = new ConfiguracaoClienteSftp();
            new ConfigureFromConfigurationOptions<ConfiguracaoClienteSftp>(
                    configuracao.GetSection("ConfiguracaoClienteSftp"))
                            .Configure(configuracaoClienteSftp);

            var configuracaoEnvioArquivo = new List<ConfiguracaoEnvioArquivo>();
            new ConfigureFromConfigurationOptions<List<ConfiguracaoEnvioArquivo>>(
                    configuracao.GetSection("ConfiguracaoEnvioArquivo"))
                            .Configure(configuracaoEnvioArquivo);

            var param = new EnviaArquivoDto
            {
                ConfiguracaoEnvioArquivo = configuracaoEnvioArquivo.ToArray(),
                ConfiguracaoClienteSftp = configuracaoClienteSftp
            };

            return param;
        }
    }
}
