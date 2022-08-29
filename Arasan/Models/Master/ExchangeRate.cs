using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class ExchangeRate
    {
        public int Id { get; set; }
        public string CurrencyName { get; set; }
        public int ExchangeAmount { get; set; }
        public string ExchangeDate { get; set; }
    }
}
