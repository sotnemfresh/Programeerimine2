using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KooliProjekt.Application.Migrations
{
    /// <inheritdoc />
    public partial class InitialUS : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Perenimi",
                table: "Kliendid",
                newName: "LastName");

            migrationBuilder.RenameColumn(
                name: "Eesnimi",
                table: "Kliendid",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LastName",
                table: "Kliendid",
                newName: "Perenimi");

            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "Kliendid",
                newName: "Eesnimi");
        }
    }
}
