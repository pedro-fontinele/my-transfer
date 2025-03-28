﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TCloudFileSync.Infra.Contexto;

#nullable disable

namespace TCloudFileSync.Infra.Migrations
{
    [DbContext(typeof(SqliteContexto))]
    [Migration("20230807142732_DSUPXVEN-8051")]
    partial class DSUPXVEN8051
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("TCloudFileSync.Infra.Entidade.ConfiguracaoSftp", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("SeqSftpConfig");

                    b.Property<string>("Host")
                        .IsRequired()
                        .HasColumnType("varchar2")
                        .HasColumnName("DescHost")
                        .HasComment("Host SFTP");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("varchar2")
                        .HasColumnName("DescPassword")
                        .HasComment("Senha SFTP");

                    b.Property<int>("Port")
                        .HasColumnType("integer")
                        .HasColumnName("CodPort");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("varchar2")
                        .HasColumnName("DescUsername")
                        .HasComment("Nome de usuário SFTP");

                    b.HasKey("Id")
                        .HasName("ECM_SFTPCONFIGPK");

                    b.ToTable("ECM_SFTPCONFIG", (string)null);
                });

            modelBuilder.Entity("TCloudFileSync.Infra.Entidade.Rotina", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("SeqSftpRotina");

                    b.Property<string>("Ativo")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("varchar2")
                        .HasDefaultValue("N")
                        .HasColumnName("IndAtivo")
                        .HasComment("S para ativo | N para inativo");

                    b.Property<string>("CaminhoArquivoLocal")
                        .IsRequired()
                        .HasColumnType("varchar2")
                        .HasColumnName("DescCaminhoLocal")
                        .HasComment("Caminho de pasta local sem \\ no final");

                    b.Property<string>("CaminhoArquivoNuvem")
                        .IsRequired()
                        .HasColumnType("varchar2")
                        .HasColumnName("DescCaminhoNuvem")
                        .HasComment("Caminho de pasta na nuvem sem / no final");

                    b.Property<string>("DeveApagarArquivoOrigem")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("varchar2")
                        .HasDefaultValue("N")
                        .HasColumnName("IndApagaArquivo")
                        .HasComment("S para apagar arquivo na origem | N para não apagar");

                    b.Property<string>("FluxoLocalParaNuvem")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(1)
                        .HasColumnType("varchar2")
                        .HasDefaultValue("S")
                        .HasColumnName("IndLocalParaNuvem")
                        .HasComment("S para integrar arquivo local para nuvem | N para integrar arquivo nuvem para local");

                    b.Property<int>("IdConfiguracaoSftp")
                        .HasColumnType("integer")
                        .HasColumnName("IdConfiguracaoSftp");

                    b.Property<int>("TempoParaIniciarProximoEnvio")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasDefaultValue(5000)
                        .HasColumnName("NroIntervalo")
                        .HasComment("Intervalo entre envios em milissegundos");

                    b.HasKey("Id")
                        .HasName("ECM_SFTPROTINAPK");

                    b.HasIndex("IdConfiguracaoSftp");

                    b.ToTable("ECM_SFTPROTINA", (string)null);
                });

            modelBuilder.Entity("TCloudFileSync.Infra.Entidade.Rotina", b =>
                {
                    b.HasOne("TCloudFileSync.Infra.Entidade.ConfiguracaoSftp", "ConfiguracaoSftp")
                        .WithMany("Rotinas")
                        .HasForeignKey("IdConfiguracaoSftp")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("ECM_SFTPCONFIGFK1");

                    b.Navigation("ConfiguracaoSftp");
                });

            modelBuilder.Entity("TCloudFileSync.Infra.Entidade.ConfiguracaoSftp", b =>
                {
                    b.Navigation("Rotinas");
                });
#pragma warning restore 612, 618
        }
    }
}
