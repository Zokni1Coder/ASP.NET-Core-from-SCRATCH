using Microsoft.Extensions.Options;
using ServiceContract;
using ServiceContract.DTOs;
using Services;
using StockAppWithCRUD.Option_Pattern;
using Xunit.Abstractions;

namespace StockTest
{
    public class StockUnitTest
    {
        private readonly ITradeService _tradeService;
        private readonly ITestOutputHelper _testOutputHelper;
        public StockUnitTest(ITestOutputHelper testOutputHelper)
        {
            this._tradeService = new TradeService();
            this._testOutputHelper = testOutputHelper;
        }

        #region AddBuyOrder
        [Fact]
        public void NullParam_AddBuyOrder()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                this._tradeService.AddBuyOrder(null);
            });
        }


        [Fact]
        public void NullQuantity_AddBuyOrder()
        {
            //Arrange
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                companyName = "MSFT",
                shares = 0,
                price = 13,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._tradeService.AddBuyOrder(addRequestBuyOrder);
            });
        }

        [Fact]
        public void TooMuchQuantity_AddBuyOrder()
        {
            //Arrange
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                companyName = "MSFT",
                shares = 100001,
                price = 13,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._tradeService.AddBuyOrder(addRequestBuyOrder);
            });
        }

        [Fact]
        public void NullPrice_AddBuyOrder()
        {
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                companyName = "MSFT",
                shares = 10000,
                price = 0
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._tradeService.AddBuyOrder(addRequestBuyOrder);
            });
        }

        [Fact]
        public void TooHighPrice_AddBuyOrder()
        {
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                companyName = "MSFT",
                shares = 10000,
                price = 10001,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._tradeService.AddBuyOrder(addRequestBuyOrder);
            });
        }

        [Fact]
        public void ProperObject_AddBuyOrder()
        {
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                companyName = "MSFT",
                shares = 10000,
                price = 100,
            };

            //Act
            ResponseBuyOrder responseBuyOrder = this._tradeService.AddBuyOrder(addRequestBuyOrder);

            //Assert
            this._testOutputHelper.WriteLine(responseBuyOrder.ToString());
            Assert.NotNull(responseBuyOrder);
        }

        #endregion

        #region AddSellOrder
        [Fact]
        public void NullParam_AddSellOrder()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                this._tradeService.AddSellOrder(null);
            });
        }


        [Fact]
        public void NullQuantity_AddSellOrder()
        {
            //Arrange
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                companyName = "MSFT",
                shares = 0,
                price = 13,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._tradeService.AddSellOrder(addRequestSellOrder);
            });
        }

        [Fact]
        public void TooMuchQuantity_AddSellOrder()
        {
            //Arrange
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                companyName = "MSFT",
                shares = 100001,
                price = 13,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._tradeService.AddSellOrder(addRequestSellOrder);
            });
        }

        [Fact]
        public void NullPrice_AddSellOrder()
        {
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                companyName = "MSFT",
                shares = 10000,
                price = 0
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._tradeService.AddSellOrder(addRequestSellOrder);
            });
        }

        [Fact]
        public void TooHighPrice_AddSellOrder()
        {
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                companyName = "MSFT",
                shares = 10000,
                price = 10001,
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._tradeService.AddSellOrder(addRequestSellOrder);
            });
        }

        [Fact]
        public void ProperObject_AddSellOrder()
        {
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                companyName = "MSFT",
                shares = 10000,
                price = 100,
            };

            //Act
            ResponseSellOrder responseSellOrder = this._tradeService.AddSellOrder(addRequestSellOrder);

            //Assert
            this._testOutputHelper.WriteLine(addRequestSellOrder.ToString());
            Assert.NotNull(addRequestSellOrder);
        }

        #endregion
    }
}
