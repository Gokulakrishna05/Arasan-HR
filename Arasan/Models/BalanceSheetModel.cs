using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class BalanceSheetModel
    {



        public BalanceSheetModel()
        {
            this.Brlst = new List<SelectListItem>();
            
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
         public string Branch { get; set; }
         public string Branchs { get; set; }
 

        public string dtFrom { get; set; }
    }
    public class BalanceSheetItems
    {
        public string groupname { get; set; }
        public string db { get; set; }
        public string cr { get; set; }
        //public string malie { get; set; }
        //public string mid { get; set; }
        


    }




}

