using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;

namespace Arasan.Models
{
    public class State
    {
        public State()
        {
            this.cuntylst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public string StateName { get; set; }
        public string StateCode { get; set; }

        public List<SelectListItem> cuntylst;
        public String countryid { get; set; }
        public string status { get; set; }
        public string ddlStatus { get; set; }

    }
    public class StateGrid
    {
        public string id { get; set; }
        public string statename { get; set; }
        public string statecode { get; set; }
        public String countryid { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}