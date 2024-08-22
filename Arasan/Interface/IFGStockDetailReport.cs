using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IFGStockDetailReport
    {
        DataTable GetAllFGStockDetailReport(string dtFrom, string loc);
        DataTable GetLocation();


    }
}
