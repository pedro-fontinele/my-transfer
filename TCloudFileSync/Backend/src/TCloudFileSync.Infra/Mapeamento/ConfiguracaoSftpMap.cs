using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TCloudFileSync.Infra.Entidade;

namespace TCloudFileSync.Infra.Mapeamento
{
    public class ConfiguracaoSftpMap : IEntityTypeConfiguration<ConfiguracaoSftp>
    {
        public void Configure(EntityTypeBuilder<ConfiguracaoSftp> builder)
        {
            builder.HasKey(e => e.Id).HasName("ECM_SFTPCONFIGPK");

            builder.Property(e => e.Id)
                .HasColumnName("SeqSftpConfig")
                .HasColumnType("integer")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Host)
                .HasColumnName("DescHost")
                .HasComment("Host SFTP")
                .HasColumnType("varchar2")
                .IsRequired();
            
            builder.Property(e => e.Port)
                .HasColumnName("CodPort")
                .HasColumnType("integer")
                .IsRequired();
            
            builder.Property(e => e.Username)
                .HasColumnName("DescUsername")
                .HasComment("Nome de usuário SFTP")
                .HasColumnType("varchar2")
                .IsRequired();
            
            builder.Property(e => e.Password)
                .HasColumnName("DescPassword")
                .HasComment("Senha SFTP")
                .HasColumnType("varchar2")
                .IsRequired();

            builder.HasMany<Rotina>()
                .WithOne(e => e.ConfiguracaoSftp)
                .HasForeignKey(e => e.IdConfiguracaoSftp)
                .HasConstraintName("ECM_SFTPCONFIGFK1");

            builder.ToTable("ECM_SFTPCONFIG");
        }
    }
}
