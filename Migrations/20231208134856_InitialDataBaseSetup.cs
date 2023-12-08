using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EsportsTour.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataBaseSetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Equipes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NomEquipe = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Equipes__DC0A3743F019E1C7", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserLogin<string>",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserLogin<string>", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserRole<string>",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserRole<string>", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserToken<string>",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserToken<string>", x => new { x.UserId, x.LoginProvider, x.Name });
                });

            migrationBuilder.CreateTable(
                name: "Tournois",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nom = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Descr = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    jeu = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateDebut = table.Column<DateTime>(type: "date", nullable: true),
                    DateFin = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Tournois__6536E3D9E8BDE8DD", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Joueurs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pseudonyme = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    DateNaissance = table.Column<DateTime>(type: "date", nullable: true),
                    EquipeId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Joueurs__D6CEE2407FCE1182", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Joueurs__EquipeI__3B75D760",
                        column: x => x.EquipeId,
                        principalTable: "Equipes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Resultats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TournoiId = table.Column<int>(type: "int", nullable: true),
                    EquipeGagnanteId = table.Column<int>(type: "int", nullable: true),
                    EquipePerdanteId = table.Column<int>(type: "int", nullable: true),
                    ScoreGagnant = table.Column<int>(type: "int", nullable: true),
                    ScorePerdant = table.Column<int>(type: "int", nullable: true),
                    DateMatch = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Resultat__20BF3E6BBAD92AB0", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Resultats__Equip__3F466844",
                        column: x => x.EquipeGagnanteId,
                        principalTable: "Equipes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Resultats__Equip__403A8C7D",
                        column: x => x.EquipePerdanteId,
                        principalTable: "Equipes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK__Resultats__Tourn__3E52440B",
                        column: x => x.TournoiId,
                        principalTable: "Tournois",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Joueurs_EquipeId",
                table: "Joueurs",
                column: "EquipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Resultats_EquipeGagnanteId",
                table: "Resultats",
                column: "EquipeGagnanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Resultats_EquipePerdanteId",
                table: "Resultats",
                column: "EquipePerdanteId");

            migrationBuilder.CreateIndex(
                name: "IX_Resultats_TournoiId",
                table: "Resultats",
                column: "TournoiId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IdentityUserLogin<string>");

            migrationBuilder.DropTable(
                name: "IdentityUserRole<string>");

            migrationBuilder.DropTable(
                name: "IdentityUserToken<string>");

            migrationBuilder.DropTable(
                name: "Joueurs");

            migrationBuilder.DropTable(
                name: "Resultats");

            migrationBuilder.DropTable(
                name: "Equipes");

            migrationBuilder.DropTable(
                name: "Tournois");
        }
    }
}
