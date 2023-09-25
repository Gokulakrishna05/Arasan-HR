using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models
{
    public class AccGroup
    {
        public AccGroup()
        {
            this.Brlst = new List<SelectListItem>();
           
          
        }
        public List<SelectListItem> Brlst;
        public string Branch { get; set; }  
        
       

        public string CPMName { get; set; }
        public string ID { get; set; }


        public string PmName { get; set; }
        public string DocId { get; set; }
        public string Unique { get; set; }

     
    }
  
}

