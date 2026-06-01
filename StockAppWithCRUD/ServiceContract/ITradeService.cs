using ServiceContract.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContract
{
    public interface ITradeService
    {
        public ResponseBuyOrder AddBuyOrder(AddRequestBuyOrder addRequestBuyOrder);

        public List<ResponseBuyOrder> GetBuyOrders();

        public ResponseSellOrder AddSellOrder(AddRequestSellOrder addRequestSellOrder);
        public List<ResponseSellOrder> GetSellOrders();
    }
}
