using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IPermissions
    {
        DataTable GetPermissionsEdit(string id);
        string GetPermi(Permissions Em);
        DataTable GetAllPermissions(string strStatus);
        string StatusChange(string tag, string id);

    }
}
