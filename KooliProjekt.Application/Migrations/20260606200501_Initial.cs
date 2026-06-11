using Microsoft.EntityFrameworkCore.Migrations;
using System.Diagnostics.CodeAnalysis;
#nullable disable

namespace KooliProjekt.Application.Migrations
{
[ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arved_Tellimused_TellimusId",
                table: "Arved");

            migrationBuilder.DropForeignKey(
                name: "FK_TellimusedRida_Tellimused_TellimusId",
                table: "TellimusedRida");

            migrationBuilder.DropForeignKey(
                name: "FK_TellimusedRida_Tooted_ToodeId",
                table: "TellimusedRida");

            migrationBuilder.AddForeignKey(
                name: "FK_Arved_Tellimused_TellimusId",
                table: "Arved",
                column: "TellimusId",
                principalTable: "Tellimused",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TellimusedRida_Tellimused_TellimusId",
                table: "TellimusedRida",
                column: "TellimusId",
                principalTable: "Tellimused",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TellimusedRida_Tooted_ToodeId",
                table: "TellimusedRida",
                column: "ToodeId",
                principalTable: "Tooted",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Arved_Tellimused_TellimusId",
                table: "Arved");

            migrationBuilder.DropForeignKey(
                name: "FK_TellimusedRida_Tellimused_TellimusId",
                table: "TellimusedRida");

            migrationBuilder.DropForeignKey(
                name: "FK_TellimusedRida_Tooted_ToodeId",
                table: "TellimusedRida");

            migrationBuilder.AddForeignKey(
                name: "FK_Arved_Tellimused_TellimusId",
                table: "Arved",
                column: "TellimusId",
                principalTable: "Tellimused",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TellimusedRida_Tellimused_TellimusId",
                table: "TellimusedRida",
                column: "TellimusId",
                principalTable: "Tellimused",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TellimusedRida_Tooted_ToodeId",
                table: "TellimusedRida",
                column: "ToodeId",
                principalTable: "Tooted",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
