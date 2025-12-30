using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface ISalaryStructure
    {
        //DataTable GetAllAmtDetails(string allamtid);
        DataTable GetAllSalaryStructureGrid(string strStatus);
        DataTable GetEditSalaryStructure(string id);
        DataTable GetEmpName();
        string SalaryStructureCRUD(SalaryStructure Cy);
        string StatusChange(string tag, string id);
    }
}
