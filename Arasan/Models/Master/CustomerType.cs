namespace Arasan.Models
{
    public class CustomerType
    {
        public string ID { get; set; }
        public string Type { get; set; }
        public string Des { get; set; }
        public string status { get; set; }
        public string ddlStatus { get; set; }
    } 
    public class CustomerGrid
    {
        public string id { get; set; }
        public string type { get; set; }
        public string des { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
    }
}
