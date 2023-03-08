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
        public double IOFrom { get; set; }
        public double IOTo { get; set; }
        public double MTO { get; set; }
        public string Leaf { get; set; }
        public string Prod { get; set; }
        public string BatchNo { get; set; }
        public string Shall { get; set; }
        public string Narr { get; set; }
        public string RefBatch { get; set; }
      
        public List<BatchItem> BatchLst { get; set; }
        public List<BatchInItem> BatchInLst { get; set; }
        public List<BatchOutItem> BatchOutLst { get; set; }
        public List<BatchOtherItem> BatchOtherLst { get; set; }
        public List<BatchParemItem> BatchParemLst { get; set; }
    }
    public class BatchItem
    {
        public string ID { get; set; }
        public string ProcessId { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Processidlst { get; set; }
       

        public List<SelectListItem> WorkCenterlst { get; set; }

        public string WorkId { get; set; }
       
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
    public class BatchOutItem
    {
        public string ID { get; set; }
        public string OProcess { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> OItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        public List<SelectListItem> OProcesslst { get; set; }

        public List<SelectListItem> OItemlst { get; set; }

        public string OItem { get; set; }

        public string OUnit { get; set; }
        public string OQty { get; set; }
        public string Isvalid { get; set; }
        public string OutType { get; set; }

        public string Waste { get; set; }
        public string Vmper { get; set; }
        public string Greas { get; set; }
    }
    public class BatchOtherItem
    {
        public string ID { get; set; }
        public string OtProcessId { get; set; }
        public string saveItemId { get; set; }

        public List<SelectListItem> OProcessidlst { get; set; }

        public string Seqe { get; set; }
        public string Start { get; set; }
        public string Isvalid { get; set; }
        public string End { get; set; }

        public string StartT { get; set; }
        public string EndT { get; set; }
       
        public string Total { get; set; }

        public string RunHrs { get; set; }
        public string Break { get; set; }
        public string Remark { get; set; }
    }
    public class BatchParemItem
    {
        public string ID { get; set; }
        public string Param { get; set; }
    
        public string PUnit { get; set; }
        public string StartDate { get; set; }
        public string Isvalid { get; set; }
        public string EndDate { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }

        public string  Value { get; set; }

       
    }
}
