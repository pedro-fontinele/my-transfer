using Microsoft.EntityFrameworkCore.Migrations;

namespace TCloudFileSync.Infra.Migrations
{
    public partial class DSUPXVEN7850 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                    name: "ECM_HISTORICOSINCRONISMO",
                    columns: table => new
                    {
                        SeqHistorico = table.Column<int>(type: "integer", nullable: false).Annotation("Sqlite:Autoincrement", true),
                        DescArquivo = table.Column<string>(type: "varchar2", nullable: false, comment: "Descrição arquivo"),
                        IndLocalParaNuvem = table.Column<string>(type: "varchar2", maxLength: 1, nullable: false, defaultValue: "S", comment: "S para integrar arquivo local para nuvem | N para integrar arquivo nuvem para local"),
                        DescCaminhoLocal = table.Column<string>(type: "varchar2", nullable: false, comment: "Caminho de pasta local sem \\ no final"),
                        DescCaminhoNuvem = table.Column<string>(type: "varchar2", nullable: false, comment: "Caminho de pasta na nuvem sem / no final"),
                        TamanhoArquivo = table.Column<int>(type: "integer", nullable: false, comment: "Discrimina o tamanho do arquivo transitado."),
                        DtaMovimento = table.Column<string>(type: "varchar2", nullable: false, comment: "Data da movimentação do arquivo."),
                        HorMovimento = table.Column<string>(type: "varchar2", nullable: false, comment: "Hora da movimentação do arquivo."),
                        Situacao = table.Column<string>(type: "varchar2", nullable: false, comment: "Situação do arquivo (FALHA, ENVIANDO, SINCRONIZADA).")
                    },
                    constraints: table =>
                    {
                        table.PrimaryKey("ECM_HISTORICOSINCRONISMOPK", x => x.SeqHistorico);
                    });

            migrationBuilder.CreateIndex(
                    name: "IX_ECM_HISTORICOSINCRONISMO_SeqHistorico",
                    table: "ECM_HISTORICOSINCRONISMO",
                    column: "SeqHistorico");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ECM_HISTORICOSINCRONISMO");
        }
    }
}
