using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MachineryCRM.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_FiscalEntities_FiscalEntityId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Sites_SiteId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Sites_SiteId",
                table: "Machines");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_FiscalEntities_FiscalEntityId",
                table: "Contacts",
                column: "FiscalEntityId",
                principalTable: "FiscalEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Sites_SiteId",
                table: "Contacts",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Sites_SiteId",
                table: "Machines",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_FiscalEntities_FiscalEntityId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Sites_SiteId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Sites_SiteId",
                table: "Machines");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_FiscalEntities_FiscalEntityId",
                table: "Contacts",
                column: "FiscalEntityId",
                principalTable: "FiscalEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Sites_SiteId",
                table: "Contacts",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Sites_SiteId",
                table: "Machines",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
