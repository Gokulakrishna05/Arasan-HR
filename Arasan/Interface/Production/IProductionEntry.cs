using Arasan.Models;
using System.Data;
namespace Arasan.Interface
{
    public interface IProductionEntry
    {
        DataTable ShiftDeatils();
        DataTable DrumDeatils();
        DataTable Getstkqty(string branch, string loc, string ItemId);
        DataTable BindProcess();
    }
}
