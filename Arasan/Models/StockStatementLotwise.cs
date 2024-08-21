using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Models
{
    public class StockStatementLotwise
    {
        public string dtFrom { get; set; }

    }
    public class StockStatementLotwiseItems
    {
        public string lid { get; set; }
        public string iid { get; set; }
        public string lno { get; set; }
        public string qty { get; set; }

    }
}
