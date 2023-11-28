using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Models
{
    public class StockIn
    {
        public string ID { get; set; }
        public string Item { get; set; }
        public string Unit { get; set; }
        public string ItemID { get; set; }
        public string Qty { get; set; }
        public string QtyS { get; set; }
        public string Branch { get; set; }
        public string Location { get; set; }
        public string Locationname { get; set; }
        public string TotalQty { get; set; }
        public List<IndentList> Indentlist { get; set; }

    }
    public class IndentList
    {
        public string IndentNo { get; set; }
        public string IndentID { get; set; }
        public string IndentDate { get; set; }
        public string ItemName { get; set; }
        public string ItemId { get; set; }
        public string Quantity { get; set; }
        public string qty { get; set; }
        public string Unit { get; set; }
        public string LocationName { get; set; }
        public bool select { get; set; }
        public string StockQty { get; set; }
        public string LocationID { get; set; }
    }
    public class StockItems
    {
        public long id { get; set; }
        public string branch { get; set; }
        public string unit { get; set; }
        public string qty { get; set; }
        //public string docDate { get; set; }
        public string location { get; set; }
        //public string editrow { get; set; }
        //public string delrow { get; set; }
        //public string grn { get; set; }
        //public string pdf { get; set; }
        public string item { get; set; }
        public string acc { get; set; }
        //public string pono { get; set; }
        ////public string Account { get; set; }
    }
    //public class StockInItemList
    //{

    //}

}
