﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class QcDashboard
    {
        public List<QcNotify> qcNotifies { get; set; }
        public List<Notify> Notifies { get; set; }
        //public List<CuringGroup> curgroups { get; set; }
        //public double loaded { get; set; }
        //public double empty { get; set; }
        //public int res { get; set; }
        //public double rem { get; set; }
        public string dis { get; set; }

        //public string Quotefollowcount { get; set; }
        public List<MatNotify> Materialnotification { get; set; }

        //public List<GridDisplay> Folllst { get; set; }
        //public List<EnqDisplay> Enqlllst { get; set; }
        //public List<SalesQuoteDisplay> SalesQuotelllst { get; set; }
        //public int Quotefollowcunt { get; set; }
        //public int EnqFollowcunt { get; set; }
        //public int SalesQuoteFollowcunt { get; set; }
        public List<APOut> APOutlist { get; set; }
        public List<POut> POutlist { get; set; }
        public List<APOutItem> Aplast { get; set; }
        public List<GRNItem> Grnplst { get; set; }
    }
    public class QcNotify
    {
        public string GateDate { get; set; }
        public string PartyName { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string Unit { get; set; }
        public string id { get; set; }

    }
    public class MatNotify
    {
        public string Date { get; set; }
        public string LocationName { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string Unit { get; set; }
        public string stockQty { get; set; }
    }
    public class APOut
    {
        public string id { get; set; }
        public string apoutid { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string Drum { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string Time { get; set; }
        public string ApId { get; set; }
        public string dis { get; set; }
        public string Fin { get; set; }

    }
    public class POut
    {
        public string id { get; set; }
        public string poutid { get; set; }
        public string Drum { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string Time { get; set; }
        public string pId { get; set; }
        public string detid { get; set; }
        public string dis { get; set; }
        public string Fin { get; set; }

    }
    public class APOutItem
    {
        public string id { get; set; }
        public string Drum { get; set; }
        public string ItemName { get; set; }
        public string TotalQty { get; set; }
        public string Time { get; set; }
        

    }
    public class Notify
    {
        public string ID { get; set; }
        public string Doc { get; set; }
        public string Item { get; set; }
        public string Drum { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        
    }

    public class GRNItem
    {
        public string id { get; set; }
        public string detid { get; set; }
        public string Docid { get; set; }
        public string Docdate { get; set; }
        public string Party { get; set; }
        public string Cur { get; set; }


    }
    //public class EnqDisplay
    //{
    //    public string displaytext { get; set; }

    //    public string followedby { get; set; }

    //    public string Count { get; set; }

    //    public string status { get; set; }

    //    public string CPerson { get; set; }

    //}

    //public class SalesQuoteDisplay
    //{
    //    public string displaytext { get; set; }

    //    public string followby { get; set; }

    //    public string Count { get; set; }

    //    public string status { get; set; }

    //    public string CPerson { get; set; }

    //}
}
