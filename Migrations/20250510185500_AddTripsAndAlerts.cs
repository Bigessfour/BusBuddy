using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BusBuddy.Migrations
{
    /// <inheritdoc />
    public partial class AddTripsAndAlerts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    TripId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    PassengerCount = table.Column<int>(type: "int", nullable: false),
                    DelayMinutes = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.TripId);
                    table.ForeignKey(
                        name: "FK_Trips_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alerts",
                columns: table => new
                {
                    AlertId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RouteId = table.Column<int>(type: "int", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Severity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alerts", x => x.AlertId);
                    table.ForeignKey(
                        name: "FK_Alerts_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Alerts_RouteId",
                table: "Alerts",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_RouteId",
                table: "Trips",
                column: "RouteId");

            // Seed data
            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "Id", "RouteName", "StartLocation", "EndLocation", "Distance", "CreatedDate", "LastModified", "Description" },
                values: new object[] { 1, "Main Route", "School", "Downtown", 5.2m, DateTime.Now, DateTime.Now, "Main school route" });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "TripId", "RouteId", "PassengerCount", "DelayMinutes", "Status", "IsActive", "LastUpdated" },
                values: new object[] { 1, 1, 25, 0, "OnTime", true, DateTime.Now });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "TripId", "RouteId", "PassengerCount", "DelayMinutes", "Status", "IsActive", "LastUpdated" },
                values: new object[] { 2, 1, 18, 10, "Delayed", true, DateTime.Now });

            migrationBuilder.InsertData(
                table: "Alerts",
                columns: new[] { "AlertId", "RouteId", "Message", "Severity", "IsActive", "CreatedAt" },
                values: new object[] { 1, 1, "Traffic congestion on Main St", "Warning", true, DateTime.Now });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Alerts");

            migrationBuilder.DropTable(
                name: "Trips");
        }
    }
}
