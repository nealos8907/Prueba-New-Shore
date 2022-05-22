using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAcces.Migrations
{
    public partial class FixRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JourneyFlights_FlightId",
                table: "JourneyFlights");

            migrationBuilder.DropIndex(
                name: "IX_JourneyFlights_JourneyId",
                table: "JourneyFlights");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyFlights_FlightId",
                table: "JourneyFlights",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyFlights_JourneyId",
                table: "JourneyFlights",
                column: "JourneyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_JourneyFlights_FlightId",
                table: "JourneyFlights");

            migrationBuilder.DropIndex(
                name: "IX_JourneyFlights_JourneyId",
                table: "JourneyFlights");

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
        }
    }
}
