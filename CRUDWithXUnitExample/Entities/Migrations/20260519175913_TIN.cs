using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class TIN : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //Ez generálódik a Fluent API-ból.
            migrationBuilder.AddColumn<string>(
                name: "TaxIdentificName",
                table: "Persons",
                type: "varchar(8)",
                nullable: true,
                defaultValue: "DefualtValue");           
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaxIdentificName",
                table: "Persons");
        }
    }
}
