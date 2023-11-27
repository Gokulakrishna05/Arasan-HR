using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class DirectDeduction
    {
        public DirectDeduction()
        {
            this.Loc = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
        }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string Dcno { get; set; }
        public string Reason { get; set; }
        public string Gro { get; set; }
        public string  net { get; set; }
        public List<SelectListItem> assignList;
        public string Entered { get; set; }
        public string Narr { get; set; }
        public string NoDurms { get; set; }
        public string Material { get; set; }

        public string status { get; set; }


        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public string Location { get; set; }
        public string ddlStatus { get; set; }
        public List<DeductionItem> Itlst { get; set; }
    }
    public class ListDirectDeductionItem
    {
        public long id { get; set; }
        public string branch { get; set; }
        public string docNo { get; set; }
        public string loc { get; set; }
        public string docDate { get; set; }
        public string refno { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string entby { get; set; }
        public string view { get; set; }

    }
    public class DeductionItem
    {
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        //public string Itemname { get; set; }
        public string Isvalid { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
        public double BinID { get; set; }
        //public string Batch { get; set; }
        //public string serial { get; set; }
        //public string CurrentStock { get; set; }
        public double Quantity { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }
        public double TotalAmount { get; set; }
        //public string Number { get; set; }
        //public string SerialNumber { get; set; }
        public double Process { get; set; }
        //public string Bat { get; set; }





    }
}
