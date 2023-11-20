using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models
{
    public class Sequence
    {
        public string ID { get; set; }
        public string Prefix { get; set; }
        public string Trans { get; set; }
        public string Last { get; set; }
        public string Des { get; set; }
        public string Start { get; set;}
        public string End { get; set; }
           

    } public class Sequencegrid
    {
        public string id { get; set; }
        public string prefix { get; set; }
        public string trans { get; set; }
        public string last { get; set; }
        public string des { get; set; }
        public string start { get; set;}
        public string end { get; set; }
        public string editrow { get; set; }
        public string delrow { get; set; }
           

    }
}
