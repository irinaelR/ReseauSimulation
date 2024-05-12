using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkSimulation.Migrations
{
    /// <inheritdoc />
    public partial class TypoFix1Migration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id_adress",
                table: "serveurs",
                newName: "ip_adress");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ip_adress",
                table: "serveurs",
                newName: "id_adress");
        }
    }
}
