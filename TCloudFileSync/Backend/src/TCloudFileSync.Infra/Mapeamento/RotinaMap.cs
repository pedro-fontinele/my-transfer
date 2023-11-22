using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCloudFileSync.Infra.Entidade;

namespace TCloudFileSync.Infra.Mapeamento
{
    public class RotinaMap : IEntityTypeConfiguration<Rotina>
    {
        public void Configure(EntityTypeBuilder<Rotina> builder)
        {
            builder.HasKey(e => e.Id).HasName("ECM_SFTPROTINAPK");

            builder.Property(e => e.Id)
                .HasColumnName("SeqSftpRotina")
                .HasColumnType("integer")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.CaminhoArquivoLocal)
                .HasColumnName("DescCaminhoLocal")
                .HasComment("Caminho de pasta local sem \\ no final")
                .HasColumnType("varchar2")
                .IsRequired();

            builder.Property(e => e.CaminhoArquivoNuvem)
                .HasColumnName("DescCaminhoNuvem")
                .HasComment("Caminho de pasta na nuvem sem / no final")
                .HasColumnType("varchar2")
                .IsRequired();
            
            builder.Property(e => e.TempoParaIniciarProximoEnvio)
                .HasColumnName("NroIntervalo")
                .HasDefaultValue(5000)
                .HasComment("Intervalo entre envios em milissegundos")
                .HasColumnType("integer")
                .IsRequired();
            
            builder.Property(e => e.Ativo)
                .HasColumnName("IndAtivo")
                .HasComment("S para ativo | N para inativo")
                .HasDefaultValue("N")
                .HasColumnType("varchar2")
                .HasMaxLength(1)
                .IsRequired();
            
            builder.Property(e => e.DeveApagarArquivoOrigem)
                .HasColumnName("IndApagaArquivo")
                .HasComment("S para apagar arquivo na origem | N para não apagar")
                .HasDefaultValue("N")
                .HasColumnType("varchar2")
                .HasMaxLength(1)
                .IsRequired();
            
            builder.Property(e => e.FluxoLocalParaNuvem)
                .HasColumnName("IndLocalParaNuvem")
                .HasComment("S para integrar arquivo local para nuvem | N para integrar arquivo nuvem para local")
                .HasDefaultValue("S")
                .HasColumnType("varchar2")
                .HasMaxLength(1)
                .IsRequired();

            // IdConfiguracaoSftp
            builder.Property(e => e.IdConfiguracaoSftp)
                .HasColumnName("IdConfiguracaoSftp")
                .HasColumnType("integer")
                .IsRequired();

            builder.HasOne(e => e.ConfiguracaoSftp)
                .WithMany(e => e.Rotinas)
                .HasForeignKey(e => e.IdConfiguracaoSftp);

            builder.ToTable("ECM_SFTPROTINA");
        }
    }
}
