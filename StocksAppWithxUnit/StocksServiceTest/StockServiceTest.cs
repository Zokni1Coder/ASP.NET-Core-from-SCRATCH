namespace Tests
{
    public class StockServiceTest
    {
        //private readonly IStockSert
        public StockServiceTest()
        {
            
        }

        #region CreateBuyOrder
        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public void BuyOrderCreateBuyOrder_NullParam()
        {
            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act

            });
        }

        #endregion
    }
}
