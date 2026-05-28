using System.ComponentModel.DataAnnotations;

namespace StockAppWithCRUD.Option_Pattern
{
    public class Config_OptionPattern
    {
        [Required(ErrorMessage = "The symbol can't be blank.")]
        public string? symbol { get; set; }
        [Required(ErrorMessage = "The Token can't be blank.")]
        public string? token { get; set; }
    }
}
