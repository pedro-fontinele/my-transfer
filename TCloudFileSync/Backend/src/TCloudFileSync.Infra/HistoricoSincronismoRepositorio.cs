using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TCloudFileSync.Aplicacao.Dto;
using TCloudFileSync.Aplicacao.Repositorio;
using TCloudFileSync.Infra.Contexto;
using TCloudFileSync.Infra.Entidade;

namespace TCloudFileSync.Infra
{
    public class HistoricoSincronismoRepositorio : IHistoricoSincronismoRepositorio
    {
        public readonly IMapper _mapper;
        public readonly SqliteContexto _contexto;

        public HistoricoSincronismoRepositorio(IMapper mapper, SqliteContexto contexto)
        {
            _mapper = mapper;
            _contexto = contexto;
        }

        public async Task<List<HistoricoSincronismoDto>> BuscaHistoricoSincronismo(int pagina = 1, int itensPorPagina = 20)
        {
            int registrosAPular = (pagina - 1) * itensPorPagina;

            var query = _contexto.HistoricoSincronismo.OrderByDescending(e => e.DtaMovimento)
                                                      .ThenByDescending(e => e.HorMovimento)
                                                      .Skip(registrosAPular)
                                                      .Take(itensPorPagina);

            return _mapper.Map<List<HistoricoSincronismoDto>>(await query.ToListAsync());
        }


        public HistoricoSincronismoDto BuscaHistoricoPorNomeArquivo(string nomeComExtensao, DateTime dateTime)
        {
            var consulta = _contexto.HistoricoSincronismo.AsNoTracking()
                                                         .Where(x => x.Arquivo == nomeComExtensao &&
                                                                     x.DtaMovimento == dateTime.ToString("dd/MM/yyyy") &&
                                                                     x.HorMovimento == dateTime.ToString("HH:mm:ss"))
                                                         .FirstOrDefault();

            return _mapper.Map<HistoricoSincronismoDto>(consulta);
        }


        public void InsereHistoricoSincronismo(HistoricoSincronismoDto historicoSincronismoDto)
        {
            var historicoSincronismo = new HistoricoSincronismo
            {
                Arquivo = historicoSincronismoDto.Arquivo,
                FluxoLocalParaNuvem = historicoSincronismoDto.FluxoLocalParaNuvem,
                CaminhoArquivoLocal = historicoSincronismoDto.CaminhoArquivoLocal,
                CaminhoArquivoNuvem = historicoSincronismoDto.CaminhoArquivoNuvem,
                Tamanho = historicoSincronismoDto.Tamanho,
                DtaMovimento = historicoSincronismoDto.DtaMovimento,
                HorMovimento = historicoSincronismoDto.HorMovimento,
                Situacao = historicoSincronismoDto.Situacao
            };

            _contexto.HistoricoSincronismo.Add(historicoSincronismo);
            _contexto.SaveChanges();
        }

        public void AtualizaHistoricoSincronismo(HistoricoSincronismoDto historicoSincronismoDto)
        {
            var consulta = _contexto.HistoricoSincronismo.Where(x => x.Arquivo == historicoSincronismoDto.Arquivo &&
                                                                     x.DtaMovimento == historicoSincronismoDto.DtaMovimento &&
                                                                     x.HorMovimento == historicoSincronismoDto.HorMovimento)
                                                         .FirstOrDefault();

            consulta.Situacao = historicoSincronismoDto.Situacao;
            _contexto.Entry(consulta).State = EntityState.Modified;
            _contexto.SaveChanges();
        }
    }
}
