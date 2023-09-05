using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
namespace Arasan.Models
{
    public class ProductionEntry
    {
        public ProductionEntry()
        {
            this.Brlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.Shiftlst=new List<SelectListItem>();
            this.Processlst=new List<SelectListItem>();
            this.ETypelst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.ProdLoglst = new List<SelectListItem>();
            this.ProdSchlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> ProdLoglst;
        public List<SelectListItem> ProdSchlst;
        public string Branch { get; set; }
        public string Itemname { get; set; }
        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Shiftdate { get; set; }
        public string ProcessId { get; set; }
        public string Selection { get; set; }
        public string BatchNo { get; set; }
        public string batchcomplete { get; set; }
        public double ProdQty { get; set; }
        public double SchQty { get; set; }
        public string EntryType { get; set; }
        public string ProdLogId { get; set; }
        public string ProdSchNo { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string IsCuring { get; set; }
        public string Shift { get; set; }
        public List<SelectListItem> Shiftlst;
        public List<SelectListItem> Processlst;
        public List<SelectListItem> ETypelst;
        public List<ProIn> inputlst { get; set; }
        public List<ProInCons> inconslst { get; set; }
        public List<output> outlst { get; set; }
        public List<wastage> wastelst { get; set; }
        public double totalinqty { get; set; }
        public double totaloutqty { get; set; }
        public double wastageqty { get; set; }
        public double Machine { get; set; }
        public double totaRmqty { get; set; }
        public double totalRmValue { get; set; }
        public double CosValue { get; set; }
        public double totalconsqty { get; set; }
        public List<SelectListItem> RecList;
        public string Enterd { get; set; }
        public string Remarks { get; set; }
       public string PROID { get; set; }
        public string BranchId { get; set; }    
        public string WCID { get; set; }
        public string LOCID { get; set; }
        public string shiftid { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string SStart { get; set; }
        public string SEnd { get; set; }
        public string OutEntryType { get; set; }
    }
    public class ProIn
    {
        public string ItemId { get; set; }
        public string saveitemId { get; set; }
        
        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> ItemGrouplst { get; set; }
        public string ItemGroupId { get; set; }
        public string BinId { get; set; }
        public List<SelectListItem> binlst { get; set; }
        public string drumno { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public string batchno { get; set; }
        public double batchqty { get; set; }
        public double StockAvailable { get; set; }
        public double IssueQty { get; set; }
        public string MillLoadAdd { get; set; }
        public string Output { get; set; }
        public string Isvalid { get; set; }
        public List<SelectListItem> outputlst;
        public string Purchasestock { get; set; }
        public string drumid { get; set; }
        public string Proinid { get; set; }
        public List<SelectListItem> lotlist { get; set; }
        public string Lotno { get; set; }
        public double totalqty { get; set; }
    }
    public class ProInCons
    {
        public string ItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public double ConsStock { get; set; }
        public string consunit { get; set; }
        public double consQty { get; set; }
        public string Isvalid { get; set; }
        public string BinId { get; set; }
        public string Purchasestock { get; set; }
        public string saveitemId { get; set; }
        public string Proinconsid { get; set; }

    }
    public class output
    {
        public string ItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Isvalid { get; set; }
        public double OutStock { get; set; }
        public double ExcessQty { get; set; }
        public double OutQty { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string drumno { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public string batchno { get; set; }
        public string status { get; set; }
        public List<SelectListItem> statuslst { get; set; }
        public string toloc { get; set; }
        public List<SelectListItem> loclst { get; set; }
        public string Shed { get; set; }
        public List<SelectListItem> Shedlst { get; set; }
        public string saveitemId { get; set; }
        public string outid { get; set; }
        public string locid { get; set; }
        public string drumid { get; set; }
    }
    public class wastage
    {
        public string ItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Isvalid { get; set; }
        public string batchno { get; set; }
        public double wastageQty { get; set; }
        public string toloc { get; set; }
        public List<SelectListItem> loclst { get; set; }
        public string BinId { get; set; }
        public string saveitemId { get; set; }
        public string wasteid { get; set; }
        public string locid { get; set; }
    }
    public class InwardItemBindList
    {
        public string branch { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public long inwardid { get; set; }
        public string curoutward { get; set; }
    }
    public class InwardItemDetailBindList
    {
        public string itemid { get; set; }
        public string drumno { get; set; }
        public string batchno { get; set; }
        public long inwdetailid { get; set; }
        public string batchqty { get; set; }
        public string duedate { get; set; }
        public long inwardid { get; set; }
    }
    
}
