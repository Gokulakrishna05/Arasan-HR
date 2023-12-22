using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class CreditorDebitNote
    {

        public CreditorDebitNote()
        {
            this.VTypelst = new List<SelectListItem>();
            this.Grouplst = new List<SelectListItem>();
            this.Ledgerlst = new List<SelectListItem>();
        }

        public string ID { get; set; }
        public string TDate { get; set; }

        public List<SelectListItem> VTypelst;

        public string VType { get; set; }
        
        public List<SelectListItem> Grouplst;
       
        public string createdby { get; set; }
        public string branchid { get; set; }
        public string Group { get; set; }

        public List<SelectListItem> Ledgerlst;
        public string Ledger { get; set; }
        public string amount { get; set; }
        public string Voucher { get; set; }
        public string number { get; set; }
        public string Total { get; set; }
        public string Ref { get; set; }
        public string Refdate { get; set; }
        public string mid { get; set; }
        public string Amtinwords { get; set; }

        public List<CreDebNoteItems> NoteLst { get; set; }
    }

    public class CreDebNoteItems
    {
        public string ID { get; set; }

        public List<SelectListItem> Grplst { get; set; }
        public string Grp { get; set; }
        public string saveGrp { get; set; }
        public List<SelectListItem> Ledlst { get; set; }
        public string Led { get; set; }
        public string Tamount { get; set; }
        public string Isvalid { get; set; }
    }

    }
