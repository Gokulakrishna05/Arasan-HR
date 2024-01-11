using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Models
{
    public class ReceiptAgtRetDC
    {
        public ReceiptAgtRetDC()
        {
            this.Partylst = new List<SelectListItem>();
            this.Stocklst = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
            this.typelst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.Enteredlst = new List<SelectListItem>();
            this.Dcnolst = new List<SelectListItem>();
            this.branchlst = new List<SelectListItem>();
        }

        public List<SelectListItem> Enteredlst;
        public List<SelectListItem> Partylst;
        public List<SelectListItem> Stocklst;
        public List<SelectListItem> Loclst;
        public List<SelectListItem> Brlst;
        public List<SelectListItem> typelst;
        public List<SelectListItem> Dcnolst;
        public List<SelectListItem> branchlst;
        public string Party { get; set; }


        public string Stock { get; set; }
        public string ID { get; set; }
        public string Branch { get; set; }

        public string Location { get; set; }
        public string Locationid { get; set; }
        public string Did { get; set; }
        public string DDate { get; set; }
        public string Dcno { get; set; }
        public string DcDate { get; set; }
        public string DcType { get; set; }


        public string Ref { get; set; }
        public string RefDate { get; set; }
        public string Entered { get; set; }
        public string Narration { get; set; }
        public string ddlStatus { get; set; }
        


        public List<ReceiptAgtRetDCItem> ReceiptLst { get; set; }

        public string sdate { get; set; }
        public string edate { get; set; }
        
        public string branch { get; set; }
    }

    public class ReceiptAgtRetDCItem
    {

        public List<SelectListItem> namelst { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> Binlst { get; set; }
        public string item { get; set; }
        public string id { get; set; }
        public string saveItemId { get; set; }
        public string itemname { get; set; }
        public string itemid { get; set; }
        public string unit { get; set; }
        public string detid { get; set; }
        public string bin { get; set; }
        public string batch { get; set; }
        public string serial { get; set; }
        public string Pend { get; set; }
        public string Recd { get; set; }
        public string qty { get; set; }
        public string rej { get; set; }
        public string Acc { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public string Isvalid { get; set; }
    }
    public class ReceiptAgtRetDCGrid
    {
        public string id { get; set; }
        public string did { get; set; }
        public string ddate { get; set; }

        public string dctype { get; set; }
        public string party { get; set; }
        public string viewrow { get; set; }
        public string approve { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    } 
    
    //public class ReceiptReport
    //{

    //    public ReceiptReport()
    //    {
    //        this.branchlst = new List<SelectListItem>();
    //    }

    //    public List<SelectListItem> branchlst;
    //    public string ID { get; set; }
    //    public string sdate { get; set; }
    //    public string edate { get; set; }

    //    public string branch { get; set; }
       
    //}

}
