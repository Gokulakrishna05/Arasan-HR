﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Models
{
    public class ORSAT
    {
        public ORSAT()
        {
            this.Brlst = new List<SelectListItem>();
            this.shiftlst = new List<SelectListItem>();
            this.worklst = new List<SelectListItem>();
            
        }

        public List<SelectListItem> Brlst;
        public string Branch { get; set; }

        public List<SelectListItem> shiftlst;
        public string shift { get; set; }

        public List<SelectListItem> worklst;
        public string work { get; set; }

        public string ID { get; set; }
        public string docdate { get; set; }
        public string docid { get; set; }
        
        public string entry { get; set; }
        public string time { get; set; }
        public string remarks { get; set; }
        public string active { get; set; }

        public List<ORSATdetails> ORSATlst { get; set; }
    }

    public class ORSATdetails
    {
        public string para { get; set; }
        public string value { get; set; }
        public string Isvalid { get; set; }

    }
}