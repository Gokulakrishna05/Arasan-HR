using Arasan.Models;
using System.Data;

namespace Arasan.Interface 
{
    public interface IPaycodeService
    {
        string PaycodeCRUD(Paycode by);
        DataTable GetAlLPaycode(string strStatus);
        DataTable GetEditPayCodes(string id);
        DataTable GetPaycode(string id);
        string RemoveChange(string tag, string id);
        string StatusChange(string tag, string id);

    }
}
