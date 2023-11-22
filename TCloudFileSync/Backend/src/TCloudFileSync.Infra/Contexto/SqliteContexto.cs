using Microsoft.EntityFrameworkCore;
using System.IO;
using System;
using TCloudFileSync.Infra.Entidade;
using TCloudFileSync.Infra.Mapeamento;

namespace TCloudFileSync.Infra.Contexto
{
    public class SqliteContexto : DbContext
    {
        public string CaminhoArquivoDB { get; set; }
        public string DiretorioDB { get; private set; }

        public DbSet<ConfiguracaoSftp> ConfiguracaoSftp { get; set; }
        public DbSet<Rotina> Rotina { get; set; }
        public DbSet<HistoricoSincronismo> HistoricoSincronismo { get; set; }

        public SqliteContexto(DbContextOptions<SqliteContexto> options) : base(options)
        {
            var diretorioAtual = Path.GetDirectoryName(AppContext.BaseDirectory);
            DiretorioDB = Path.Join(diretorioAtual, "_database");
            CaminhoArquivoDB = Path.Join(DiretorioDB, "TCloudFileSync.db");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={CaminhoArquivoDB}");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new ConfiguracaoSftpMap());
            builder.ApplyConfiguration(new RotinaMap());
            builder.ApplyConfiguration(new HistoricoSincronismoMap());

            base.OnModelCreating(builder);
        }
    }
}
