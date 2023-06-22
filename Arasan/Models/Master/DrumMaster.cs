using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Models
{
    public class DrumMaster
    {

        public DrumMaster()
        {
            this.Categorylst = new List<SelectListItem>();
            this.Locationlst = new List<SelectListItem>();
            this.DrumTypelst = new List<SelectListItem>();
        }

        public String ID { get; set; }
        public String Drum { get; set; }
        public String DocDa { get; set; }
        public String Cate { get; set; }
        public String Loca { get; set; }
        public String Type { get; set; }
        public String Weight { get; set; }

        public string Status { get; set; }
        public string DrumNo { get; set; }
        public string DocDate { get; set; }


        public List<SelectListItem> Categorylst;
        public string Category { get; set; }

        public List<SelectListItem> Locationlst;
        public string Location { get; set; }

        public List<SelectListItem> DrumTypelst;
        public string DrumType { get; set; }

        public string TargetWeight { get; set; }
        public string status { get; set; }


    }
}
