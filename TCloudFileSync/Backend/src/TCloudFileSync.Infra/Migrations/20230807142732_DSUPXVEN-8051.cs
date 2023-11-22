using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TCloudFileSync.Infra.Migrations
{
    /// <inheritdoc />
    public partial class DSUPXVEN8051 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ECM_SFTPCONFIG",
                columns: table => new
                {
                    SeqSftpConfig = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DescHost = table.Column<string>(type: "varchar2", nullable: false, comment: "Host SFTP"),
                    CodPort = table.Column<int>(type: "integer", nullable: false),
                    DescUsername = table.Column<string>(type: "varchar2", nullable: false, comment: "Nome de usuário SFTP"),
                    DescPassword = table.Column<string>(type: "varchar2", nullable: false, comment: "Senha SFTP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("ECM_SFTPCONFIGPK", x => x.SeqSftpConfig);
                });

            migrationBuilder.CreateTable(
                name: "ECM_SFTPROTINA",
                columns: table => new
                {
                    SeqSftpRotina = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IndAtivo = table.Column<string>(type: "varchar2", maxLength: 1, nullable: false, defaultValue: "N", comment: "S para ativo | N para inativo"),
                    DescCaminhoLocal = table.Column<string>(type: "varchar2", nullable: false, comment: "Caminho de pasta local sem \\ no final"),
                    DescCaminhoNuvem = table.Column<string>(type: "varchar2", nullable: false, comment: "Caminho de pasta na nuvem sem / no final"),
                    NroIntervalo = table.Column<int>(type: "integer", nullable: false, defaultValue: 5000, comment: "Intervalo entre envios em milissegundos"),
                    IndApagaArquivo = table.Column<string>(type: "varchar2", maxLength: 1, nullable: false, defaultValue: "N", comment: "S para apagar arquivo na origem | N para não apagar"),
                    IndLocalParaNuvem = table.Column<string>(type: "varchar2", maxLength: 1, nullable: false, defaultValue: "S", comment: "S para integrar arquivo local para nuvem | N para integrar arquivo nuvem para local"),
                    IdConfiguracaoSftp = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ECM_SFTPROTINAPK", x => x.SeqSftpRotina);
                    table.ForeignKey(
                        name: "ECM_SFTPCONFIGFK1",
                        column: x => x.IdConfiguracaoSftp,
                        principalTable: "ECM_SFTPCONFIG",
                        principalColumn: "SeqSftpConfig",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ECM_SFTPROTINA_IdConfiguracaoSftp",
                table: "ECM_SFTPROTINA",
                column: "IdConfiguracaoSftp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ECM_SFTPROTINA");

            migrationBuilder.DropTable(
                name: "ECM_SFTPCONFIG");
        }
    }
}
