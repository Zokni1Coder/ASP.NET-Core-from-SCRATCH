using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using ServiceContract;
using ServiceContract.DTOs;
using StockApp.Models;

namespace StockApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService, StockMarketDbContext stockMarketDbContext)
        {
            this._tradeService = tradeService;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> BuyOrder(AddRequestBuyOrder addRequestBuyOrder)
        {
            ResponseBuyOrder responseBuyOrder = await this._tradeService.AddBuyOrder(addRequestBuyOrder);

            return RedirectToAction(nameof(StockController.Index), "stock");
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> SellOrder(AddRequestSellOrder addRequestSellOrder)
        {
            ResponseSellOrder responseSellOrder = await this._tradeService.AddSellOrder(addRequestSellOrder);

            return RedirectToAction(nameof(StockController.Index), "stock");
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Orders()
        {
            Trades trades = new Trades()
            {
                buyOrders = await this._tradeService.GetBuyOrders(),
                sellOrders = await this._tradeService.GetSellOrders()

            };

            return View(trades);
        }
        [HttpGet("[action]")]
        public async Task<IActionResult> OrdersToPDF()
        {
            Trades trades = new Trades();
            trades.sellOrders = await this._tradeService.GetSellOrders();
            trades.buyOrders = await this._tradeService.GetBuyOrders();

            return new ViewAsPdf("OrdersToPDF", trades, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins()
                {
                    Top = 20,
                    Left = 20,
                    Right = 20,
                    Bottom = 20
                },

                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }
    }
}
