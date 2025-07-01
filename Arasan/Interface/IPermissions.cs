using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IPermissions
    {
        DataTable GetPermissionsEdit(string id);
        DataTable GetAllPermissions(string strStatus);
        string StatusChange(string tag, string id);
        string PermissionCRUD(Permissions Cy);
        string ViewPermission(Permissions Cy);
        DataTable GetEmpName();
    }
}
