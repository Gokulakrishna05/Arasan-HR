using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IStockIn
    {
        IEnumerable<StockIn> GetAllStock();
    }
}
