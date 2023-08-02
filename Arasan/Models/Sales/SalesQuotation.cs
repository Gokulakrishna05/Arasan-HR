using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;


namespace Arasan.Models
{
    public class SalesQuotation
    {
        public SalesQuotation()
        {
            this.Brlst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.Categorylst = new List<SelectListItem>();
            this.cuntylst = new List<SelectListItem>();
            this.Enqlst = new List<SelectListItem>();
            this.Typelst = new List<SelectListItem>();
            this.Prilst = new List<SelectListItem>();
        }
        public List<SelectListItem> RecList;
        public string Recieved { get; set; }
      

        public List<SelectListItem> Suplst;
        public List<SelectListItem> Enqlst;
        public List<SelectListItem> Typelst;
        public string CustomerType { get; set; }
        public string EnqType { get; set; }
        public List<SelectListItem> cuntylst;

        public string Country { get; set; }
        public List<SelectListItem> Prilst;

        public string Priority { get; set; }
        public List<SelectListItem> Brlst;

        public string Branch { get; set; }

        public List<SelectListItem> assignList;

        public string Emp { get; set; }

        public List<SelectListItem> Curlst;

        public string Currency { get; set; }

        public List<SelectListItem> Categorylst;
        public string Through { get; set; }

        public string Sent { get; set; }
        public string ID { get; set; }

        public string QuoId { get; set; }
        public string QuoDate { get; set; }
        public string EnNo { get; set; }
        public string EnDate { get; set; }

        public string Customer { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Assign { get; set; }
        public string Mobile { get; set; }
        public string Pro { get; set; }
        public string Gmail { get; set; }
        public string PinCode { get; set; }
        public double Net { get; set; }
        public string status { get; set; }
        public List<QuoItem> QuoLst { get; set; }
        public string QuoteFormatId { get; set; }
        public List<SelectListItem> QuoteFormatList { get; set; }   
        public string EnquiryId { get; set; }
        public List<SelectListItem> EnquiryList { get; set; }
    }
    public class QuoItem 
    {
        public string ItemId { get; set; }

        public List<SelectListItem> Itlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }
        
        public string saveItemId { get; set; }
        public string ItemGroupId { get; set; }
        public string Des { get; set; }
        public string Unit { get; set; }
        public string ConFac { get; set; }
        public double Quantity { get; set; }

        //public string Description { get; set; }
        public double Rate { get; set; }
        public double Amount { get; set; }
        public double Disc { get; set; }
        public double DiscAmount { get; set; }

        public double FrigCharge { get; set; }
        public double CGSTP { get; set; }
        public double SGSTP { get; set; }
        public double IGSTP { get; set; }
        public double CGST { get; set; }
        public double SGST { get; set; }
        public double IGST { get; set; }
        public double TotalAmount { get; set; }
        public string Isvalid { get; set; }
    }
    public class QuotationFollowup
    {
        public string FolID { get; set; }
        public string QuoId { get; set; }
        public string Customer { get; set; }
        public string Enqdate { get; set; }
        public string Followby { get; set; }
        public string Enquiryst { get; set; }
        public string Followdate { get; set; }
        public string Nfdate { get; set; }
        public string Rmarks { get; set; }
        public List<SelectListItem> EnqassignList;
        public string Quoteid { get; set; }
        public List<QuotationFollowupDetail> qflst { get; set; }

    }
    public class QuotationFollowupDetail
    {
        public string ID { get; set; }
        public string Followby { get; set; }
        public string Enquiryst { get; set; }
        public string Followdate { get; set; }
        public string Nfdate { get; set; }
        public string Rmarks { get; set; }
    }

    public class SQuoItemDetail
    {
        public string ITEMID { get; set; }

        public string UNIT { get; set; }

        public double QTY { get; set; }

        public double RATE { get; set; }

       

    }
}
