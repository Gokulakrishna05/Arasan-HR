using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class DirectPurchase
    {
        public DirectPurchase()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public string DPId { get; set; }

        public string Branch { get; set; }

        public string Supplier { get; set; }

        public List<SelectListItem> Suplst;

        public string Currency { get; set; }

        public List<SelectListItem> Curlst;
        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public List<DirItem> DirLst { get; set; }
        public string Gross { get; set; }
        public string Voucher { get; set; }
        public string DocNo { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string DocDate { get; set; }
        public string net { get; set; }
        public string Frig { get; set; }
        public string Other { get; set; }
        public string Round { get; set; }
        public string SpDisc { get; set; }
        public string LRCha { get; set; }
        public string DelCh { get; set; }



    }
    public class DirItem
    {
        public string ItemId { get; set; }
      
        public List<SelectListItem> Itemlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string ItemGroupId { get; set; }

        public double ConFac { get; set; }
        public string Unit { get; set; }
        public string PurType { get; set; }
        public double Quantity { get; set; }
     //   public string unitprim { get; set; }
      //  public double QtyPrim { get; set; }
        public double rate { get; set; }
        public double Amount { get; set; }
        public double Disc { get; set; }
        public double DiscAmount { get; set; }
        public double CostRate { get; set; }
        public double Assessable { get; set; }
        public double TariffId { get; set; }
        public double CGSTP { get; set; }
        public double SGSTP { get; set; }
        public double IGSTP { get; set; }
        public double CGST { get; set; }
        public double SGST { get; set; }
        public double IGST { get; set; }
        public double TotalAmount { get; set; }
        public string Isvalid { get; set; }

    }
}
