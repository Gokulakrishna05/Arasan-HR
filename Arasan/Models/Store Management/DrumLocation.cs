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
    }
}
 