using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Models
{
    public class RetNonRetDc
    {

        public RetNonRetDc()
        {
            this.Partylst = new List<SelectListItem>();
            this.Stocklst = new List<SelectListItem>();
            this.Brlst = new List<SelectListItem>();
            this.typelst = new List<SelectListItem>();
            this.applst = new List<SelectListItem>();
            this.apprlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
        }

        public List<SelectListItem> Partylst;
        public List<SelectListItem> Stocklst;
        public List<SelectListItem> applst;
        public List<SelectListItem> apprlst;
        public List<SelectListItem> Loclst;
        public string Party { get; set; }


        public string Stock { get; set; }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;


        public string Branch { get; set; }

        public string Location { get; set; }
        public string Did { get; set; }
        public string DDate { get; set; }

        public List<SelectListItem> typelst;
        public string DcType { get; set; }
        public string Through { get; set; }
        public string DcNo { get; set; }
        public string DcDate { get; set; }


        public string Ref { get; set; }
        public string RefDate { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string City { get; set; }
        public string Delivery { get; set; }
        public string Approved { get; set; }
        public string Approval2 { get; set; }
        public string Entered { get; set; }
        public string Narration { get; set; }
        public string ddlStatus { get; set; }

        public List<RetNonRetDcItem> RetLst { get; set; }

    }

    public class RetNonRetDcItem
    {
        public List<SelectListItem> Sublst { get; set; }

        public List<SelectListItem> Itemlst { get; set; }

        public string saveItemId { get; set; }

        public string subgrp { get; set; }
        public string item { get; set; }
        public string Unit { get; set; }
        public string Current { get; set; }
        public string Qty { get; set; }
        public string Transaction { get; set; }
        public string PurRate { get; set; }
        public string Rate { get; set; }
        public string Amount { get; set; }
        public string Isvalid { get; set; }
    }

    public class RetNonRetDcGrid
    {
        public string id { get; set; }
        public string did { get; set; }
        public string ddate { get; set; }

        public string dctype { get; set; }
        public string party { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}