using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IStockStatementLotwise
    {
        DataTable GetAllStatementLotwise(string dtFrom);

    }
}
