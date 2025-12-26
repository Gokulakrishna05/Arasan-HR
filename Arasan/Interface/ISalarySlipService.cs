using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface ISalarySlipService
    {
        DataTable GetEmpDetails(string empid);
        DataTable GetAllSalarySlipGrid(string strStatus);
        DataTable GetEmpName();
        string SalarySlipCRUD(SalarySlip Cy);
        string StatusChange(string tag, string id);
    }
}
