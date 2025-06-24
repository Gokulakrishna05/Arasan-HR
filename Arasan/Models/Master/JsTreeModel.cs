using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class JsTreeModel
    {

    }
    public class TreeViewNode
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
    }
    public class Accounttree
    {
        public string accname { get; set; }
        public string group { get; set; }
        public string cate { get; set; }
       public List<SelectListItem> catelst { get; set; }
        public string parent { get; set; }
        public List<SelectListItem> itemlst { get; set; }
        public string id { get; set; }
    }
}
