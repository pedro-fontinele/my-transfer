using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Repositorio;
using TCloudFileSync.Infra.Auxiliar;
using TCloudFileSync.Infra.Contexto;
using TCloudFileSync.Infra.Entidade;

namespace TCloudFileSync.Infra
{
    public class RotinaRepositorio : IRotinaRepositorio
    {
        public readonly SqliteContexto _contexto;
        public readonly IMapper _mapper;

        public RotinaRepositorio(SqliteContexto contexto, IMapper mapper)
        {
            _contexto = contexto;
            _mapper = mapper;
        }

        public List<ConfiguracaoEnvioArquivo> BuscaTodasAsRotinas()
        {
            var entidades = _contexto.Rotina.ToList(); // Buscar todas as entidades

            if (entidades != null && entidades.Any())
            {
                var configuracoes = entidades.Select(entidade => new ConfiguracaoEnvioArquivo
                {
                    Ativo = entidade.Ativo == "S" ? true : false,
                    CaminhoArquivoLocal = entidade.CaminhoArquivoLocal,
                    CaminhoArquivoNuvem = entidade.CaminhoArquivoNuvem,
                    TempoParaIniciarProximoEnvio = entidade.TempoParaIniciarProximoEnvio,
                    Id = entidade.Id,
                    DeveApagarArquivoOrigem = entidade.DeveApagarArquivoOrigem == "S" ? true : false,
                    FluxoLocalParaNuvem = entidade.FluxoLocalParaNuvem == "S" ? true : false,
                    IdConfiguracaoSftp = entidade.Id
                }).ToList();

                return configuracoes;
            }

            return new List<ConfiguracaoEnvioArquivo>(); // Retornar uma lista vazia se não houver entidades
        }


        public ConfiguracaoEnvioArquivo BuscaRotinaPorId(int id)
        {
            var entidade = _contexto.Rotina.Where(x => x.Id == id).FirstOrDefault();

            if (entidade != null)
            {
                ConfiguracaoEnvioArquivo configuracaoEnvioArquivo = new ConfiguracaoEnvioArquivo
                {
                    Ativo = entidade.Ativo == "S" ? true : false,
                    CaminhoArquivoLocal = entidade.CaminhoArquivoLocal,
                    CaminhoArquivoNuvem = entidade.CaminhoArquivoNuvem,
                    TempoParaIniciarProximoEnvio = entidade.TempoParaIniciarProximoEnvio,
                    Id = entidade.Id,
                    DeveApagarArquivoOrigem = entidade.DeveApagarArquivoOrigem == "S" ? true : false,
                    FluxoLocalParaNuvem = entidade.FluxoLocalParaNuvem == "S" ? true : false,
                    IdConfiguracaoSftp = entidade.Id
                };

                return configuracaoEnvioArquivo;
            }

            return _mapper.Map<ConfiguracaoEnvioArquivo>(entidade);
        }

        public async Task InsereRotina(ConfiguracaoEnvioArquivo consultaConfiguracaoEnvioArquivo)
        {
            var entidade = _contexto.ConfiguracaoSftp
                .Include(e => e.Rotinas)
                .FirstOrDefault();

            Rotina novaRotina = new Rotina
            {
                Ativo = consultaConfiguracaoEnvioArquivo.Ativo ? "S" : "N",
                CaminhoArquivoLocal = consultaConfiguracaoEnvioArquivo.CaminhoArquivoLocal,
                CaminhoArquivoNuvem = consultaConfiguracaoEnvioArquivo.CaminhoArquivoNuvem,
                TempoParaIniciarProximoEnvio = consultaConfiguracaoEnvioArquivo.TempoParaIniciarProximoEnvio,
                Id = 0,
                DeveApagarArquivoOrigem = consultaConfiguracaoEnvioArquivo.DeveApagarArquivoOrigem ? "S" : "N",
                FluxoLocalParaNuvem = consultaConfiguracaoEnvioArquivo.FluxoLocalParaNuvem ? "S" : "N",
                IdConfiguracaoSftp = entidade.Id
            };

            _contexto.Rotina.Add(novaRotina);
            await _contexto.SaveChangesAsync();
        }

        public async Task AtualizaRotina(ConfiguracaoEnvioArquivo consultaConfiguracaoEnvioArquivo)
        {
            var rotina = await _contexto.Rotina.FindAsync(consultaConfiguracaoEnvioArquivo.Id);

            if (rotina != null)
            {
                EntidadeConversor.ConverteConsulta(consultaConfiguracaoEnvioArquivo, rotina);

                var entidade = _contexto.ConfiguracaoSftp.Include(e => e.Rotinas).FirstOrDefault();
                rotina.IdConfiguracaoSftp = entidade.Id;

                _contexto.Entry(rotina).State = EntityState.Modified;
                await _contexto.SaveChangesAsync();
            }
        }
    }
}
