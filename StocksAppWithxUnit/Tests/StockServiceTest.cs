using Microsoft.AspNetCore.Razor.TagHelpers;
using Service;
using ServiceContract;
using ServiceContract.DTOs;
using System.Runtime.ConstrainedExecution;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Tests
{
    public class StockServiceTest
    {
        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _outputHelper;
        public StockServiceTest(ITestOutputHelper testOutputHelper)
        {
            this._stocksService = new StockService();
            this._outputHelper = testOutputHelper;
        }

        #region CreateBuyOrder
        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void CreateBuyOrder_NullParam()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                this._stocksService.CreateBuyOrder(null);
            });
        }

        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_ZeroQuantity()
        {
            //Arrange
            BuyOrderRequest request = new BuyOrderRequest()
            {
                Quantity = 0,
                Price = 5,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateBuyOrder(request);
            });
        }

        //When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_TooMuchQuantity()
        {
            //Arrange
            BuyOrderRequest request = new BuyOrderRequest()
            {
                Quantity = 100001,
                Price = 5,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateBuyOrder(request);
            });
        }

        //. When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_ZeroPrice()
        {
            //Arrange
            BuyOrderRequest request = new BuyOrderRequest()
            {
                Quantity = 100,
                Price = 0,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateBuyOrder(request);
            });
        }

        // When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_TooHighPrice()
        {
            //Arrange
            BuyOrderRequest request = new BuyOrderRequest()
            {
                Quantity = 100,
                Price = 10001,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateBuyOrder(request);

            });
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_NullStockSymbol()
        {
            //Arrange
            BuyOrderRequest request = new BuyOrderRequest()
            {
                Quantity = 100,
                Price = 10001,
                StockName = "asd",
                StockSymbol = null,
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateBuyOrder(request);
            });
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_TooOldDate()
        {
            //Arrange
            BuyOrderRequest request = new BuyOrderRequest()
            {
                Quantity = 100,
                Price = 10001,
                StockName = "asd",
                StockSymbol = "hgfj",
                DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31")
            };

            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateBuyOrder(request);
            });
        }
        // If you supply all valid values, it should be successful and return an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public void CreateBuyOrder_ProperObject()
        {
            //Arrange
            BuyOrderRequest request = new BuyOrderRequest()
            {
                Quantity = 100,
                Price = 1500,
                StockName = "asd",
                StockSymbol = "hgfj",
                DateAndTimeOfOrder = Convert.ToDateTime("2005-05-18")
            };

            //Act
            BuyOrderResponse buyOrder_from_Create = this._stocksService.CreateBuyOrder(request);
            this._outputHelper.WriteLine(buyOrder_from_Create.ToString());

            //Assert
            Assert.NotNull(buyOrder_from_Create.BuyOrderId);
        }
        #endregion

        #region CreateSellOrder

        //When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void SellOrder_NullParam()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                this._stocksService.CreateSellOrder(null);
            });
        }

        //When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void SellOrder_ZeroQuantity()
        {
            //Arrange
            SellOrderRequest request = new SellOrderRequest()
            {
                DateAndTimeOfOrder = Convert.ToDateTime("2005-05-18"),
                Quantity = 0,
                Price = 5,
                StockName = "ads",
                StockSymbol = "fgh"
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateSellOrder(request);
            });
        }

        //When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void SellOrder_TooMuchQuantity()
        {
            //Arrange
            SellOrderRequest request = new SellOrderRequest()
            {
                DateAndTimeOfOrder = Convert.ToDateTime("2005-05-18"),
                Quantity = 100001,
                Price = 5,
                StockName = "ads",
                StockSymbol = "fgh"
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateSellOrder(request);
            });
        }

        //When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void SellOrder_ZeroPrice()
        {
            //Arrange
            SellOrderRequest request = new SellOrderRequest()
            {
                DateAndTimeOfOrder = Convert.ToDateTime("2005-05-18"),
                Quantity = 1001,
                Price = 0,
                StockName = "ads",
                StockSymbol = "fgh"
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateSellOrder(request);
            });
        }


        //When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void SellOrder_TooHighPrice()
        {
            //Arrange
            SellOrderRequest request = new SellOrderRequest()
            {
                DateAndTimeOfOrder = Convert.ToDateTime("2005-05-18"),
                Quantity = 1001,
                Price = 10001,
                StockName = "ads",
                StockSymbol = "fgh"
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateSellOrder(request);
            });
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void SellOrder_NullStockSymbol()
        {
            //Arrange
            SellOrderRequest request = new SellOrderRequest()
            {
                DateAndTimeOfOrder = Convert.ToDateTime("2005-05-18"),
                Quantity = 1001,
                Price = 100,
                StockName = "ads",
                StockSymbol = null
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateSellOrder(request);
            });
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification, it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void SellOrder_TooOldDate()
        {
            //Arrange
            SellOrderRequest request = new SellOrderRequest()
            {
                DateAndTimeOfOrder = Convert.ToDateTime("1999-12-31"),
                Quantity = 1001,
                Price = 100,
                StockName = "ads",
                StockSymbol = "ghf"
            };
            //Assert
            Assert.Throws<ArgumentException>(() =>
            {
                //Act
                this._stocksService.CreateSellOrder(request);
            });
        }
        //If you supply all valid values, it should be successful and return an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public void SellOrderCreate_ProperObject()
        {
            //Arrange
            SellOrderRequest request = new SellOrderRequest()
            {
                Quantity = 100,
                Price = 1500,
                StockName = "asd",
                StockSymbol = "hgfj",
                DateAndTimeOfOrder = Convert.ToDateTime("2005-05-18")
            };

            //Act
            SellOrderResponse selllOrder_from_Create = this._stocksService.CreateSellOrder(request);
            this._outputHelper.WriteLine(selllOrder_from_Create.ToString());

            //Assert
            Assert.NotNull(selllOrder_from_Create.SellOrderId);
        }


        #endregion

        #region GetAllBuyOrders

        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public void GetAllBuyOrders_DefaultEmppty()
        {
            //Act
            List<BuyOrderResponse> orders_from_GetAll = this._stocksService.GetBuyOrders();

            //Assert
            Assert.Empty(orders_from_GetAll);
        }


        //When you first add few buy orders using CreateBuyOrder() method; and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public void GetAllBuyOrders_FewOrder()
        {
            //Arrange
            BuyOrderRequest request1 = new BuyOrderRequest()
            {
                Quantity = 10,
                Price = 5,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };
            BuyOrderRequest request2 = new BuyOrderRequest()
            {
                Quantity = 10,
                Price = 5,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };
            BuyOrderRequest request3 = new BuyOrderRequest()
            {
                Quantity = 10,
                Price = 5,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };
            BuyOrderRequest request4 = new BuyOrderRequest()
            {
                Quantity = 10,
                Price = 5,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };
            //Act
            BuyOrderResponse response1 = this._stocksService.CreateBuyOrder(request1);
            BuyOrderResponse response2 = this._stocksService.CreateBuyOrder(request2);
            BuyOrderResponse response3 = this._stocksService.CreateBuyOrder(request3);
            BuyOrderResponse response4 = this._stocksService.CreateBuyOrder(request4);

            List<BuyOrderResponse> orders_from_Create = new List<BuyOrderResponse>()
            {
                response1, response2, response3, response4
            };
            List<BuyOrderResponse> orders_from_GetAll = this._stocksService.GetBuyOrders();

            this._outputHelper.WriteLine("Actual");
            foreach (BuyOrderResponse order in orders_from_Create)
            {
                this._outputHelper.WriteLine(order.ToString());
            }
            this._outputHelper.WriteLine("Expected");
            foreach (BuyOrderResponse order in orders_from_GetAll)
            {
                this._outputHelper.WriteLine(order.ToString());
            }

            //Assert
            foreach (BuyOrderResponse order in orders_from_Create)
            {
                Assert.Contains(order, orders_from_GetAll);
            }
        }
        #endregion

        #region GetAllSellOrders

        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public void GetAllSellOrders_DefaultEmpty()
        {
            //Act
            List<SellOrderResponse> orders_from_GetAll = this._stocksService.GetSellOrders();

            //Assert
            Assert.Empty(orders_from_GetAll);
        }

        //When you first add few sell orders using CreateSellOrder() method; and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public void GetAllSellOrders_FewOrder()
        {
            //Arrange
            SellOrderRequest request1 = new SellOrderRequest()
            {
                Quantity = 10,
                Price = 5,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };
            SellOrderRequest request2 = new SellOrderRequest()
            {
                Quantity = 10,
                Price = 5,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };
            SellOrderRequest request3 = new SellOrderRequest()
            {
                Quantity = 10,
                Price = 5,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };
            SellOrderRequest request4 = new SellOrderRequest()
            {
                Quantity = 10,
                Price = 5,
                StockName = "asd",
                StockSymbol = "fgh",
                DateAndTimeOfOrder = Convert.ToDateTime("2000-09-22")
            };
            //Act
            SellOrderResponse response1 = this._stocksService.CreateSellOrder(request1);
            SellOrderResponse response2 = this._stocksService.CreateSellOrder(request2);
            SellOrderResponse response3 = this._stocksService.CreateSellOrder(request3);
            SellOrderResponse response4 = this._stocksService.CreateSellOrder(request4);

            List<SellOrderResponse> orders_from_Create = new List<SellOrderResponse>()
            {
                response1, response2, response3, response4
            };
            List<SellOrderResponse> orders_from_GetAll = this._stocksService.GetSellOrders();

            this._outputHelper.WriteLine("Actual");
            foreach (SellOrderResponse order in orders_from_Create)
            {
                this._outputHelper.WriteLine(order.ToString());
            }
            this._outputHelper.WriteLine("Expected");
            foreach (SellOrderResponse order in orders_from_GetAll)
            {
                this._outputHelper.WriteLine(order.ToString());
            }

            //Assert
            foreach (SellOrderResponse order in orders_from_Create)
            {
                Assert.Contains(order, orders_from_GetAll);
            }
        }
        #endregion
    }
}
