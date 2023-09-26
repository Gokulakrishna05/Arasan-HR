using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class ItemDescription
    {
        public ItemDescription()
        {
            //this.Deslst = new List<SelectListItem>();
           
        }
        public string ID { get; set; }
        //public List<SelectListItem> Deslst;

        public string Value { get; set; }
        public string Unit { get; set; }
        public string Des { get; set; }
    }
}
