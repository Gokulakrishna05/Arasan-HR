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

        public List<SelectListItem> Curlst;
        public string Party { get; set; }
        public string PartyName { get; set; }

        public List<SelectListItem> Suplst;

        public string WorkCenter { get; set; }

        public List<SelectListItem> Joblst;
        public List<ProFormaInvoiceDetail> ProFormalst { get; set; }
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

        public string amount { get; set; }

        public string itrodis { get; set; }

        public string cashdisc { get; set; }

        public string tradedis { get; set; }

        public string additionaldis { get; set; }

        public string dis { get; set; }

        public string frieght { get; set; }

        public string tariff { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string SGSTCGST { get; set; }
        public string IGST { get; set; }

        public string CGSTP { get; set; }
        public string SGSTP { get; set; }
        public string IGSTP { get; set; }

        public string totamount { get; set; }

        public string Isvalid { get; set; }
    }

}
