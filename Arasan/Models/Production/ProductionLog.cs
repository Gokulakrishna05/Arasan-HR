using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models 
{
    public class ProductionLog
    {
        public ProductionLog()
        {
            this.Brlst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();
            this.Shiftlst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
            
            this.RecList = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;

        public string Branch { get; set; }
        public string Itemname { get; set; }
        public List<SelectListItem> Worklst;
        public string WorkId { get; set; }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string Docdate { get; set; }
        public string ProcessId { get; set; }
        public string Shift { get; set; }
        public List<SelectListItem> Shiftlst;
        public List<SelectListItem> Processlst;
        public List<SelectListItem> RecList;
        public string Supervised { get; set; }
        public double FuelQty { get; set; }
        public string ProdLog { get; set; }
        public string ProcessLot { get; set; }
        public double InpuyQty { get; set; }
        public double OutputQty { get; set; }
        public double ConsQty { get; set; }
        public double TotalPowder { get; set; }
        public double TotalDust { get; set; }
        public double TotalWaste { get; set; }
        public string Entered { get; set; }
        public string ComplYN { get; set; }
        public string MainValue { get; set; }
        public string ProdSieve { get; set; }
        public string EUnit { get; set; }
        public List<WorkCenter> WorkLst { get; set; }
    }
    public class WorkCenter
    {
        public string ID { get; set; }
        public List<SelectListItem> Statuslst { get; set; }
        public string Status { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Reasonlst { get; set; }
        public string Reason { get; set; }

        public List<SelectListItem> WorkCenterlst { get; set; }

        public string WorkId { get; set; }
        public List<SelectListItem> PTypelst { get; set; }
        public string PType { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string TotalHrs { get; set; }
        public string Isvalid { get; set; }
    }
}
