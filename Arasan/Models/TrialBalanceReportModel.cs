using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class TrialBalanceReportModel
    {
      
      

            public TrialBalanceReportModel()
            {
                this.Brlst = new List<SelectListItem>();
                 this.Masterlst = new List<SelectListItem>();
                //this.ItemGrouplst = new List<SelectListItem>();
                //this.Itemlst = new List<SelectListItem>();
            }
            public string ID { get; set; }

            public List<SelectListItem> Brlst;
             public List<SelectListItem> Masterlst;
            public string Branch { get; set; }
            public string Master { get; set; }


            public string dtFrom { get; set; }
    }
        public class TrialBalanceReportModelItems
        {
            public string groupname { get; set; }
            public string mname { get; set; }
            public string debit { get; set; }
            public string credit { get; set; }
            public string masterid { get; set; }
            public string mstatus { get; set; }
            public string malie { get; set; }
           

        }

  


}

