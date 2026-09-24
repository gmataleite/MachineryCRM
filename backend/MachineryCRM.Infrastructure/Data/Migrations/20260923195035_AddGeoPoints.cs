using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MachineryCRM.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddGeoPoints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Coordinates",
                table: "GeoPoints");

            migrationBuilder.DropColumn(
                name: "IsMachineLocation",
                table: "GeoPoints");

            migrationBuilder.DropColumn(
                name: "IsOffice",
                table: "GeoPoints");

            migrationBuilder.DropColumn(
                name: "IsWaypoint",
                table: "GeoPoints");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "GeoPoints",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "LocationType",
                table: "GeoPoints",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "GeoPoints",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "GeoPoints",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "GeoPoints");

            migrationBuilder.DropColumn(
                name: "LocationType",
                table: "GeoPoints");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "GeoPoints");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "GeoPoints");

            migrationBuilder.AddColumn<string>(
                name: "Coordinates",
                table: "GeoPoints",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsMachineLocation",
                table: "GeoPoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsOffice",
                table: "GeoPoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWaypoint",
                table: "GeoPoints",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
