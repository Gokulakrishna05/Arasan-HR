using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
namespace Arasan.Models 
{
    public class DrumChange
    {
        public DrumChange()
        {
            this.Brlst = new List<SelectListItem>();

            this.Loclst = new List<SelectListItem>();
            this.shiftlst = new List<SelectListItem>();
            this.typelst = new List<SelectListItem>();
            this.itemlst = new List<SelectListItem>();
        }


        public List<SelectListItem> Brlst;

        public string ID { get; set; }
        public string Branch { get; set; }

        public string Docdate { get; set; }
        public string Docid { get; set; }
        public string Enterd { get; set; }
        public string ddlStatus { get; set; }


        public List<SelectListItem> Loclst;
        public List<SelectListItem> shiftlst;
        public List<SelectListItem> typelst;
        public List<SelectListItem> itemlst;
        public string EType { get; set; }
        public string Location { get; set; }
        public string shift { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string item { get; set; }
        public string totinpqty { get; set; }
        public string totoutqty { get; set; }
         public List<unpacking> unpackLst { get; set; }
        public List<reuseitem> reuseLst { get; set; }
        public List<packeditem> packLst { get; set; }
        public List<empitem> empLst { get; set; }

    }
    public class unpacking
    {
        public string ItemId { get; set; }
        public string Item { get; set; }
        public string APID { get; set; }
        public string inpid { get; set; }
        public string unit { get; set; }
        public string saveitemId { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        
      
        public string drumno { get; set; }
        public string comp { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public List<SelectListItem> batchlst { get; set; }
        public string batchno { get; set; }
        
        public double batchqty { get; set; }
        public double StockAvailable { get; set; }
        public double IssueQty { get; set; }
         

        public string Isvalid { get; set; }
       
        public double totalqty { get; set; }
        public double rate { get; set; }
        public double amount { get; set; }
    }
    public class reuseitem
    {
        public string ItemId { get; set; }
        public string APID { get; set; }
        public string unit { get; set; }
        public string outid { get; set; }
        public string saveitemId { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
       

        public string BinId { get; set; }
        public string outBin { get; set; }
        public string FromTime { get; set; }
        public string ToTime { get; set; }
        public string Bin { get; set; }
        public List<SelectListItem> binlst { get; set; }
        public string drumno { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public string batchno { get; set; }

        public string isoutinsert { get; set; }
        public double IssueQty { get; set; }

        public string rate { get; set; }
        public string amount { get; set; }
        public string Status { get; set; }
        public double OutputQty { get; set; }

        public double ExcessQty { get; set; }
        public double StockQty { get; set; }
        public string Isvalid { get; set; }
        public List<SelectListItem> outputlst;
        public string Purchasestock { get; set; }
        public string drumid { get; set; }
        public string Proinid { get; set; }
        public List<SelectListItem> lotlist { get; set; }
        public string Lotno { get; set; }
        public double totalqty { get; set; }
    }
    public class packeditem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string APID { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public string drumno { get; set; }
        public string batch { get; set; }
        public string comp { get; set; }
        public string tbatch { get; set; }
        public string batchqty { get; set; }
        public string recqty { get; set; }
        public string batchno { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
      
        public string Isvalid { get; set; }
       
   

    }
    public class empitem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
         public List<SelectListItem> employeelst { get; set; }
        public string emp { get; set; }
        public string empcode { get; set; }
        public string empcost { get; set; }
        public string rempcost { get; set; }
        public string department { get; set; }
       

        public string Isvalid { get; set; }



    }
}
