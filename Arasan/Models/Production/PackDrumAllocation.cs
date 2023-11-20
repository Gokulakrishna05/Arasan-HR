using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
namespace Arasan.Models 
{
    public class PackDrumAllocation
    {
        public PackDrumAllocation()
        {
            this.Brlst = new List<SelectListItem>();
            
            this.Loclst = new List<SelectListItem>();
            this.Emplst = new List<SelectListItem>();
        }
       
      
        public List<SelectListItem> Brlst;

        public string ID { get; set; }
        public string Branch { get; set; }
        
        public string Docdate { get; set; }
        public string Docid { get; set; }
       

        public List<SelectListItem> Loclst;
        public List<SelectListItem> Emplst;
        public string Location { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public string Pri { get; set; }
        public string Enter { get; set; }
        public string ddlStatus { get; set; }
        public string Totdrum { get; set; }
 
        public List<DrumCreate> drumlst { get; set; }
       
    }
    public class DrumCreate
    {
        public string packdrum { get; set; }
        public string totaldrum { get; set; }
        public string packyn { get; set; }
       
        public string Isvalid { get; set; }

    }
    public class PackList
    {
        public string id { get; set; }
        public string docid { get; set; }
        public string docdate { get; set; }
        public string start { get; set; }
        public string last { get; set; }
        public string prefix { get; set; }
        public string location { get; set; }
        public string totdrum { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }
    

    }
}

