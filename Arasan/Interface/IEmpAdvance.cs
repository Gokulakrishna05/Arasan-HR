using System.Data;
using System.Diagnostics.Contracts;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IEmpAdvance
    {

        DataTable GetEmpId();
        DataTable GetAdvId();
        DataTable GetEmpAdvanceEdit(string id);
        string EmpAdvanceCRUD(EmpAdvance ic);
        DataTable GetAllEmpAdvance(string strStatus);
        string StatusChange(string tag, string id);



    }
}
