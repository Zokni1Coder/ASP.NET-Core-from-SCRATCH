using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddingStoredProcedures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string AddBuyOrder = @"CREATE PROCEDURE [dbo].[AddBuyOrderSP] (@Id uniqueidentifier, @stockName nvarchar(40), @stockSymbol nvarchar(10), @shares int, @price float, @date datetime) AS BEGIN INSERT INTO [dbo].[BuyOrders](Id, stockName, stockSymbol, shares, price, date) VALUES(@Id, @stockName, @stockSymbol, @shares, @price, @date) END";

            migrationBuilder.Sql(AddBuyOrder);

            string AddSellOrder = @"CREATE PROCEDURE [dbo].[AddSellOrderSP] (@Id uniqueidentifier, @stockName nvarchar(40), @stockSymbol nvarchar(10), @shares int, @price float, @date datetime) AS BEGIN INSERT INTO [dbo].[SellOrders](Id, stockName, stockSymbol, shares, price, date) VALUES(@Id, @stockName, @stockSymbol, @shares, @price, @date) END";

            migrationBuilder.Sql(AddSellOrder);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
