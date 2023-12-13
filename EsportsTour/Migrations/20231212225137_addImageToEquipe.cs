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

           
        }
    }
}
