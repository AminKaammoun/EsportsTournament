using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsTour.Migrations
{
    /// <inheritdoc />
    public partial class tournois_foreign_key_jeu : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "JeuId",
                table: "Tournois",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JeuxId",
                table: "Tournois",
                type: "int",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tournois_Jeux_JeuxId",
                table: "Tournois");

            migrationBuilder.DropIndex(
                name: "IX_Tournois_JeuxId",
                table: "Tournois");

            migrationBuilder.DropColumn(
                name: "JeuId",
                table: "Tournois");

            migrationBuilder.DropColumn(
                name: "JeuxId",
                table: "Tournois");
        }
    }
}
