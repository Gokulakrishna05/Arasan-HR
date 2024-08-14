using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class JournalVoucherModel
    {
        public JournalVoucherModel()
        {
            this.LocLst = new List<SelectListItem>();
            this.VTypeLst = new List<SelectListItem>();
            this.CurLst = new List<SelectListItem>();


            this.SecIDLst = new List<SelectListItem>();
            this.PartyIDLst = new List<SelectListItem>();

        }
        public string ID { get; set; }
        public string VocNo { get; set; }
        public string VocDate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string Loc { get; set; }
        public List<SelectListItem> LocLst;
        public string VType { get; set; }
        public List<SelectListItem> VTypeLst;
        public string Cur { get; set; }
        public List<SelectListItem> CurLst;
        public string ExcRate { get; set; }

        public List<Journal> JournalVoucherlist { get; set; }
        public string ddlStatus { get; set; }



        public string Narr { get; set; }
        public string SSP { get; set; }
        public string Expensive { get; set; }
        public string GST { get; set; }
        public string ICredit { get; set; }
        public string AmtWd { get; set; }


        public string SecID { get; set; }
        public List<SelectListItem> SecIDLst;
        public string Per { get; set; }
        public string Amt { get; set; }
        public string TDS { get; set; }
        public string PartyID { get; set; }
        public List<SelectListItem> PartyIDLst;
        public string DueDate { get; set; }
        public string TDCAmt { get; set; }
        public string Service { get; set; }
        public string Cess { get; set; }
        public string SHECess { get; set; }
        public string Branch { get; set; }


    }

    public class Journal
    {
        public string DBCR { get; set; }
        public List<SelectListItem> DBCRlst { get; set; }
        public string AccName { get; set; }
        public List<SelectListItem> AccNamelst { get; set; }
        public string DebitAmt { get; set; }
        public string CreditAmt { get; set; }
        public string Balance { get; set; }
        public string Isvalid { get; set; }
    }
}
