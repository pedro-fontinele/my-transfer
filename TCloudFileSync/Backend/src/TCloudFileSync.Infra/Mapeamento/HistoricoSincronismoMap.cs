using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCloudFileSync.Infra.Entidade;

namespace TCloudFileSync.Infra.Mapeamento
{
    public class HistoricoSincronismoMap : IEntityTypeConfiguration<HistoricoSincronismo>
    {
        public void Configure(EntityTypeBuilder<HistoricoSincronismo> builder)
        {
            builder.HasKey(e => e.Id).HasName("ECM_HISTORICOSINCRONISMOPK"); ;

            builder.Property(h => h.Id)
                   .HasColumnName("SeqHistorico")
                   .HasColumnType("integer")
                   .ValueGeneratedOnAdd();

            builder.Property(h => h.Arquivo)
                   .HasColumnName("DescArquivo")
                   .HasComment("Descrição arquivo")
                   .HasColumnType("varchar2")
                   .IsRequired();

            builder.Property(h => h.FluxoLocalParaNuvem)
                   .HasColumnName("IndLocalParaNuvem")
                   .HasComment("S para integrar arquivo local para nuvem | N para integrar arquivo nuvem para local")
                   .HasDefaultValue("S")
                   .HasColumnType("varchar2")
                   .HasMaxLength(1)
                   .IsRequired();

            builder.Property(h => h.CaminhoArquivoLocal)
                   .HasColumnName("DescCaminhoLocal")
                   .HasComment("Caminho de pasta local sem \\ no final")
                   .HasColumnType("varchar2")
                   .IsRequired();

            builder.Property(h => h.CaminhoArquivoNuvem)
                   .HasColumnName("DescCaminhoNuvem")
                   .HasComment("Caminho de pasta na nuvem sem / no final")
                   .HasColumnType("varchar2")
                   .IsRequired();

            builder.Property(h => h.Tamanho)
                   .HasColumnName("TamanhoArquivo")
                   .HasComment("Discrimina o tamanho do arquivo transitado.")
                   .HasColumnType("integer")
                   .IsRequired();

            builder.Property(h => h.DtaMovimento)
                   .HasColumnName("DtaMovimento")
                   .HasComment("Data da movimentação do arquivo.")
                   .HasColumnType("varchar2")
                   .IsRequired();

            builder.Property(h => h.HorMovimento)
                   .HasColumnName("HorMovimento")
                   .HasComment("Hora da movimentação do arquivo.")
                   .HasColumnType("varchar2")
                   .IsRequired();

            builder.Property(h => h.Situacao)
                   .HasColumnName("Situacao")
                   .HasComment("Situação do arquivo (FALHA, ENVIANDO, SINCRONIZADA).")
                   .HasColumnType("varchar2")
                   .IsRequired();

            builder.ToTable("ECM_HISTORICOSINCRONISMO");
        }
    }
}
