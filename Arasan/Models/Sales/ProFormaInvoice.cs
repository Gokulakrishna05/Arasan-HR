using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Models
{
    public class ProFormaInvoice
    {
        public ProFormaInvoice()
        {
            this.Brlst = new List<SelectListItem>();
            //this.Curlst = new List<SelectListItem>();
            //this.Suplst = new List<SelectListItem>();
            this.Joblst = new List<SelectListItem>();

        }
        public string ID { get; set; }
        public string DocId { get; set; }
        public string status { get; set; }
        public string Docdate { get; set; }
        public string RefNo { get; set; }
        public string RefDate { get; set; }
        public string ExRate { get; set; }
        public string SalesValue { get; set; }
        public string Gross { get; set; }
        public string Net { get; set; }
        public string Amount { get; set; }
        public string BankName { get; set; }
        public string AcNo { get; set; }
        public string Address { get; set; }
        public string Narration { get; set; }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }
        public string Currency { get; set; }
        public double Round { get; set; }

        public double Discount { get; set; }
        public double cgst { get; set; }
        public double sgst { get; set; }
        public double igst { get; set; }
        public double FrightCharge { get; set; }
        public List<SelectListItem> Curlst;
        public string Party { get; set; }
        public string PartyName { get; set; }

        public List<SelectListItem> Suplst;

        public string WorkCenter { get; set; }
        public string ddlStatus { get; set; }

        public List<SelectListItem> Joblst;
        public List<ProFormaInvoiceDetail> ProFormalst { get; set; }
        public List<PAreaItem> AreaItemlst { get; set; }
        public List<PTermsItem> TermsItemlst { get; set; }
    }
    public class ProFormaInvoiceDetail
    {
        public string ID { get; set; }

        public string itemid { get; set; }
        public string item { get; set; }
        public string itemdes { get; set; }

        public string unit { get; set; }


        public string qty { get; set; }

        public string BaID { get; set; }
        public string rate { get; set; }

        public string discount { get; set; }

        public double amount { get; set; }

        public string itrodis { get; set; }

        public string cashdisc { get; set; }

        public string tradedis { get; set; }

        public string additionaldis { get; set; }

        public string dis { get; set; }

        public string frieght { get; set; }

        public string tariff { get; set; }
        public string cgst { get; set; }
        public string sgst { get; set; }
        public string SGSTCGST { get; set; }
        public string igst { get; set; }

        public string cgstp { get; set; }
        public string sgstp { get; set; }
        public string igstp { get; set; }

        public double totamount { get; set; }

        public string Isvalid { get; set; }
    }
    public class ViewDrumdetailstable
    {
        public List<DDrumdetailsView> Drumlst { get; set; }

        public bool selectall { get; set; }
    }
    public class DDrumdetailsView
    {
        public string lotno { get; set; }
        public string drumno { get; set; }
        public string qty { get; set; }
        public string rate { get; set; }
        public string invid { get; set; }
        public bool drumselect { get; set; }
    }
    public class PAreaItem
    {
        public string Isvalid { get; set; }

        public string ID { get; set; }

        public List<SelectListItem> Arealst { get; set; }

        public string Areaid { get; set; }
        public string Address { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string PinCode { get; set; }
        public string Fax { get; set; }
        public string Receiver { get; set; }
        public string Phone { get; set; }
        public string Cst { get; set; }

        public string Tin { get; set; }
        public string Add1 { get; set; }
        public string Add2 { get; set; }
        public string Add3 { get; set; }
        public string Email { get; set; }

    }
    public class PTermsItem
    {
        public string Isvalid { get; set; }
        public string ID { get; set; }

        public List<SelectListItem> Termslst { get; set; }

        public string Terms { get; set; }

    }
    public class ListProFormaInvoiceItems
    {
        public long id { get; set; }
        //public string branch { get; set; }
        public string enqno { get; set; }
        public string date { get; set; }
        public string party { get; set; }
        public string delrow { get; set; }
        public string refno { get; set; }
        public string edit { get; set; }

    }
}
