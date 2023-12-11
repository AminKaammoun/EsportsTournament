using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsTour.Migrations
{
    /// <inheritdoc />
    public partial class tournois_foreign_key_jeu_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournois_Jeux_JeuxId",
                table: "Tournois");

            migrationBuilder.DropIndex(
                name: "IX_Tournois_JeuxId",
                table: "Tournois");

            migrationBuilder.DropColumn(
                name: "JeuxId",
                table: "Tournois");

            migrationBuilder.DropColumn(
                name: "jeu",
                table: "Tournois");

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Tournois",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.CreateIndex(
                name: "IX_Tournois_JeuId",
                table: "Tournois",
                column: "JeuId");

            migrationBuilder.AddForeignKey(
                name: "FK__Tournois__JeuId__3B75D760",
                table: "Tournois",
                column: "JeuId",
                principalTable: "Jeux",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Tournois__JeuId__3B75D760",
                table: "Tournois");

            migrationBuilder.DropIndex(
                name: "IX_Tournois_JeuId",
                table: "Tournois");

            migrationBuilder.AlterColumn<string>(
                name: "Nom",
                table: "Tournois",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "JeuxId",
                table: "Tournois",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "jeu",
                table: "Tournois",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Tournois_JeuxId",
                table: "Tournois",
                column: "JeuxId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tournois_Jeux_JeuxId",
                table: "Tournois",
                column: "JeuxId",
                principalTable: "Jeux",
                principalColumn: "Id");
        }
    }
}
