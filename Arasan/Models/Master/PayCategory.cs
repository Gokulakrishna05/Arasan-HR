using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Models
{
    public class PayCategory
    {
        public string DocID { get; set; }
        public string DocDate { get; set; }
        public string PayCat { get; set; }
        public string PayTim { get; set; }
        public string BasCat { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
        public string ID { get; set; }
        public List<SelectListItem> PayTyplst { get; set; }
        public List<SelectListItem> BasCatlst { get; set; }
        public string ddlStatus { get; set; }



        public List<PayCat> PcLst { get; set; }

    }

    public class PayCatdetailstable
    {
        public List<PayCatVList> pcalst { get; set; }

        public bool selectall { get; set; }
    }
    public class PayCatVList
    {
        public string pcode { get; set; }
        public string print { get; set; }
        public string pas { get; set; }
        public string aol { get; set; }
        public string pcoval { get; set; }
        public string form { get; set; }
        public string dtid { get; set; }
        public bool drumselect { get; set; }
    }
    public class PayCat
    {
        public string id { get; set; }
        public string pc { get; set; }
        public string pr { get; set; }
        public string prs { get; set; }
        public string aod { get; set; }

        public string pcv { get; set; }
        public string fo { get; set; }
        public string Isvalid { get; set; }
    }
    public class ListPayCategory
    {
        public string docid { get; set; }
        public string docdate { get; set; }
        public string paycat { get; set; }
        public string paytim { get; set; }
        public string bascat { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
    }