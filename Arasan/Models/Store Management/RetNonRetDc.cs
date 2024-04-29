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
            this.Enteredlst = new List<SelectListItem>();
        }

        public List<SelectListItem> Enteredlst;
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
        public string Locationid { get; set; }
        public string Did { get; set; }
        public string DDate { get; set; }
        public string ADDate { get; set; }

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
        public string address { get; set; }
        public string add { get; set; }
        public string city { get; set; }
        public string Entered { get; set; }
        public string user { get; set; }
        public string userid { get; set; }
        public string Part { get; set; }
        public string Narration { get; set; }
        public string ddlStatus { get; set; }

        public List<RetNonRetDcItem> RetLst { get; set; }

    }

    public class RetNonRetDcItem
    {
        public List<SelectListItem> Sublst { get; set; }

        public List<SelectListItem> Itemlst { get; set; }

        public string saveItemId { get; set; }
        public string detid { get; set; }

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
        public string viewrow { get; set; }

        public string approve { get; set; }

        public string generate { get; set; }

        public string editrow { get; set; }
        public string delrow { get; set; }
    }


    public class ReturnDetail
    {
        public string ID { get; set; }
        public string LOCID { get; set; }
        public string DELTYPE { get; set; }
        public string DOCDATE { get; set; }
        public string EMPNAME { get; set; }
        //public string EMPNAME { get; set; }
        public string ITEMID { get; set; }
        public string QTY { get; set; }
        public string UNIT { get; set; }
        public string PURFTRN { get; set; }
        public string FROMLOCID { get; set; }
        public string APPBY { get; set; }
        public string APPBY2 { get; set; }
        public string THROUGH { get; set; }
        public string DELDATE { get; set; }
        public string RDELBASICID { get; set; }
        public string PARTYID { get; set; }
        public string ADD1 { get; set; }
        public string CITY { get; set; }
        public string EBY { get; set; }
        public string DOCID { get; set; }

    }


    }