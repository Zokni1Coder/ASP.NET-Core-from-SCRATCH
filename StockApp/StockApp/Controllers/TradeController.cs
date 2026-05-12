using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using ServiceContract.DTOs;
using StockApp.Models;

namespace StockApp.Controllers
{
    [Route("[controller]")]
    public class TradeController : Controller
    {
        private readonly ITradeService _tradeService;

        public TradeController(ITradeService tradeService)
        {
            this._tradeService = tradeService;
        }
        [HttpPost("[action]")]
        public IActionResult BuyOrder(AddRequestBuyOrder addRequestBuyOrder)
        {
            ResponseBuyOrder responseBuyOrder = this._tradeService.AddBuyOrder(addRequestBuyOrder);

            return RedirectToAction(nameof(StockController.Index), "stock");
        }
        [HttpPost("[action]")]
        public IActionResult SellOrder(AddRequestSellOrder addRequestSellOrder)
        {
            ResponseSellOrder responseSellOrder = this._tradeService.AddSellOrder(addRequestSellOrder);

            return RedirectToAction(nameof(StockController.Index), "stock");
        }

        [HttpGet("[action]")]
        public IActionResult Orders()
        {
            Trades trades = new Trades()
            {
                buyOrders = this._tradeService.GetBuyOrders(),
                sellOrders = this._tradeService.GetSellOrders()
                
            };                       

            return View(trades);
        }


    }
}
