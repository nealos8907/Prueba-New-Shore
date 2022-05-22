using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAcces.Migrations
{
    public partial class thirdMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Journeys_JourneyId",
                table: "Flights");

            migrationBuilder.DropForeignKey(
                name: "FK_Transports_Flights_FlightId",
                table: "Transports");

            migrationBuilder.DropIndex(
                name: "IX_Transports_FlightId",
                table: "Transports");

            migrationBuilder.DropIndex(
                name: "IX_Flights_JourneyId",
                table: "Flights");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Transports");

            migrationBuilder.RenameColumn(
                name: "JourneyId",
                table: "Flights",
                newName: "TransportId");

            migrationBuilder.CreateTable(
                name: "JourneyFlights",
                columns: table => new
                {
                    JourneyFlightId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JourneyId = table.Column<int>(type: "int", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyFlights", x => x.JourneyFlightId);
                    table.ForeignKey(
                        name: "FK_JourneyFlights_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "FlightId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JourneyFlights_Journeys_JourneyId",
                        column: x => x.JourneyId,
                        principalTable: "Journeys",
                        principalColumn: "JourneyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_TransportId",
                table: "Flights",
                column: "TransportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JourneyFlights_FlightId",
                table: "JourneyFlights",
                column: "FlightId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JourneyFlights_JourneyId",
                table: "JourneyFlights",
                column: "JourneyId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Transports_TransportId",
                table: "Flights",
                column: "TransportId",
                principalTable: "Transports",
                principalColumn: "TransportId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flights_Transports_TransportId",
                table: "Flights");

            migrationBuilder.DropTable(
                name: "JourneyFlights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_TransportId",
                table: "Flights");

            migrationBuilder.RenameColumn(
                name: "TransportId",
                table: "Flights",
                newName: "JourneyId");

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Transports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Transports_FlightId",
                table: "Transports",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_JourneyId",
                table: "Flights",
                column: "JourneyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flights_Journeys_JourneyId",
                table: "Flights",
                column: "JourneyId",
                principalTable: "Journeys",
                principalColumn: "JourneyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transports_Flights_FlightId",
                table: "Transports",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
