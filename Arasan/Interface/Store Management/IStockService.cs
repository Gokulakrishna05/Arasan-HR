using Arasan.Models;
using System.Data;
namespace Arasan.Interface
{
    public interface IStockService
    {
        DataTable GetStockInHand();
        DataTable GetIndentDeatils();
        DataTable GetStockDeatils();

        DataTable GetBinid();
    }
}
