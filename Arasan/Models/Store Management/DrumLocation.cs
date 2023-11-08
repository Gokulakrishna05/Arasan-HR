using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models 
{
    public class DrumLocation
    {
        public string Id { get; set; }
            public string Drum { get; set; }
        public string Item { get; set; }
        public string Location { get; set; }
       
    }
    public class Drumhistory
    {
        public string DrumNo { get; set; }
        public string Id { get; set; }
        public string Drum { get; set; }
        public string Item { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
        public List<DrumhistoryDet> dumlst { get; set; }
    }
    public class DrumhistoryDet
    {
        public string Drum { get; set; }
        public string Item { get; set; }
        public string Location { get; set; }
        public string Isvalid { get; set; }
        public string Type { get; set; }
    }
}
 