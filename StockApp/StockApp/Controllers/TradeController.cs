using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using ServiceContract.DTOs;

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
            List<ResponseBuyOrder>? buyOrders = this._tradeService.GetBuyOrders();
            List<ResponseSellOrder>? sellOrders = this._tradeService.GetSellOrders();

            ViewBag.BuyOrders = buyOrders;
            ViewBag.SellOrders = sellOrders;

            return View();
        }


    }
}
