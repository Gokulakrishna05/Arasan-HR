using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
namespace Arasan.Models
{
    public class BatchProduction
    {
        public BatchProduction()
        {
            this.Brlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.Shiftlst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
            this.ETypelst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;

        public string Branch { get; set; }

        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Shiftdate { get; set; }
        public string ProcessId { get; set; }
        public string batchcomplete { get; set; }
        public double ProdQty { get; set; }
        public double SchQty { get; set; }
        public string EntryType { get; set; }
        public string ProdLogId { get; set; }
        public string ProdSchNo { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string Shift { get; set; }
        public List<SelectListItem> Shiftlst;
        public List<SelectListItem> Processlst;
        public List<SelectListItem> ETypelst;
       
        public List<ProInCons> inconslst { get; set; }
        public List<output> outlst { get; set; }
        public List<wastage> wastelst { get; set; }
        public double totalinqty { get; set; }
        public double totaloutqty { get; set; }
        public double wastageqty { get; set; }
        public double totalconsqty { get; set; }
        public double Machine { get; set; }
        public double totaRmqty { get; set; }
        public double totalRmValue { get; set; }
        public double CosValue { get; set; }

        public string BatchNo { get; set; }
        public List<SelectListItem> RecList;
        public string Enterd { get; set; }
        public List<BProInCons> Binconslst { get; set; }
        public List<Boutput> Boutlst { get; set; }
        public List<Bwastage> Bwastelst { get; set; }
        public List<ProInputItem> inplst { get; set; }
    }
    public class ProInputItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
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
        //public string Output { get; set; }
        public string Isvalid { get; set; }
        //public List<SelectListItem> outputlst;

    }
    public class BProInCons
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public double ConsStock { get; set; }
        public string consunit { get; set; }
        public double consQty { get; set; }
        public string Isvalid { get; set; }
        public string BinId { get; set; }

    }
    public class Boutput
    {
        public string ID { get; set; }
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
    }
    public class Bwastage
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public string Isvalid { get; set; }
        public string batchno { get; set; }
        public double wastageQty { get; set; }
        public string toloc { get; set; }
        public List<SelectListItem> loclst { get; set; }
        public string BinId { get; set; }
    }
}
