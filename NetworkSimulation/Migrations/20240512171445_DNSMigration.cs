using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetworkSimulation.Migrations
{
    /// <inheritdoc />
    public partial class DNSMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dns",
                columns: table => new
                {
                    id_mapping = table.Column<int>(type: "serial", nullable: false),
                    id_serveur = table.Column<int>(type: "integer", nullable: false),
                    url = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dns", x => x.id_mapping);
                    table.ForeignKey(
                        name: "FK_dns_serveurs_id_serveur",
                        column: x => x.id_serveur,
                        principalTable: "serveurs",
                        principalColumn: "id_serveur",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_dns_id_serveur",
                table: "dns",
                column: "id_serveur");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dns");
        }
    }
}
