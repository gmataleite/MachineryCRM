using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MachineryCRM.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDomainModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Sites_SiteId",
                table: "Machines");

            migrationBuilder.DropTable(
                name: "MaintenanceOrders");

            migrationBuilder.DropTable(
                name: "Technicians");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "Cnpj",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "Cpf",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "FiscalAddress",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "Ie",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "Observations",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "PostalAddress",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "SapPn",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "State",
                table: "FiscalEntities",
                newName: "ShippingState");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "FiscalEntities",
                newName: "ShippingCity");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Sites",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Sites",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "AddressLine",
                table: "Sites",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Sites",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Neighborhood",
                table: "Sites",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCode",
                table: "Sites",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Machines",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AddColumn<DateTime>(
                name: "AcquisitionDate",
                table: "Machines",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Machines",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Machines",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "GeoPoints",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LocationType",
                table: "GeoPoints",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "GeoPoints",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ShippingState",
                table: "FiscalEntities",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ShippingCity",
                table: "FiscalEntities",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "BillingAddressLine",
                table: "FiscalEntities",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingCity",
                table: "FiscalEntities",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingCountryCode",
                table: "FiscalEntities",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingNeighborhood",
                table: "FiscalEntities",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingPostalCode",
                table: "FiscalEntities",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BillingState",
                table: "FiscalEntities",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddressLine",
                table: "FiscalEntities",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingCountryCode",
                table: "FiscalEntities",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingNeighborhood",
                table: "FiscalEntities",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingPostalCode",
                table: "FiscalEntities",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TaxId",
                table: "FiscalEntities",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Contacts",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observations",
                table: "Contacts",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Contacts",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Contacts",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransferHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MachineId = table.Column<Guid>(type: "uuid", nullable: false),
                    OriginSiteId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationSiteId = table.Column<Guid>(type: "uuid", nullable: false),
                    TransferDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    LoggedBy = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferHistories_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferHistories_Sites_DestinationSiteId",
                        column: x => x.DestinationSiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferHistories_Sites_OriginSiteId",
                        column: x => x.OriginSiteId,
                        principalTable: "Sites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Communications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MachineId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    InteractionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Channel = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Summary = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Communications_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Communications_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Communications_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maintenances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MachineId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    MaintenanceType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PartsUsed = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Maintenances_AppUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Maintenances_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_Email",
                table: "AppUsers",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Communications_AppUserId",
                table: "Communications",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Communications_ContactId",
                table: "Communications",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Communications_MachineId",
                table: "Communications",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_AppUserId",
                table: "Maintenances",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenances_MachineId",
                table: "Maintenances",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_DestinationSiteId",
                table: "TransferHistories",
                column: "DestinationSiteId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_MachineId",
                table: "TransferHistories",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferHistories_OriginSiteId",
                table: "TransferHistories",
                column: "OriginSiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Sites_SiteId",
                table: "Machines",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Sites_SiteId",
                table: "Machines");

            migrationBuilder.DropTable(
                name: "Communications");

            migrationBuilder.DropTable(
                name: "Maintenances");

            migrationBuilder.DropTable(
                name: "TransferHistories");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropColumn(
                name: "AddressLine",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Neighborhood",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "PostalCode",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "AcquisitionDate",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "BillingAddressLine",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "BillingCity",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "BillingCountryCode",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "BillingNeighborhood",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "BillingPostalCode",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "BillingState",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "ShippingAddressLine",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "ShippingCountryCode",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "ShippingNeighborhood",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "ShippingPostalCode",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "TaxId",
                table: "FiscalEntities");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "ShippingState",
                table: "FiscalEntities",
                newName: "State");

            migrationBuilder.RenameColumn(
                name: "ShippingCity",
                table: "FiscalEntities",
                newName: "City");

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "Sites",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "Sites",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "Sites",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "Sites",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Model",
                table: "Machines",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Machines",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "GeoPoints",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "LocationType",
                table: "GeoPoints",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "GeoPoints",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "State",
                table: "FiscalEntities",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "City",
                table: "FiscalEntities",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cnpj",
                table: "FiscalEntities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "FiscalEntities",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cpf",
                table: "FiscalEntities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FiscalAddress",
                table: "FiscalEntities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ie",
                table: "FiscalEntities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Observations",
                table: "FiscalEntities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalAddress",
                table: "FiscalEntities",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SapPn",
                table: "FiscalEntities",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Contacts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Observations",
                table: "Contacts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Contacts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Contacts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "MaintenanceOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IssueDescription = table.Column<string>(type: "text", nullable: false),
                    MachineId = table.Column<Guid>(type: "uuid", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    TechnicianId = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaintenanceOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Technicians",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "text", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Technicians", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Sites_SiteId",
                table: "Machines",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
