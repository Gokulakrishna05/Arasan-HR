using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IAllowanceMaster
    {
        string AllowanceMasterCRUD(AllowanceMaster Cy);
        DataTable GetAllAllowanceMasterGrid(string strStatus);
        DataTable GetEditAllowanceMaster(string id);
        string StatusChange(string tag, string id);
    }
}
