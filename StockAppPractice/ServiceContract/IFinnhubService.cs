namespace ServiceContract
{
    public interface IFinnhubService
    {
        Task<Dictionary<string, object>?> GetStockProfile(string stockSymbol);
        Task<Dictionary<string, object>?> GetStockQuote(string stockSymbol);
    }
}
