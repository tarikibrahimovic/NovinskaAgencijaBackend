using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NovinskaAgencija.Migrations
{
    /// <inheritdoc />
    public partial class userPlacanja : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placanja_Klijenti_KlijentId",
                table: "Placanja");

            migrationBuilder.RenameColumn(
                name: "KlijentId",
                table: "Placanja",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Placanja_KlijentId",
                table: "Placanja",
                newName: "IX_Placanja_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "Clanci",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishDate",
                table: "Clanci",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.InsertData(
                table: "Oblasti",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Politics" },
                    { 2, "Entertainment" },
                    { 3, "Business" },
                    { 4, "Sports" },
                    { 5, "Crime" },
                    { 6, "Education" },
                    { 7, "City Council News" },
                    { 8, "Tech" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Placanja_Users_UserId",
                table: "Placanja",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Placanja_Users_UserId",
                table: "Placanja");

            migrationBuilder.DeleteData(
                table: "Oblasti",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Oblasti",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Oblasti",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Oblasti",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Oblasti",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Oblasti",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Oblasti",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Oblasti",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "PublishDate",
                table: "Clanci");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Placanja",
                newName: "KlijentId");

            migrationBuilder.RenameIndex(
                name: "IX_Placanja_UserId",
                table: "Placanja",
                newName: "IX_Placanja_KlijentId");

            migrationBuilder.AlterColumn<string>(
                name: "FileUrl",
                table: "Clanci",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Placanja_Klijenti_KlijentId",
                table: "Placanja",
                column: "KlijentId",
                principalTable: "Klijenti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
