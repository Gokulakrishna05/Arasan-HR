using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class Ledger
    {
        public Ledger()
        {

            this.Typelst = new List<SelectListItem>();
            this.AccGrouplst = new List<SelectListItem>();

        }

        public string ID { get; set; }

        public List<SelectListItem> Typelst;
        public string AType { get; set; }
        public List<SelectListItem> AccGrouplst;
        public string AccGroup { get; set; }
        public string DisplayName { get; set; }


        public string LedName { get; set; }
        public string DocDate { get; set; }
        public string OpStock { get; set; }
        public string ClStock { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public string Groupcode { get; set; }
        public string Ledgercode { get; set; }
        public string LegCode { get; set; }
        public string ddlStatus { get; set; }
     


    }
    public class LedgerItems
    {
        public string id { get; set; }
        public string atype { get; set; }
        public string accgroup { get; set; }
        public string ledname { get; set; }
        public string displayname { get; set; }
        public string legcode { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}
