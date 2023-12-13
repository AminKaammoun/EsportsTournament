using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsTour.Migrations
{
    /// <inheritdoc />
    public partial class addImageToEquipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Joueurs_Users_UserId1",
                table: "Joueurs");

            migrationBuilder.DropIndex(
                name: "IX_Joueurs_UserId1",
                table: "Joueurs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Joueurs");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "Joueurs");

            migrationBuilder.AddColumn<string>(
                name: "image",
                table: "Equipes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "image",
                table: "Equipes");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Joueurs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId1",
                table: "Joueurs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Joueurs_UserId1",
                table: "Joueurs",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Joueurs_Users_UserId1",
                table: "Joueurs",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
