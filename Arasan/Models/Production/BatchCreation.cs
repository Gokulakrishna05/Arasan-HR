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
            this.Prodlst = new List<SelectListItem>();

        }
        public List<SelectListItem> Prodlst;
        public string Prod { get; set; }

      

        public List<SelectListItem> Brlst;
        public string ID { get; set; }

        public string Branch { get; set; }
        public List<SelectListItem> Worklst;
        public string WorkCenter { get; set; }
        public string WorkCenterid { get; set; }
        public List<SelectListItem> Processlst;
        public string Process { get; set; }
        public string Processid { get; set; }
        public string DocDate { get; set; }
        public string Seq { get; set; }
        public List<SelectListItem> RecList;
        public string Enterd { get; set; }
        public double IOFrom { get; set; }
        public double IOTo { get; set; }
        public double MTO { get; set; }
        public string Leaf { get; set; }
        public string BatchNo { get; set; }
        public string Shall { get; set; }
        public string Narr { get; set; }
        public string RefBatch { get; set; }
        public string Enterdid { get; set; }
        public string ddlStatus { get; set; }
      
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
       
        public List<SelectListItem> IProcesslst { get; set; }

        public List<SelectListItem> Itemlst { get; set; }

        public string itemid { get; set; }
        
        public string unit { get; set; }
        public string qty { get; set; }
        public string saveitemid { get; set; }
        public string Isvalid { get; set; }
    }
    public class BatchOutItem
    {
        public string ID { get; set; }
        public string OProcess { get; set; }
        public string saveitemid { get; set; }
        public List<SelectListItem> OItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }
        public List<SelectListItem> OProcesslst { get; set; }

        public List<SelectListItem> OItemlst { get; set; }

        public string oitem { get; set; }

        public string ounit { get; set; }
        public string oqty { get; set; }
        public string Isvalid { get; set; }
        public string outtype { get; set; }

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

    public class BatchlstItem
    {
        public long id { get; set; }
        public string doc { get; set; }
        public string work { get; set; }
        public string process { get; set; }
        public string schno { get; set; }
        public string docDate { get; set; }

        public string editrow { get; set; }
        public string delrow { get; set; }
        public string view { get; set; }
        public string Accrow { get; set; }
        //public string Status { get; set; }
        //public string Account { get; set; }
    }
}
