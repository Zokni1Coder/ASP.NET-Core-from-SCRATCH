namespace StockAppWithCRUD.ViewModels
{
    public class StockViewModel
    {
        public Dictionary<string, object>? Quotes { get; set; }
        public Dictionary<string, object>? Profile { get; set; }        
        public string? Name { get; set; }
        public double Price { get; set; }        
    }
}
