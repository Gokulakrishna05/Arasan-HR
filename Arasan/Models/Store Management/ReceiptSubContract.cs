using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class ReceiptSubContract
    {
        public ReceiptSubContract()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.DClst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.Vocherlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> Vocherlst;
        public string ID { get; set; }

        public string Branch { get; set; }

        public string Supplier { get; set; }
        public string item { get; set; }
        public string itemid { get; set; }
        public string qty { get; set; }
        public string rate { get; set; }
        public string DQty { get; set; }
        public string statetype { get; set; }

        public List<SelectListItem> Suplst;
        public List<SelectListItem> DClst;

        public string DCNo { get; set; }

        public List<SelectListItem> Curlst;
        public List<SelectListItem> Loclst;
        public string Location { get; set; }
       
        public string City { get; set; }
        public string Chellan { get; set; }
        public string DocNo { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string DocDate { get; set; }
        public string Through { get; set; }
        public string Narr { get; set; }
        public string TotRecqty { get; set; }
        public string enterd { get; set; }
        public string qtyrec { get; set; }
        
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string ddlStatus { get; set; }
        public List<ReceiptDeliverItem> Delilst { get; set; }
        public List<ReceiptRecivItem> Reclst { get; set; }
        public List<DrumItem> drumlst { get; set; }


    }

    public class ReceiptRecivItem
    {
        public string id { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public string item { get; set; }
        public string itemid { get; set; }
        public string qty { get; set; }
        public string unit { get; set; }
        public string unitid { get; set; }

        
        public string rate { get; set; }
        public string amount { get; set; }
      

       
        public string Isvalid { get; set; }


    }
    public class ReceiptDeliverItem
    {
        public string id { get; set; }

        public List<SelectListItem> Itemlst { get; set; }
        public string item { get; set; }
        public string itemid { get; set; }
        
        public string unit { get; set; }
        public string unitid { get; set; }

        public string qty { get; set; }
        public string val { get; set; }
        public string dqty { get; set; }
        public string drumno { get; set; }
        public string damount { get; set; }
       
        public string rate { get; set; }
        public string drate { get; set; }
        public string amount { get; set; }
        public string supid { get; set; }
       
        public string Isvalid { get; set; }


    }
    public class DrumItem
    {
        public string ID { get; set; }


        public string item { get; set; }
        public string itemid { get; set; }
        public List<SelectListItem> drumlist { get; set; }
        public string drumno { get; set; }
        public string unitid { get; set; }

        public string qty { get; set; }

        public string rate { get; set; }
        public string amount { get; set; }

        public string Isvalid { get; set; }


    }

    public class ReceiptSubContractItem
    {
        public long id { get; set; }
        public string branch { get; set; }
        public string supplier { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string loc { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string move { get; set; }
        public string view { get; set; }
        //public string Status { get; set; }
        //public string Account { get; set; }
    }
}

