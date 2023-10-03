using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class ItemDescription
    {
        public ItemDescription()
        {
            this.Unitlst = new List<SelectListItem>();
           
        }
        public string ID { get; set; }
        public List<SelectListItem> Unitlst;

        public string Value { get; set; }
        public string Unit { get; set; }
        public string Des { get; set; }
    }
}
