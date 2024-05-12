using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkSimulation.Migrations
{
    /// <inheritdoc />
    public partial class CaseChangesMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "yPos",
                table: "serveurs",
                newName: "y");

            migrationBuilder.RenameColumn(
                name: "xPos",
                table: "serveurs",
                newName: "x");

            migrationBuilder.RenameColumn(
                name: "ipAdress",
                table: "serveurs",
                newName: "id_adress");

            migrationBuilder.RenameColumn(
                name: "idServeur",
                table: "serveurs",
                newName: "id_serveur");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "y",
                table: "serveurs",
                newName: "yPos");

            migrationBuilder.RenameColumn(
                name: "x",
                table: "serveurs",
                newName: "xPos");

            migrationBuilder.RenameColumn(
                name: "id_adress",
                table: "serveurs",
                newName: "ipAdress");

            migrationBuilder.RenameColumn(
                name: "id_serveur",
                table: "serveurs",
                newName: "idServeur");
        }
    }
}
