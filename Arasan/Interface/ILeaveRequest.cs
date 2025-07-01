using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface ILeaveRequest
    {
        DataTable GetAllLeaveRequestGrid(string strStatus);
        DataTable GetEditLeaveRequest(string id);
        DataTable GetEmployee();
        DataTable GetLeaveType();
        string LeaveRequestCRUD(LeaveRequest Cy);
        string StatusChange(string tag, string id);
        string ViewLeaveRequest(LeaveRequest Cy);
    }
}
