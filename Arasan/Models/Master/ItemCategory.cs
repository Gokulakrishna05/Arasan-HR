using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class ItemCategory
    {
        public string ID { get; set; }
      
        public string Category { get; set; }

        public String status { get; set; }
        public String ddlStatus { get; set; }
    }
    
    public class ItemCategoryGrid
    {
        public string id { get; set; }
      
        public string category { get; set; }

        public String status { get; set; }

        public String editrow { get; set; }
        public String delrow { get; set; }
    }
}
