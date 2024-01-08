using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class Stock
    {
        public long id { get; set; }
        public string itemname { get; set; }
        public double quantity { get; set; }
        public string branchname { get; set; }
        public string location { get; set; }
        public string unit { get; set; }
        public string indentno { get; set; }
        public string indentdate { get; set; }
        public double IssuedQty { get; set; }
        public int stockavailable { get; set; }

    } 
    
    public class StockGrid
    {
        public long id { get; set; }
        public string itemname { get; set; }
        public string binid { get; set; }
        public double quantity { get; set; }
        public string branchname { get; set; }
        public string location { get; set; }
        public string unit { get; set; }
        public string indentno { get; set; }
        public string indentdate { get; set; }
        public double IssuedQty { get; set; }
        public int stockavailable { get; set; }

    }
    public class Asset
    {
        public long id { get; set; }
        public string itemname { get; set; }
        public double quantity { get; set; }
        public string plmi { get; set; }
        public string loc { get; set; }
        public string type { get; set; }
        public string indentno { get; set; }
        public string indentdate { get; set; }
        public double IssuedQty { get; set; }
        public int stockavailable { get; set; }

    }
}
