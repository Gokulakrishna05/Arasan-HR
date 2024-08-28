using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ItemsToBeReceivedModel
    { 
        public string ID { get; set; }
         
        public string dtFrom { get; set; }
    }
    public class ItemsToBeReceivedModelItems
    {

        public string docno { get; set; }
        public string docdate { get; set; }
        public string partyid { get; set; }
        public string locid { get; set; }
        public string itemid { get; set; }
        public string unit { get; set; }
        public string dqty { get; set; }
        public string rqty { get; set; }
        public string pendqty { get; set; }
        public string empname { get; set; }
        public string expretdt { get; set; }
        public string days { get; set; }



    }




}

