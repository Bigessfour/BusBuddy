using Microsoft.EntityFrameworkCore.Migrations;

namespace BusBuddy.Migrations
{
    public partial class AddPerformanceIndices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add index for RouteData.AMDriverId to improve foreign key lookup performance
            migrationBuilder.CreateIndex(
                name: "IX_RouteData_AMDriverId",
                table: "RouteData",
                column: "AMDriverId");

            // Add index for RouteData.PMDriverId to improve foreign key lookup performance
            migrationBuilder.CreateIndex(
                name: "IX_RouteData_PMDriverId",
                table: "RouteData",
                column: "PMDriverId");

            // Add index for Drivers.LicenseExpiration to improve license expiration queries
            migrationBuilder.CreateIndex(
                name: "IX_Drivers_LicenseExpiration",
                table: "Drivers",
                column: "LicenseExpiration");
            
            // Add index for Vehicle.AssignedDriverId to improve lookup performance
            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_AssignedDriverId",
                table: "Vehicles",
                column: "AssignedDriverId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the indices if the migration is rolled back
            migrationBuilder.DropIndex(
                name: "IX_RouteData_AMDriverId",
                table: "RouteData");

            migrationBuilder.DropIndex(
                name: "IX_RouteData_PMDriverId",
                table: "RouteData");

            migrationBuilder.DropIndex(
                name: "IX_Drivers_LicenseExpiration",
                table: "Drivers");
                
            migrationBuilder.DropIndex(
                name: "IX_Vehicles_AssignedDriverId",
                table: "Vehicles");
        }
    }
}
