using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class BatchCreation
    {
        public BatchCreation()
        {
            this.Brlst = new List<SelectListItem>();
            this.Worklst = new List<SelectListItem>();
            this.Processlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
           
        }
        public List<SelectListItem> Brlst;
        public string ID { get; set; }

        public string Branch { get; set; }
        public List<SelectListItem> Worklst;
        public string WorkCenter { get; set; }
        public List<SelectListItem> Processlst;
        public string Process { get; set; }
        public string DocDate { get; set; }
        public string Seq { get; set; }
        public List<SelectListItem> RecList;
        public string Enterd { get; set; }
        public List<BatchItem> BatchLst { get; set; }
        public List<BatchInItem> BatchInLst { get; set; }
    }
    public class BatchItem
    {
        public string ID { get; set; }
        public string ProcessId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Processidlst { get; set; }
       

        public List<SelectListItem> WorkCenterlst { get; set; }

        public string WorkId { get; set; }
        public string Process { get; set; }
        public string Seq { get; set; }
        public string Req { get; set; }
        public string Isvalid { get; set; }
    }
    public class BatchInItem
    {
        public string ID { get; set; }
        public string Process { get; set; }
        public string saveItemId { get; set; }
       
        public List<SelectListItem> IProcesslst { get; set; }

        public List<SelectListItem> Itemlst { get; set; }

        public string Item { get; set; }
        
        public string Unit { get; set; }
        public string Qty { get; set; }
        public string Isvalid { get; set; }
    }
}
