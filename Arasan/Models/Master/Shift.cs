using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class Shift
    {
       
            public string ftime { get; set; }
            public string ttime { get; set; }
 

            public string shifthrs { get; set; }
             public string othrs { get; set; }
            public string shiftn { get; set; }
            public string ID { get; set; }
            public string createby { get; set; }
            public string ddlStatus { get; set; }
            

        
    }
    public class shiftgrid
    {
        public string id { get; set; }

        public string shift { get; set; }
        public string fromtime { get; set; }
        public string shiftn { get; set; }
        public string totime { get; set; }
        public string shifthrs { get; set; }
        public string othrs { get; set; }
 


        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

    }
}
