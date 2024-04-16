using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class DirectPurchase
    {
        public DirectPurchase()
        {
            this.Brlst  = new  List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
            this.Vocherlst = new List<SelectListItem>();
            this.putypelst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> Vocherlst;
        public string ID { get; set; }

        public string Branch { get; set; }

        public string Supplier { get; set; }
        public string statetype { get; set; }
        public string Purtype { get; set; }
        public List<SelectListItem> Suplst;

        public List<SelectListItem> putypelst;
        public string Currency { get; set; }

        public List<SelectListItem> Curlst;
        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public List<DirItem> DirLst { get; set; }
        public double Gross { get; set; }
        public string Voucher { get; set; }
        public string DocNo { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string DocDate { get; set; }
        public double Net { get; set; }
        public double Frieghtcharge { get; set; }
        public double Other { get; set; }
        public double Round { get; set; }
        public double Disc { get; set; }
        public double LRCha { get; set; }
        public double DelCh { get; set; }
        public string status { get; set; }
        public string Narration { get; set; }
        public string ddlStatus { get; set; }
        public string IGST { get; set; }
        public string IGSTDISP { get; set; }
        public string SGST { get; set; }
        public string SGSTDISP { get; set; }
        public string CGST { get; set; }
        public string CGSTDISP { get; set; }
        public string Packingcharges { get; set;}
        public string truckno { get; set; }
        public string drivername { get; set; }  
        public string LRno { get; set; }
        public string LRdate { get; set; }
        public string dispatchname { get; set; }
    }
    public class DirItem
    {
        public string ID { get; set; }
        public string ItemId { get; set; }
        public string gst { get; set; }
        public string saveItemId { get; set; }
        public List<SelectListItem> Itemlst { get; set; }
        public List<SelectListItem> gstlst { get; set; }

        public List<SelectListItem>  ItemGrouplst { get; set; }
        public List<SelectListItem> Indentlst { get; set; }
        public string IndentId { get; set; }
        public string ItemGroupId { get; set; }
        public double FrigCharge { get; set; }
        public string ConFac { get; set; }
        public string Unit { get; set; }
        public List<SelectListItem> PURLst;

        public string PurType { get; set; }
        public double Quantity { get; set; }
     //   public string unitprim { get; set; }
      //  public double QtyPrim { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }

        public double Disc { get; set; }
        public double DiscAmount { get; set; }
    
        public double CGSTP { get; set; }
        public double Percentage { get; set; }
        public double SGSTP { get; set; }
        public double IGSTP { get; set; }
        public double CGST { get; set; }
        public double SGST { get; set; }
        public double IGST { get; set; }
        public double TotalAmount { get; set; }
        public string Isvalid { get; set; }

    }
    public class DirectPurchaseItems
    {
        public long id { get; set; }
        public string branch { get; set; }
        public string supplier { get; set; }
        public string docNo { get; set; }
        public string docDate { get; set; }
        public string mailrow { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string move { get; set; }
        public string Accrow { get; set; }
        public string gross { get; set; }
        public string net { get; set; }
        //public string Status { get; set; }
        //public string Account { get; set; }
    }
}
