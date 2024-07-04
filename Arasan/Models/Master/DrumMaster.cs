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
        public string DeLocation { get; set; }

        public List<SelectListItem> DrumTypelst;
        public string DrumType { get; set; }
        public string capacity { get; set; }

        public string TargetWeight { get; set; }
        public string status { get; set; }
        public string ddlStatus { get; set; }
        public string createby { get; set; }
        public List<drumloc> Loclst { get; set; }


    }

    public class DrumMastergrid
    {
        public string id { get; set; }
        public string drumnno { get; set; }
        public string docdate { get; set; }
        public string category { get; set; }

        public string location { get; set; }

        public string drumtype { get; set; }

        public string targetweight { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
    public class drumloc
    {
        public string id { get; set; }
        public List<SelectListItem> locationlst { get; set; }

        public string location { get; set; }

      
        public string Isvalid { get; set; }
    }

}
