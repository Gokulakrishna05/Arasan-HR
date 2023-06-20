using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class Branch
    {
        public Branch()
        {
            this.Compalst = new List<SelectListItem>();
            //this.cuntylst = new List<SelectListItem>();
            this.statlst = new List<SelectListItem>();
            this.Citylst = new List<SelectListItem>();
        }

        public string ID { get; set; }
        public string BranchName { get; set; }
        public string PinCode { get; set; }
        public List<SelectListItem> Citylst;
        public string City { get; set; }
        public string GSTDate { get; set; }
        public string Address { get; set; }
        public string GSTNo { get; set; }

        public List<SelectListItem> Compalst;
        public string CompanyName { get; set; }

        //public List<SelectListItem> cuntylst;
        //public String countryid { get; set; }

        public List<SelectListItem> statlst;

        public String StateName { get; set; }
        public String status { get; set; }


    }

    internal class RegularExpressionAttribute : Attribute
    {
    }
}

