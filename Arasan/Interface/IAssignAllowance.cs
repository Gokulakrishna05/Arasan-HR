using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IAssignAllowance
    {
        string AssignAllowanceCRUD(AssignAllowance Cy);
        DataTable GetAllAssignAllowanceGrid(string strStatus);
        DataTable GetAllowanceName();
        DataTable GetAllowanceType(string alltypeid);
        DataTable GetEditAssignAllowance(string id);
        DataTable GetEmpName();
        string StatusChange(string tag, string id);
    }
}
