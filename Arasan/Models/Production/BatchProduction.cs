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
        public List<ProIn> inputlst { get; set; }
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

        public double BatchNo { get; set; }
        public List<SelectListItem> RecList;
        public string Enterd { get; set; }
    }
}
