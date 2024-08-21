using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Models
{
    public class PayTransaction
    {
        public string Brc { get; set; }
        public string DocID { get; set; }
        public string PayPer { get; set; }
        public string PayCod { get; set; }
        public string PayCat { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string ID { get; set; }
        public string ddlStatus { get; set; }
        public List<SelectListItem> Brchlst { get; set; }
        public List<SelectListItem> PayPerlst { get; set; }
        public List<SelectListItem> PayCodlst { get; set; }
        public List<SelectListItem> PayCatlst { get; set; }
        public List<PayTra> BrLst { get; set; }

        public List<PayTraVList> ptrlst { get; set; }

        public bool selectall { get; set; }
    }

   
       
    
    public class PayTraVList
    {
        public string empid { get; set; }
        public string empname { get; set; }

        public string amo { get; set; }

        public string dtid { get; set; }
        public string Isvalid { get; set; }

        public bool drumselect { get; set; }
    }
    public class PayTra
    {
        public string eid { get; set; }
        public string ename { get; set; }
        public string amo { get; set; }
        public string Isvalid { get; set; }
    }
    public class ListPayTransaction
    {
        public string brc { get; set; }
        public string docid { get; set; }
        public string payper { get; set; }
        public string paycod { get; set; }
        public string paycat { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}