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
            this.cuntylst = new List<SelectListItem>();
            this.statlst = new List<SelectListItem>();
        }
        public List<SelectListItem> Compalst;
        public string CompanyName { get; set; }

        public List<SelectListItem> cuntylst;
        public String countryid { get; set; }

        public List<SelectListItem> statlst;

        public String StateName { get; set; }


    }
}

