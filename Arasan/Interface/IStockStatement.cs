
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IStockStatement
    {
        DataTable GetLocation();
        DataTable GetBranch();
        DataTable GetType();
        DataTable Getswis(string type);

        DataTable GetAllStockStatementReport(string type, string loc,string branch,string asdate);



    }
}
