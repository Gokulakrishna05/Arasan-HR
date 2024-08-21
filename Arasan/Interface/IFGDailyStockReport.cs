
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IFGDailyStockReport
    {
        DataTable GetAllFGDailyStockReport(string dtFrom, string loc);
        DataTable GetLocation();


    }
}
