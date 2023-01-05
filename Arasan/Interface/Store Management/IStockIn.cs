using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IStockIn
    {
        IEnumerable<StockIn> GetAllStock();
        DataTable GetStockInItem(string Itemid);

        DataTable GetIndentItem(string Itemid);
        string IssueToStockCRUD(StockIn Cy);

    }
}
