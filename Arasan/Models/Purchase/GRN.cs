//using DocumentFormat.OpenXml.Office.CoverPageProps;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class GRN
    {
        public GRN()
        {
            this.Brlst = new List<SelectListItem>();
            this.Suplst = new List<SelectListItem>();
            this.Curlst = new List<SelectListItem>();
            this.RecList = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
            this.desplst = new List<SelectListItem>();
            this.Paymenttermslst = new List<SelectListItem>();
            this.deltermlst = new List<SelectListItem>();
            this.warrantytermslst = new List<SelectListItem>();
            this.Accconfiglst = new List<SelectListItem>();
        }
        public List<SelectListItem> Brlst;
        public List<SelectListItem> desplst;
        public List<SelectListItem> Paymenttermslst;
        public List<SelectListItem> deltermlst;
        public List<SelectListItem> warrantytermslst;
        public string desp { get; set; }
        public string Paymentterms { get; set; }
        public string delterms { get; set; }
        public string warrantyterms { get; set; }
        public string ID { get; set; }
        public string Branch { get; set; }

        public string GRNNo { get; set; }
        public string BranchID { get; set; }
        public string GRNdate { get; set; }
        public string PONo { get; set; }
        public string POdt { get; set; }
        public string Qcstatus { get; set; }
        public string ExRate { get; set; }
        public string ParNo { get; set; }
        public string QuoteDate { get; set; }
        public string Recid { get; set; }

        public string assignid { get; set; }

        public string truckno { get; set; }
        public string LRno { get; set; }
        public string LRdate { get; set; }
        public string drivername { get; set; }
        public string dispatchname { get; set; }

        public string Supplier { get; set; }
        public string party { get; set; }
        public string Status { get; set; }
        public string Active { get; set; }

        public List<SelectListItem> Suplst;

        public string Cur { get; set; }

        public List<SelectListItem> Curlst;
        public List<SelectListItem> Accconfiglst;
        public string ADCOMPHID { get; set; }
        public List<POItemlst> PoItemlst { get; set; }

        public List<POItem> PoItem { get; set; }
        public List<GRNAccount> Acclst { get; set; }

        public List<SelectListItem> RecList;

        public List<SelectListItem> assignList;

        public double Gross { get; set; }
        public double Net { get; set; }
        public double TotalCRAmt { get; set; }
        public double TotalDRAmt { get; set; }
        public double Frieghtcharge { get; set; }
        public double Packingcharges { get; set; }
        public double Othercharges { get; set; }
        public double Round { get; set; }
        public string Narration { get; set; }
        public string Fax { get; set; }
        public string Amtinwords { get; set; }
        public string PhoneNo { get; set; }
        public string DespatchAddr { get; set; }
        public double Roundminus { get; set; }
        public double otherdeduction { get; set; }
        public string GRNID { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string mid { get; set; }
        public double DiscAmt { get; set; }
        public double Disc { get; set; }
        public double CGST { get; set; }
        public double SGST { get; set; }
        public double IGST { get; set; }

        public string CGSTDISP { get; set; }
        public string SGSTDISP { get; set; }
        public string IGSTDISP { get;set; }
        public double address { get; set; }
        public double ddlStatus { get; set; }
        public string createdby { get; set; }
        public string Vmemo { get; set; }

    }
    public class GRNItems
    {
        public long id { get; set; }
        public string branch { get; set; }
        public string supplier { get; set; }
        public string enqno { get; set; }
        public string docDate { get; set; }
        public string qcresult { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string grn { get; set; }
        public string pdf { get; set; }
        public string view { get; set; }
        public string acc { get; set; }
        public string pono { get; set; }
        //public string Account { get; set; }
    }
    //public class GRNitems
    //{
    //}

    public class GRNAccount
    {
        public string Ledgername { get; set; }
        public List<SelectListItem> Ledgerlist { get; set; }
        public string TypeName { get; set; }
        public string CRDR { get; set; }
        public List<SelectListItem> CRDRLst { get; set; }
        public double CRAmount { get; set; }
        public double DRAmount { get; set; }
        public string Isvalid { get; set; }
        public string symbol { get; set; }

    }
}
