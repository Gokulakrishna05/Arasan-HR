using Microsoft.AspNetCore.Mvc.Rendering;



namespace Arasan.Models
{
    public class AdvanceTM
    {

        public string ID { get; set; }
        public string AType { get; set; }
        public string MALmt { get; set; }
        public string ERules { get; set; }
        public string RPType { get; set; }
        public string NOIns { get; set; }
        public string Dedn { get; set; }
        public string Rmarks { get; set; }

        public string ddlStatus { get; set; }


    }

    public class AdvanceTMList
    {
        public string id { get; set; }
        public string atype { get; set; }
        public string maxlmt { get; set; }
        public string egrules { get; set; }
        public string rtype { get; set; }


        public string editrow { get; set; }
        public string viewrow { get; set; }
        public string delrow { get; set; }

        //public string rrow { get; set; }

    }
}
