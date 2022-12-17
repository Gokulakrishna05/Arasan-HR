using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class SalesEnquiry
    {
        public SalesEnquiry()
    {
        this.Brlst = new List<SelectListItem>();
        //this.Suplst = new List<SelectListItem>();
        this.Curlst = new List<SelectListItem>();
        //this.Loclst = new List<SelectListItem>();
    }
    public List<SelectListItem> Brlst;
    public string ID { get; set; }

    public string Branch { get; set; }

   // public string Supplier { get; set; }

    public List<SelectListItem> Suplst;

    public string Currency { get; set; }

    public List<SelectListItem> Curlst;
   // public List<SelectListItem> Loclst;
    public string Location { get; set; }
    public string ReqDate { get; set; }
    public string RetDate { get; set; }
    public double Packingcharges { get; set; }
    public double Frieghtcharge { get; set; }
    public double Othercharges { get; set; }
    public double Round { get; set; }
    public double otherdeduction { get; set; }
    public double Roundminus { get; set; }
    public double Gross { get; set; }
    public double Net { get; set; }
}
}
   