using Microsoft.AspNetCore.Mvc.Rendering;
namespace Arasan.Models 
{
    public class Ledger
    {

        public string ID { get; set; }

        public string MName { get; set; }
        public string DispName { get; set; }


        public string GrpAccount { get; set; }
        public string Category { get; set; }
        public string Date { get; set; }
    }
}
