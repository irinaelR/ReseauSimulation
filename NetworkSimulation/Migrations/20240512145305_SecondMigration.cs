using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetworkSimulation.Migrations
{
    /// <inheritdoc />
    public partial class SecondMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "serveurs",
                columns: table => new
                {
                    idServeur = table.Column<int>(type: "serial", nullable: false),
                    ipAdress = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    xPos = table.Column<int>(type: "integer", nullable: false),
                    yPos = table.Column<int>(type: "integer", nullable: false),
                    activite = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_serveurs", x => x.idServeur);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "serveurs");
        }
    }
}
