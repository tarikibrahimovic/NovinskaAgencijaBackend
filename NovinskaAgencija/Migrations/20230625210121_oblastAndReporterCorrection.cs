using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovinskaAgencija.Migrations
{
    /// <inheritdoc />
    public partial class oblastAndReporterCorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClanakOblast");

            migrationBuilder.DropTable(
                name: "ReporterClanak");

            migrationBuilder.AddColumn<int>(
                name: "OblastId",
                table: "Clanci",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReporterId",
                table: "Clanci",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Clanci_OblastId",
                table: "Clanci",
                column: "OblastId");

            migrationBuilder.CreateIndex(
                name: "IX_Clanci_ReporterId",
                table: "Clanci",
                column: "ReporterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clanci_Oblasti_OblastId",
                table: "Clanci",
                column: "OblastId",
                principalTable: "Oblasti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Clanci_Reporteri_ReporterId",
                table: "Clanci",
                column: "ReporterId",
                principalTable: "Reporteri",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clanci_Oblasti_OblastId",
                table: "Clanci");

            migrationBuilder.DropForeignKey(
                name: "FK_Clanci_Reporteri_ReporterId",
                table: "Clanci");

            migrationBuilder.DropIndex(
                name: "IX_Clanci_OblastId",
                table: "Clanci");

            migrationBuilder.DropIndex(
                name: "IX_Clanci_ReporterId",
                table: "Clanci");

            migrationBuilder.DropColumn(
                name: "OblastId",
                table: "Clanci");

            migrationBuilder.DropColumn(
                name: "ReporterId",
                table: "Clanci");

            migrationBuilder.CreateTable(
                name: "ClanakOblast",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClanakId = table.Column<int>(type: "int", nullable: false),
                    OblastId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanakOblast", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClanakOblast_Clanci_ClanakId",
                        column: x => x.ClanakId,
                        principalTable: "Clanci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClanakOblast_Oblasti_OblastId",
                        column: x => x.OblastId,
                        principalTable: "Oblasti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReporterClanak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClanakId = table.Column<int>(type: "int", nullable: false),
                    ReporterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReporterClanak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReporterClanak_Clanci_ClanakId",
                        column: x => x.ClanakId,
                        principalTable: "Clanci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReporterClanak_Reporteri_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "Reporteri",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClanakOblast_ClanakId",
                table: "ClanakOblast",
                column: "ClanakId");

            migrationBuilder.CreateIndex(
                name: "IX_ClanakOblast_OblastId",
                table: "ClanakOblast",
                column: "OblastId");

            migrationBuilder.CreateIndex(
                name: "IX_ReporterClanak_ClanakId",
                table: "ReporterClanak",
                column: "ClanakId");

            migrationBuilder.CreateIndex(
                name: "IX_ReporterClanak_ReporterId",
                table: "ReporterClanak",
                column: "ReporterId");
        }
    }
}
