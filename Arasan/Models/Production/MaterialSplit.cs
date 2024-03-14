using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models 
{
    public class MaterialSplit
    {


        public MaterialSplit()
        {


            Loclst = new List<SelectListItem>();

        }
 
        public string ID { get; set; }
        public string Branch { get; set; }

        public string Docdate { get; set; }
        public string Docid { get; set; }
        public string Enterd { get; set; }
        public string ddlStatus { get; set; }


        public List<SelectListItem> Loclst;

        public string EType { get; set; }
        public string Location { get; set; }
        public string shift { get; set; }
        public string startdate { get; set; }
        public string enddate { get; set; }
        public string starttime { get; set; }
        public string endtime { get; set; }
        public string reason { get; set; }
        public string totinpqty { get; set; }
        public string totoutqty { get; set; }
        public List<splitdetail> splitLst { get; set; }


    }
    public class splitdetail
    {
        public string ItemId { get; set; }
        public string ConItemId { get; set; }
        public string Item { get; set; }
        public string APID { get; set; }
        public string inpid { get; set; }
        public string unit { get; set; }
        public string Conunit { get; set; }
        public string saveitemId { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> ConItemlst { get; set; }


        public string drumno { get; set; }
        public string comp { get; set; }
        public List<SelectListItem> drumlst { get; set; }
        public List<SelectListItem> batchlst { get; set; }
        public string batchno { get; set; }

        public double wight { get; set; }
        public double Stock{ get; set; }
        public double  Qty { get; set; }
        public double  ConQty { get; set; }

        public string Isvalid { get; set; }
        public string tstock { get; set; }
        public string stockdiff { get; set; }
        public string totstock { get; set; }

        public double totalqty { get; set; }
        public double rate { get; set; }
        public double conrate { get; set; }
        public double amount { get; set; }
        public double conamount { get; set; }
    }
}
