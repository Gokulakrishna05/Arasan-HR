using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface
{
    public interface IDepartment
    {
        string DepartmentCRUD(Department ss);

        DataTable GetDepartment(string id);
        DataTable GetDesign();

        //DataTable GetDepartmentDetail(string id);

        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetAllDEPARTMENT(string strStatus);
        DataTable GetAllPDEPARTMENT(string strStatus);
        DataTable GetPDepartment(string id);
        string PDepartmentCRUD(Department ss);
    }
}
