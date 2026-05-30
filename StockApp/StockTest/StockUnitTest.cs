using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using ServiceContract;
using ServiceContract.DTOs;
using Services;
using Xunit.Abstractions;

namespace StockTest
{
    public class StockUnitTest
    {
        private readonly ITradeService _tradeService;
        private readonly ITestOutputHelper _testOutputHelper;
        public StockUnitTest(ITestOutputHelper testOutputHelper)
        {
            StockMarketDbContext stockMarketDbContext = new StockMarketDbContext(new DbContextOptionsBuilder<StockMarketDbContext>().Options);
            this._testOutputHelper = testOutputHelper;
        }

        #region AddBuyOrder
        [Fact]
        public async Task NullParam_AddBuyOrder()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await this._tradeService.AddBuyOrder(null);
            });
        }


        [Fact]
        public async Task NullQuantity_AddBuyOrder()
        {
            //Arrange
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                stockName = "MSFT",
                shares = 0,
                price = 13,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._tradeService.AddBuyOrder(addRequestBuyOrder);
            });
        }

        [Fact]
        public async Task TooMuchQuantity_AddBuyOrder()
        {
            //Arrange
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                stockName = "MSFT",
                shares = 100001,
                price = 13,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._tradeService.AddBuyOrder(addRequestBuyOrder);
            });
        }

        [Fact]
        public async Task NullPrice_AddBuyOrder()
        {
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 0
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._tradeService.AddBuyOrder(addRequestBuyOrder);
            });
        }

        [Fact]
        public async Task TooHighPrice_AddBuyOrder()
        {
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 10001,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._tradeService.AddBuyOrder(addRequestBuyOrder);
            });
        }

        [Fact]
        public async Task ProperObject_AddBuyOrder()
        {
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100,
            };

            //Act
            ResponseBuyOrder responseBuyOrder = await this._tradeService.AddBuyOrder(addRequestBuyOrder);

            //Assert
            this._testOutputHelper.WriteLine(responseBuyOrder.ToString());
            Assert.NotNull(responseBuyOrder);
        }

        [Fact]
        public async Task TooOldDate_AddBuyOrder()
        {
            //Arrange
            AddRequestBuyOrder addRequestBuyOrder = new AddRequestBuyOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100,
                date = Convert.ToDateTime("1999-12-12")
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._tradeService.AddBuyOrder(addRequestBuyOrder);
            });
        }

        [Fact]
        public async Task GetAllBuyOrder()
        {
            //Arrange
            AddRequestBuyOrder addRequestBuyOrder1 = new AddRequestBuyOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100
            };
            AddRequestBuyOrder addRequestBuyOrder4 = new AddRequestBuyOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100
            };
            AddRequestBuyOrder addRequestBuyOrder2 = new AddRequestBuyOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100
            };
            AddRequestBuyOrder addRequestBuyOrder3 = new AddRequestBuyOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100
            };

            List<ResponseBuyOrder> buyOrders_fromAdding = new List<ResponseBuyOrder>();

            buyOrders_fromAdding.Add(await this._tradeService.AddBuyOrder(addRequestBuyOrder1));
            buyOrders_fromAdding.Add(await this._tradeService.AddBuyOrder(addRequestBuyOrder2));
            buyOrders_fromAdding.Add(await this._tradeService.AddBuyOrder(addRequestBuyOrder3));
            buyOrders_fromAdding.Add(await this._tradeService.AddBuyOrder(addRequestBuyOrder4));

            //Act
            List<ResponseBuyOrder> buyOrders_form_GetAll = await this._tradeService.GetBuyOrders();

            this._testOutputHelper.WriteLine("Actual");
            foreach (ResponseBuyOrder order in buyOrders_fromAdding)
            {
                this._testOutputHelper.WriteLine(order.ToString());
                //Assert
                Assert.Contains(order, buyOrders_form_GetAll);
            }
            this._testOutputHelper.WriteLine("Expected");
            foreach (var item in buyOrders_form_GetAll)
            {
                this._testOutputHelper.WriteLine(item.ToString());
            }
        }

        #endregion

        #region AddSellOrder
        [Fact]
        public async Task NullParam_AddSellOrder()
        {
            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await this._tradeService.AddSellOrder(null);
            });
        }


        [Fact]
        public async Task NullQuantity_AddSellOrder()
        {
            //Arrange
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                stockName = "MSFT",
                shares = 0,
                price = 13,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._tradeService.AddSellOrder(addRequestSellOrder);
            });
        }

        [Fact]
        public async Task TooMuchQuantity_AddSellOrder()
        {
            //Arrange
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                stockName = "MSFT",
                shares = 100001,
                price = 13,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._tradeService.AddSellOrder(addRequestSellOrder);
            });
        }

        [Fact]
        public async Task NullPrice_AddSellOrder()
        {
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 0
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._tradeService.AddSellOrder(addRequestSellOrder);
            });
        }

        [Fact]
        public async Task TooHighPrice_AddSellOrder()
        {
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 10001,
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._tradeService.AddSellOrder(addRequestSellOrder);
            });
        }

        [Fact]
        public async Task ProperObject_AddSellOrder()
        {
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100,
            };

            //Act
            ResponseSellOrder responseSellOrder = await this._tradeService.AddSellOrder(addRequestSellOrder);

            //Assert
            this._testOutputHelper.WriteLine(addRequestSellOrder.ToString());
            Assert.NotNull(addRequestSellOrder);
        }

        [Fact]
        public async Task TooOldDate_AddSellOrder()
        {
            //Arrange
            AddRequestSellOrder addRequestSellOrder = new AddRequestSellOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100,
                date = Convert.ToDateTime("1999-12-12")
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await this._tradeService.AddSellOrder(addRequestSellOrder);
            });
        }

        [Fact]
        public async Task GetAllSellOrder()
        {
            //Arrange
            AddRequestSellOrder addRequestSellOrder1 = new AddRequestSellOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100
            };
            AddRequestSellOrder addRequestSellOrder2 = new AddRequestSellOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100
            }; AddRequestSellOrder addRequestSellOrder3 = new AddRequestSellOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100
            }; AddRequestSellOrder addRequestSellOrder4 = new AddRequestSellOrder()
            {
                stockName = "MSFT",
                shares = 10000,
                price = 100
            };

            List<ResponseSellOrder> sellOrders_fromAdding = new List<ResponseSellOrder>();

            sellOrders_fromAdding.Add(await this._tradeService.AddSellOrder(addRequestSellOrder1));
            sellOrders_fromAdding.Add(await this._tradeService.AddSellOrder(addRequestSellOrder2));
            sellOrders_fromAdding.Add(await this._tradeService.AddSellOrder(addRequestSellOrder3));
            sellOrders_fromAdding.Add(await this._tradeService.AddSellOrder(addRequestSellOrder4));

            //Act
            List<ResponseSellOrder> sellOrders_form_GetAll = await this._tradeService.GetSellOrders();

            this._testOutputHelper.WriteLine("Actual");
            foreach (ResponseSellOrder order in sellOrders_fromAdding)
            {
                this._testOutputHelper.WriteLine(order.ToString());
                //Assert
                Assert.Contains(order, sellOrders_form_GetAll);
            }
            this._testOutputHelper.WriteLine("Expected");
            foreach (var item in sellOrders_form_GetAll)
            {
                this._testOutputHelper.WriteLine(item.ToString());
            }
        }
        #endregion
    }
}
