using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class StockIn
    {
        public string ID { get; set; }
        public string Item { get; set; }
        public string Unit { get; set; }
        public string ItemID { get; set; }
        public string Qty { get; set; }
         public string Branch { get; set; }
        public string Location { get; set; }
        public List<IndentList> Indentlist { get; set; }
       
    }
    public class IndentList
    {
        public string IndentNo { get; set; }
        public string IndentDate { get; set; }
        public string ItemName { get; set; }
        public string Quantity { get; set; }
        public string LocationName { get; set; }
    }

}
