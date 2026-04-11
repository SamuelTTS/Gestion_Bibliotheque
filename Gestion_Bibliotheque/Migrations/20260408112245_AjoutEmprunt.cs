using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gestion_Bibliotheque.Migrations
{
    /// <inheritdoc />
    public partial class AjoutEmprunt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "available",
                table: "Livres",
                newName: "EstEmprunte");

            migrationBuilder.AddColumn<string>(
                name: "Emprunteur",
                table: "Livres",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Emprunteur",
                table: "Livres");

            migrationBuilder.RenameColumn(
                name: "EstEmprunte",
                table: "Livres",
                newName: "available");
        }
    }
}
