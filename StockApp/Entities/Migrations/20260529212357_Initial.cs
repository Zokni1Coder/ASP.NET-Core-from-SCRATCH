using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BuyOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stockName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    stockSymbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    shares = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BuyOrders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    stockName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    StockSymbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    shares = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellOrders", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BuyOrders");

            migrationBuilder.DropTable(
                name: "SellOrders");
        }
    }
}
