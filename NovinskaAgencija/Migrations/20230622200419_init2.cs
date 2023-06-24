using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovinskaAgencija.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placanja_Users_UserId",
                table: "Placanja");

            migrationBuilder.DropTable(
                name: "UserClanak");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Placanja",
                newName: "KlijentId");

            migrationBuilder.RenameIndex(
                name: "IX_Placanja_UserId",
                table: "Placanja",
                newName: "IX_Placanja_KlijentId");

            migrationBuilder.AddColumn<string>(
                name: "FileUrl",
                table: "Clanci",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Klijenti",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    NazivKompanije = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TipPreduzeca = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Klijenti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Klijenti_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reporteri",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Ime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Prezime = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reporteri", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reporteri_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
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
                name: "IX_Klijenti_UserId",
                table: "Klijenti",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReporterClanak_ClanakId",
                table: "ReporterClanak",
                column: "ClanakId");

            migrationBuilder.CreateIndex(
                name: "IX_ReporterClanak_ReporterId",
                table: "ReporterClanak",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_Reporteri_UserId",
                table: "Reporteri",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Placanja_Klijenti_KlijentId",
                table: "Placanja",
                column: "KlijentId",
                principalTable: "Klijenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placanja_Klijenti_KlijentId",
                table: "Placanja");

            migrationBuilder.DropTable(
                name: "Klijenti");

            migrationBuilder.DropTable(
                name: "ReporterClanak");

            migrationBuilder.DropTable(
                name: "Reporteri");

            migrationBuilder.DropColumn(
                name: "FileUrl",
                table: "Clanci");

            migrationBuilder.RenameColumn(
                name: "KlijentId",
                table: "Placanja",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Placanja_KlijentId",
                table: "Placanja",
                newName: "IX_Placanja_UserId");

            migrationBuilder.CreateTable(
                name: "UserClanak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClanakId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClanak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClanak_Clanci_ClanakId",
                        column: x => x.ClanakId,
                        principalTable: "Clanci",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserClanak_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserClanak_ClanakId",
                table: "UserClanak",
                column: "ClanakId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClanak_UserId",
                table: "UserClanak",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Placanja_Users_UserId",
                table: "Placanja",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
