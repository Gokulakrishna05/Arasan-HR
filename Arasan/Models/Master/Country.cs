namespace Arasan.Models
{
    public class Country
    {
        public string ID{ get; set; }
        public string ConName { get; set; }
        public string ConCode { get; set; }
        public string status { get; set; }
        public string ddlStatus { get; set; }
        public string createby { get; set; }

    }
    public class Countrygrid
    {
        public string id{ get; set; }
        public string coname { get; set; }
        public string concode { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }

    }
}