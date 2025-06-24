using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface ILeaveTypeMaster
    {
        DataTable GetLeaveTypeMasterEdit(string id);
        DataTable GetAllLeaveTypeMaster(string strStatus);

        string GetInsLTM(LeaveTypeMaster Em);

        string StatusChange(string tag, string id);
    }
}
