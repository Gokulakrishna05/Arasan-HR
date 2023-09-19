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
            this.Quolst = new List<SelectListItem>();
            this.Currlst = new List<SelectListItem>();
            this.Custypelst = new List<SelectListItem>();

        }
        public List<SelectListItem> RecList;
        public string Recieved { get; set; }


        public List<SelectListItem> Suplst;
        public List<SelectListItem> Enqlst;
        public List<SelectListItem> Typelst;
        public List<SelectListItem> Quolst;
        public List<SelectListItem> Currlst;
        public List<SelectListItem> Custypelst;
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
        public string QuoType { get; set; }
        public string EnNo { get; set; }
        public string EnDate { get; set; }

        public string Customer { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string narr { get; set; }
        public string Assign { get; set; }
        public string Mobile { get; set; }
        public string Pro { get; set; }
        public string Gmail { get; set; }
        public string PinCode { get; set; }
        public double gross { get; set; }
        public double net { get; set; }
        public string status { get; set; }
        public List<QuoItem> QuoLst { get; set; }
        public string QuoteFormatId { get; set; }
        public List<SelectListItem> QuoteFormatList { get; set; }
        public string EnquiryId { get; set; }
        public List<SelectListItem> EnquiryList { get; set; }
        public List<SelectListItem> Itemgrouplst { get; internal set; }
    }
    public class QuoItem
    {
        

        public List<SelectListItem> Itlst { get; set; }

        public List<SelectListItem> ItemGrouplst { get; set; }

        public string saveItemId { get; set; }
        public string ItemGroupId { get; set; }
        
        public string confac { get; set; }

        //public string Description { get; set; }
        public string rate { get; set; }
        public string amount { get; set; }
        public string disc { get; set; }
        public string discamount { get; set; }

        public string frigcharge { get; set; }
        public string cgstp { get; set; }
        public string sgstp { get; set; }
        public string igstp { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string igst { get; set; }
        public string totalamount { get; set; }
        public string Isvalid { get; set; }

        public string itemid { get; set; }
        public string des { get; set; }
        public string unit { get; set; }
        public string quantity { get; set; }
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
        public string ID { get; set; }
        public string ITEMID { get; set; }

        public string UNIT { get; set; }

        public double QTY { get; set; }

        public double RATE { get; set; }

        public string TOTAMT { get; set; }
        public string PARTYID { get; set; }
        public string PARTYNAME { get; set; }
        public string ADD1 { get; set; }
        public string ADD2 { get; set; }
        public string ADD3 { get; set; }
        public string STATE { get; set; }
        public double AMOUNT { get; set; }
        public string GSTNO { get; set; }
        public string SALESQUOID { get; set; }
        public string ADDRESS { get; set; }
        public string IGST { get; set; }
        public string SALES_ENQ_ID { get; set; }
        public string QUOTE_NO { get; set; }
        public string QUOTE_DATE { get; set; }
        public string DELIVERY_TERMS { get; set; }
        public string ITEMDESC { get; set; }
        public string GROSS { get; set; }
        public string NET { get; set; }
        public double CGSTAMT { get; set; }
        public double SGSTAMT { get; set; }
        public double IGSTAMT { get; set; }
        public string AMTINWORDS { get; set; }

    }
}
