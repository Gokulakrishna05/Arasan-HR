using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Models
{
    public class HolidayMaster
    {
        public string Holidayid { get; set; }

        public string ID { get; set; }
        public string Hname { get; set; }
        public string Hdate { get; set; }
        public string DWeek { get; set; }
        public string HType { get; set; }
        public string Rmk { get; set; }
        public string Cdate { get; set; }
        public string Cby { get; set; }
        public string Mdate { get; set; }
        public string Mby { get; set; }

        public string ddlStatus { get; set; }

    }

    public class HolidayMasterList
    {
        public string id { get; set; }
        public string hname { get; set; }
        public string hdate { get; set; }
        public string dweek { get; set; }


        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

        //public string rrow { get; set; }

    }

}
