namespace Arasan.Models
{
    public class Company
    {
        public string ID { get; set; }
        public string CompanyId { get; set; }
        public string status { get; set; }

        public string CompanyName { get; set; }

        public string ddlStatus { get; set; }


    }
    public class CompanyList
    {
        public string compname { get; set; }
        public string id { get; set; }
        public string compdesc { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }


    }
}
