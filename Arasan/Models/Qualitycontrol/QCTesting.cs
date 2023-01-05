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
        public string ID { get; set; }
        public string Type { get; set; }
        public List<SelectListItem> lst;
        public string POGRN { get; set; }
        public string GRN { get; set; }
        public string Party { get; set; }
        public string GRNNo { get; set; }
        public string DocId { get; set; }
        public string DocDate { get; set; }
        public string GRNDate { get; set; }
        public string SNo { get; set; }
        public string LotNo { get; set; }
        public string ClassCode { get; set; }
        public string TestResult { get; set; }
        public List<SelectListItem> assignList;
        public string TestedBy { get; set; }
        public string Remarks { get; set; }
        public List<QCItem> QCLst;
        public string TestDec { get; set; }
        public string Result { get; set; }
        public string TestValue { get; set; }
        public string ManualValue { get; set; }
        public string AccVale { get; set; }
        public string AcTestValue { get; set; }
        public string Isvalid { get; set; }

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

    }

}