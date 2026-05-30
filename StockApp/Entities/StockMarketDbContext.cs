using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class StockMarketDbContext : DbContext
    {
        public StockMarketDbContext(DbContextOptions options) : base(options) { }

        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuyOrder>().ToTable(nameof(BuyOrders)).ToString();
            modelBuilder.Entity<SellOrder>().ToTable(nameof(SellOrders)).ToString();
        }        

        public async Task<int> InsertBuyOrder(BuyOrder buyOrder)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Id", buyOrder.Id),
                new SqlParameter("@stockName", buyOrder.stockName),
                new SqlParameter("@stockSymbol", buyOrder.stockSymbol),
                new SqlParameter("@shares", buyOrder.shares),
                new SqlParameter("@price", buyOrder.price),
                new SqlParameter("@date", buyOrder.date)
            };

            return await Database.ExecuteSqlRawAsync("EXECUTE [dbo].[AddBuyOrderSP] @Id, @stockName, @stockSymbol, @shares, @price, @date", sqlParameters);
        }

        public async Task<int> InsertSellOrder(SellOrder sellOrder)
        {
            SqlParameter[] sqlParameters = new SqlParameter[] {
                new SqlParameter("@Id", sellOrder.Id),
                new SqlParameter("@stockName", sellOrder.stockName),
                new SqlParameter("@stockSymbol", sellOrder.StockSymbol),
                new SqlParameter("@shares", sellOrder.shares),
                new SqlParameter("@price", sellOrder.price),
                new SqlParameter("@date", sellOrder.date)
            };

            return await Database.ExecuteSqlRawAsync("EXECUTE [dbo].[AddSellOrderSP] @Id, @stockName, @stockSymbol, @shares, @price, @date", sqlParameters);
        }
    }
}
