using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class DirectDeduction
    {
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string Dcno { get; set; }
        public string Reason { get; set; }
        public string Gro { get; set; }
        public string Entered { get; set; }
        public string Narr { get; set; }
        public string NoDurms { get; set; }


        public DirectDeduction()
        {
            this.Loc = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
        }
        public string Branch { get; set; }

        public List<SelectListItem> Brlst;

        public List<SelectListItem> Loc;
        public string Location { get; set; }
        public List<DeductionItem> Itlst { get; set; }
    }
    public class DeductionItem
    {
        public string ItemId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        public string Isvalid { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
        public double BinID { get; set; }
        //public string Batch { get; set; }
        //public string serial { get; set; }
        //public string CurrentStock { get; set; }
        public double Qty { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        //public string Number { get; set; }
        //public string SerialNumber { get; set; }
        public double Process { get; set; }
        //public string Bat { get; set; }





    }
}
