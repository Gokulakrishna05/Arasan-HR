using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
namespace Arasan.Models
{
    public class SalesReturn
    {
        public SalesReturn()
        {
            this.invoicelst = new List<SelectListItem>();
            this.vlst = new List<SelectListItem>();

        }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string Narr { get; set; }
        public string Vtype { get; set; }
        public string location { get; set; }
        public string returnfrom { get; set; }
        public string transitlocation { get; set; }
        public string custname { get; set; }
        public string invoiceid { get; set; }
        public string invoicedate { get; set; }
        public List<SelectListItem> invoicelst { get; set; }
        public List<SelectListItem> vlst { get; set; }
        public List<SalesReturnItem> returnlist { get; set; }
        public double gross { get; set; }
        public double net { get; set; }
    }
    public class SalesReturnItem
    {
        public string id { get; set; }
        public string itemid { get; set; }
        public string itemname { get; set; }
        public double frigcharge { get; set; }
        public string confac { get; set; }
        public string unit { get; set; }
        public string unitid { get; set; }
        public string quantity { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public double disc { get; set; }
        public double discamount { get; set; }
        public double cgstper { get; set; }
        public double cgstamt { get; set; }
        public double sgstper { get; set; }
        public double sgstamt { get; set; }
        public double igstper { get; set; }
        public double igstamt { get; set; }
        public double totalamount { get; set; }
        public string stkqty { get; set; }
        public string soldqty { get; set; }
        public double recdqty { get; set; }
        public string exicetype { get; set; }
        public string traiffid { get; set; }
        public string isvalid { get; set; }
    }
}
