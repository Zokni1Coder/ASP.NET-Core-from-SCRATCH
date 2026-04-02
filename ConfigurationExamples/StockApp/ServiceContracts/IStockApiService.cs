namespace StockApp.ServiceContracts
{
    public interface IStockApiService
    {
        Task<Dictionary<string, object>?> GetStocks(string quoteSymbol);
    }
}
