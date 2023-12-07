//using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class PaymentVoucher
    {
        public PaymentVoucher()
        {
            this.Brlst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Loclst = new List<SelectListItem>();
        }
        public string Currency { get; set; }

        public List<SelectListItem> Curlst;
        public List<SelectListItem> Brlst;
       
        public string ID { get; set; }
        public string Branch { get; set; }
        public string Supplier { get; set; }
        public string Grn { get; set; }

        public string RefNo { get; set; }

        public string Vdate { get; set; }
        public string VoucherNo { get; set; }
        public string ExRate { get; set; }
        public string RefDate { get; set; }

        public List<SelectListItem> Loclst;
        public string Location { get; set; }
        public string PType { get; set; }
        public double ReqAmount { get; set; }
        public double TotalDeAmount { get; set; }
        public double TotalCrAmount { get; set; }
        public string VType { get; set; }
       public string Ledgername { get; set; }
        public string LedgerId { get; set; }
        public double TotalAmount { get; set; }
        public List<VoucherItem> VoucherLst { get; set; }
        public double ClaimAmount { get; set; }
        public string Amtinwords { get; set; }
        public string Vmemo { get; set; }
        //public string Enqdate { get; set; }
        //public string EnqNo { get; set; }
        //public string ExRate { get; set; }
        //public string ParNo { get; set; }
    }
    public class VoucherItem
    {
        public string Credit { get; set; }
        public string Account { get; set; }
        public List<SelectListItem> Creditlst { get; set; }

        public List<SelectListItem> Acclst { get; set; }

        public string ID { get; set; }
        public double CreditAmount { get; set; }
        public double DepitAmount { get; set; }
        public string Isvalid { get; set; }

    }
}
