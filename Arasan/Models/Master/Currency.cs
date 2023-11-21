namespace Arasan.Models
{
    public class Currency
    {
        public string ID { get; set; }
        public string CurrencyCode { get; set; }
        public string CurrencyName { get; set; }
        public string status { get; set; }


    }
    public class Currencygrid
    {
        public string id { get; set; }
        public string currencycode { get; set; }
        public string currencyname { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }


    }
}