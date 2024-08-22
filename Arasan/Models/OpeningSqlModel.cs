using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class OpeningSqlModel
    {



        public OpeningSqlModel()
        {
            this.Brlst = new List<SelectListItem>();
             
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;
         public string Branch { get; set; }
 

        public string dtFrom { get; set; }
    }
    public class OpeningSqlModelItems
    {
       
        public string debit { get; set; }
        public string credit { get; set; }
        


    }




}

