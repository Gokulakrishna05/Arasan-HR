using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class PurMonReport
    {
        public PurMonReport()
        {
            this.Brlst = new List<SelectListItem>();
        }
        public string ID { get; set; }

        public List<SelectListItem> Brlst;

        public string Branch { get; set; }
        public string Sdate { get; set; }
        public string Edate { get; set; }
        public string dtFrom { get; set; }
        public string dtTo { get; set; }

    }

    public class PurMonReportItem
    {
        public long id { get; set; }
        public string part { get; set; }
        public string item { get; set; }
        public string unit { get; set; }
        public string jan { get; set; }
        public string feb { get; set; }
        public string mar { get; set; }
        public string april { get; set; }
        public string may { get; set; }
        public string june { get; set; }
        public string july { get; set; }
        public string aug { get; set; }
        public string sep { get; set; }
        public string act { get; set; }
        public string nov { get; set; }
        public string dec { get; set; }
    }

}
