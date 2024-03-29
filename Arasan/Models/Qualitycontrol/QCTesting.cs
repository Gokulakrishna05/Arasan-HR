using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class QCTesting
    {
        public QCTesting()
        {
            this.Typlst = new List<SelectListItem>();
            this.lst = new List<SelectListItem>();
            this.Itemlst = new List<SelectListItem>();
            this.Supplst = new List<SelectListItem>();
            this.assignList = new List<SelectListItem>();
        }
        public List<SelectListItem> Typlst;
        public List<SelectListItem> Itemlst;
        public List<SelectListItem> Supplst;
        public string ItemId { get; set; }
        public string Item { get; set; }
        public string detid { get; set; }
        public string basicid { get; set; }
        public string temp { get; set; }
        public string ID { get; set; }
        public string Type { get; set; }
        public List<SelectListItem> lst;
        public string POGRN { get; set; }
        public string GRN { get; set; }
        public string Party { get; set; }
        public string ddlStatus { get; set; }
        public string GRNNo { get; set; }
        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string Branch { get; set; }
        public string GRNDate { get; set; }
        public string SNo { get; set; }
        public string LotNo { get; set; }
        public string Qty { get; set; }
        public string Procedure { get; set; }
        public string APID { get; set; }
        public string ClassCode { get; set; }
        public string TestResult { get; set; }
        public List<SelectListItem> assignList;
        public string TestBy { get; set; }

        public string Remarks { get; set; }
        public string Stat { get; set; }
        public string GRNProd { get; set; }
        public string Po { get; set; }
        public string PoDate { get; set; }
        public string Partyid { get; set; }
        public string Par { get; set; }
        public string PoId { get; set; }
       
        public List<QCItem> QCLst { get; set; }

        public List<QCPOItem> QCPOLst { get; set; }
        public List<QCGRNItem> QCGRNLst { get; set; }


    }
    public class QCPOItem
    {
       public string description { get; set; }
        public string value { get; set; }
        public string unit { get; set; }
        public string startvalue { get; set; }
        public string endvalue { get; set; }
        public string test { get; set; }
        public string manual { get; set; }
        public string actual { get; set; }
        public string apid { get; set; }
        public string testresult { get; set; }
        public string Isvalid { get; set; }
        public string testid { get; set; }



    }
    public class QCItem
    {
        public string TestDec { get; set; }
        public string Result { get; set; }
        public string TestValue { get; set; }
        public string ManualValue { get; set; }
        public string AccVale { get; set; }
        public string AcTestValue { get; set; }
        public string Isvalid { get; set; }
        public string apid { get; set; }

      

    }
    public class QCGRNItem
    {

        public string testid { get; set; }
        public string description { get; set; }
        public string unit { get; set; }
        public string value { get; set; }
        public string startvalue { get; set; }
        public string endvalue { get; set; }
        public string test { get; set; }
        public string manual { get; set; }
        public string actual { get; set; }
        public string testresult { get; set; }
        public string Isvalid { get; set; }
        public string apid { get; set; }
    }
    public class qcItem
    {
        public long id { get; set; }
        public string doc { get; set; }
        public string item { get; set; }
        public string party { get; set; }
        public string work { get; set; }
        public string process { get; set; }
        public string schno { get; set; }
        public string docDate { get; set; }
        public string test { get; set; }

        public string editrow { get; set; }
        public string delrow { get; set; }
        public string view { get; set; }
        public string Accrow { get; set; }
        //public string Status { get; set; }
        //public string Account { get; set; }
    }
}